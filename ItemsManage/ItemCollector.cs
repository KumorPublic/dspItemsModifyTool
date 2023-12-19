using CommonAPI.Systems;
using CommonAPI.Systems.ModLocalization;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace ItemsManage
{
    ///<summary>
    ///星球矿机
    ///</summary>
    class ItemCollector
    {
        private static ItemProto oldItem;
        private static RecipeProto oldRecipe;
        private static ModelProto oldModel;
        private static ItemProto NewItem;
        private static RecipeProto NewRecipe;
        private static ModelProto NewModel;

        private static long frame = 0L;
        private static uint seed = 100000U;


        public static void AddItem()
        {
            // 添加字符串
            LocalizationModule.RegisterTranslation("轨道开采站",
                "PlantCollector",
                "轨道开采站",
                "PlantCollector"
                );
            LocalizationModule.RegisterTranslation("轨道开采站描述",
                "Minerals that can mine entire planets",
                "可以开采整个行星的矿物",
                "Minerals that can mine entire planets"
                );
            
            oldItem = LDB.items.Select(ProtoID.物品.星际物流运输站);
            oldRecipe = LDB.recipes.Select(ProtoID.配方.星际物流运输站);
            oldModel = LDB.models.Select(oldItem.ModelIndex);

            // 注册原型
            int ItemID = ProtoID.物品.轨道开采站;
            string IconPath = LDB.items.Select(ProtoID.物品.轨道采集器).IconPath;
            int Grid = ProtoID.GRID.轨道开采站;
            NewItem = ProtoRegistry.RegisterItem(ItemID, "轨道开采站", "轨道开采站描述", IconPath, Grid, 50, EItemType.Logistics);

            // 注册配方
            int RecipeID = ProtoID.配方.轨道开采站;
            ERecipeType RecipeType = oldRecipe.Type;
            int TimeSpend = 300 * myConst.Second;
            int[] Input = new int[5] { ProtoID.物品.星际物流运输站, ProtoID.物品.能量核心满, ProtoID.物品.大型采矿机, ProtoID.物品.抽水站, ProtoID.物品.原油萃取站 };
            int[] InCount = new int[5] { 1, 1, 10, 20, 20 };
            int[] Output = new int[1] { NewItem.ID };
            int[] OutCount = new int[1] { 1 };
            int preTech = ProtoID.Tech.光子聚束采矿科技;
            NewRecipe = ProtoRegistry.RegisterRecipe(RecipeID, RecipeType, TimeSpend, Input, InCount, Output, OutCount, "轨道开采站描述", preTech, Grid, "轨道开采站", IconPath);

            // 注册模型
            int ModelID = ProtoID.ModelID.轨道开采站;
            string PrefabPath = oldModel.PrefabPath;
            int[] descFieids = oldItem.DescFields;
            int BuildID = ProtoID.BuildID.轨道开采站;
            NewModel = ProtoRegistry.RegisterModel(ModelID, NewItem, PrefabPath, null, descFieids, BuildID);


            ProtoRegistry.onLoadingFinished += new Action(Finished);

        }



        private static void Finished()
        {
            Material material = UnityEngine.Object.Instantiate<Material>(NewModel.prefabDesc.lodMaterials[0][0]);
            material.color = Color.red;
            NewModel.prefabDesc.lodMaterials[0][0] = material;
            NewModel.prefabDesc.stationMaxEnergyAcc = ItemConfig.CollectorCfg.MaxEnergyAcc;
            NewModel.prefabDesc.workEnergyPerTick = ItemConfig.CollectorCfg.MaxEnergyPerTick;
            //ItemManagePlugin.logger.LogInfo("maxAcuEnergy::" + NewModel.prefabDesc.maxAcuEnergy.ToString());
            //ItemManagePlugin.logger.LogInfo("maxAcuEnergy::" + NewModel.prefabDesc.maxExcEnergy.ToString());
            //ItemManagePlugin.logger.LogInfo("maxAcuEnergy::" + NewModel.prefabDesc.stationMaxEnergyAcc.ToString());
            //ItemManagePlugin.logger.LogInfo("maxAcuEnergy::" + NewModel.prefabDesc.workEnergyPerTick.ToString());
            //ItemManagePlugin.logger.LogInfo("maxAcuEnergy::" + NewModel.prefabDesc.idleEnergyPerTick.ToString());
            
            NewModel.HpMax = 150000;
            NewModel.HpRecover = 1000;
            NewItem.HpMax = 150000;
        }





        [HarmonyPostfix]
        [HarmonyPatch(typeof(FactorySystem), "GameTickLabResearchMode")]
        public static void FactorySystemGameTick(ref FactorySystem __instance)
        {
            float miningSpeedScale = GameMain.history.miningSpeedScale;//采矿速度
            float miningCostRate = GameMain.history.miningCostRate;//消耗速率
            int needEnergy;

            if ((double)miningSpeedScale <= 0.0)
            {
                return;
            }
            //原始采矿速度下，每120Tick开采一次
            int miningFrame = (int)(120.0 / (double)miningSpeedScale);
            if (miningFrame < 1)
            {
                miningFrame = 1;//如果采矿速度无限大，则每Tick开采一次
            }

            if ((ulong)frame % (ulong)miningFrame > 0UL)
            {
                return;
            }

            VeinData[] veinPool = __instance.factory.veinPool;
            Dictionary<int, List<int>> veins = new Dictionary<int, List<int>>();
            if (__instance.minerPool[0].seed == 0U)
            {
                System.Random random = new System.Random();
                __instance.minerPool[0].seed = (uint)(__instance.planet.id * 100000 + random.Next(1, 9999));
            }
            else
            {
                seed = __instance.minerPool[0].seed;
            }


            for (int index = 0; index < veinPool.Length; ++index)
            {
                VeinData veinData = veinPool[index];
                if (veinData.amount > 0 && veinData.productId > 0)
                {
                    AddVeinData(veins, veinData.productId, index);
                }

            }
            //注册生产统计
            int[] numArray = (int[])null;
            bool flag = false;
            if (GameMain.statistics.production.factoryStatPool[__instance.factory.index] != null)
            {
                numArray = GameMain.statistics.production.factoryStatPool[__instance.factory.index].productRegister;
                flag = true;
            }
            //扫描星球上每个运输站
            foreach (StationComponent stationComponent in __instance.planet.factory.transport.stationPool)
            {
                if (stationComponent == null || stationComponent.storage == null)
                {
                    continue;
                }
                //匹配开采站的id
                if (__instance.planet.factory.entityPool[stationComponent.entityId].protoId != ProtoID.物品.轨道开采站)
                {
                    continue;
                }
                //计算开采倍率
                int energyMultiplier = 0;
                int Multiplier = GetMultiplier(__instance.planet.factory.powerSystem.consumerPool[stationComponent.pcId].workEnergyPerTick, stationComponent.energy, ref energyMultiplier);
                //ItemManagePlugin.logger.LogInfo("Multiplier = " + Multiplier.ToString());
                //ItemManagePlugin.logger.LogInfo("energyMultiplier = " + energyMultiplier.ToString());

                //检查能量是否足够
                if (stationComponent.energy < ItemConfig.CollectorCfg.UseEnergy * energyMultiplier)
                {
                    continue;
                }
                needEnergy = 0;
                //扫描开采站每个槽位
                for (int StorageIndex = 0; StorageIndex < stationComponent.storage.Length; ++StorageIndex)
                {
                    StationStore Store = stationComponent.storage[StorageIndex];
                    //ItemManagePlugin.logger.LogInfo("StorageIndex = " + StorageIndex.ToString());
                    //ItemManagePlugin.logger.LogInfo("energy = " + stationComponent.energy.ToString());
                    //ItemManagePlugin.logger.LogInfo("energyMax = " + stationComponent.energyMax.ToString());
                    //ItemManagePlugin.logger.LogInfo("warningId = " + __instance.planet.factory.entityPool[stationComponent.entityId].warningId.ToString());

                    //消耗燃料充电, 能量低，储量大于50，并且是原油或煤
                    if (stationComponent.energy < ItemConfig.CollectorCfg.MaxFuelEnergyAcc && Store.count > 50)
                    {
                        if (Store.itemId == ProtoID.物品.原油)
                        {
                            stationComponent.storage[StorageIndex].count -= 50;//减掉库存
                            stationComponent.energy += (long)(50 * 4050000 * 5); //4.05MJ

                        }
                        else if (Store.itemId == ProtoID.物品.煤)
                        {
                            stationComponent.storage[StorageIndex].count -= 50;//减掉库存
                            stationComponent.energy += (long)(50 * 270000 * 5); //2.7MJ
                        }
                    }
                    if (Store.max > Store.count)//如果库存未满
                    {
                        if (veins.ContainsKey(Store.itemId))//如果星球上存在目标矿物
                        {
                            int veinsIndex = veins[Store.itemId].First<int>();//获取目标矿物索引
                            if (veinPool[veinsIndex].type == EVeinType.Oil)//如果是油
                            {
                                int numOil = 0;//矿物数量
                                //遍历星球上每个与itemId匹配的矿物索引
                                foreach (int i in veins[Store.itemId])
                                {
                                    if (veinPool.Length > i && veinPool[i].productId > 0)//检查索引合法性
                                    {
                                        if (GetMine(veinPool, i, miningCostRate, __instance.planet.factory, Multiplier))
                                        {
                                            numOil += ItemConfig.CollectorCfg.CollectorOilNum * Multiplier;//增加油
                                        }
                                        else
                                        {
                                            //如果石油枯竭，50%概率增加1个油
                                            if (new System.Random().Next(1, 100) > 50)
                                            {
                                                numOil++;
                                            }
                                        }
                                    }
                                }
                                //如果获得的矿物>0，则计算耗电量、库存和统计数据
                                if (numOil > 0f)
                                {
                                    needEnergy++;
                                    stationComponent.storage[StorageIndex].count += (int)numOil;
                                    if (flag)
                                    {
                                        numArray[Store.itemId] += (int)numOil;
                                    }
                                }

                            }
                            else
                            {//如果是其他矿
                                int numVein = 0;
                                int MineTime = 0;//只挖取前 n 个矿，产量大约 n * 倍率 个/s
                                foreach (int i in veins[Store.itemId])
                                {
                                    if (GetMine(veinPool, i, miningCostRate, __instance.planet.factory, Multiplier))
                                    {
                                        MineTime++;
                                        numVein += ItemConfig.CollectorCfg.CollectorMineNum * Multiplier;
                                    }
                                    if (MineTime > 99)
                                    {
                                        break;
                                    }
                                }
                                //如果获得的矿物>0，则计算耗电量、库存和统计数据
                                if (numVein > 0)
                                {
                                    needEnergy++;
                                    stationComponent.storage[StorageIndex].count += numVein;
                                    if (flag)
                                    {
                                        numArray[Store.itemId] += numVein;
                                    }
                                }


                            }
                        }
                        else if (Store.itemId == __instance.planet.waterItemId)//如果开采的是水或硫酸
                        {
                            int numWater = 0;
                            float c = 1 - __instance.planet.landPercent;//根据海洋面积计算开采量
                            if (c > 0)
                            {
                                numWater = (int)(ItemConfig.CollectorCfg.CollectorWaterNum * c) * Multiplier;
                            }

                            if (numWater > 0)
                            {
                                needEnergy++;
                                stationComponent.storage[StorageIndex].count += numWater;
                                if (flag)
                                {
                                    numArray[Store.itemId] += numWater;
                                }
                            }

                        }

                    }
                }

                if (needEnergy > 0)
                {
                    //stationComponent.energy -= (long)(ItemConfig.CollectorCfg.UseEnergy * needEnergy * energyMultiplier / (120 / miningFrame));
                    stationComponent.energy -= (long)(ItemConfig.CollectorCfg.UseEnergy * needEnergy * energyMultiplier);//采矿速度越高，能耗越高
                }




            }

        }
        //在界面显示挖矿倍率
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UIStationWindow), "OnMaxChargePowerSliderValueChange")]
        public static void UIStationWindowOnMaxChargePowerSliderValueChange(UIStationWindow __instance, float value)
        {
            if (__instance.stationId == 0 || __instance.factory == null)
            {
                return;
            }
            if (Traverse.Create(__instance).Field("event_lock").GetValue<bool>())
            {
                return;
            }
            StationComponent stationComponent = __instance.transport.stationPool[__instance.stationId];
            if (stationComponent == null || stationComponent.id != __instance.stationId)
            {
                return;
            }
            if (__instance.factory.entityPool[stationComponent.entityId].protoId != ProtoID.物品.轨道开采站)
            {
                return;
            }
            int energyMultiplier = 0;
            int Multiplier = GetMultiplier(__instance.factory.powerSystem.consumerPool[stationComponent.pcId].workEnergyPerTick, stationComponent.energy, ref energyMultiplier);
            //__instance.maxChargePowerValue.rectTransform.localScale = new Vector3(10,1,1);
            //__instance.maxChargePowerValue.text = "效率:"+Multiplier.ToString() + "00%" ;
            //ItemManagePlugin.logger.LogInfo(__instance.maxChargePowerValue.text);
            //ItemManagePlugin.logger.LogInfo("效率: " + Multiplier.ToString() + "× 能耗: " + energyMultiplier.ToString() + "×" + ((int)(ItemConfig.CollectorCfg.UseEnergy / 1000000)).ToString() + "MJ×5");
            string name = "轨道开采站".Translate() + " #" + stationComponent.gid.ToString() + " 效率:" + Multiplier.ToString() + "00% 能耗:" + ((int)(ItemConfig.CollectorCfg.UseEnergy / 1000000) * energyMultiplier * 5).ToString() + "MW";
            __instance.nameInput.text = name;
        }

        //计算挖矿倍率
        private static int GetMultiplier(long workEnergyPerTick, long energy, ref int energyMultiplier)
        {
            int Multiplier = (int)(workEnergyPerTick / (ItemConfig.CollectorCfg.MaxEnergyPerTick1 / 3));
            //倍数必须在1-3之间
            //Multiplier = Multiplier > 3 ? 3 : Multiplier < 1 ? 1 : energy < ItemConfig.CollectorCfg.MaxFuelEnergyAcc ? 1 : Multiplier;
            Multiplier = Multiplier > 3 ? 3 : Multiplier < 1 ? 1 : Multiplier;
            energyMultiplier = Multiplier * Multiplier;
            return Multiplier;
        }

        //调整界面
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UIStationWindow), "OnStationIdChange")]
        public static void UIStationWindowOnStationIdChange(UIStationWindow __instance)
        {
            if (!__instance.active)
            {
                return;
            }
            if (__instance.stationId == 0 || __instance.factory == null)
            {
                return;
            }
            if (__instance.transport?.stationPool == null)
            {
                return;
            }
            StationComponent stationComponent = __instance.transport.stationPool[__instance.stationId];
            ItemProto itemProto = LDB.items.Select((int)__instance.factory.entityPool[stationComponent.entityId].protoId);
            if (itemProto == null)
            {
                return;
            }
            if (itemProto.ID != ProtoID.物品.轨道开采站)
            {
                __instance.droneIconButton.gameObject.SetActive(true);
                return;
            }
            int energyMultiplier = 0;
            int Multiplier = GetMultiplier(__instance.factory.powerSystem.consumerPool[stationComponent.pcId].workEnergyPerTick, stationComponent.energy, ref energyMultiplier);
            string name = "轨道开采站".Translate() + " #" + stationComponent.gid.ToString() + " 效率:" + Multiplier.ToString() + "00% 能耗:" + ((int)(ItemConfig.CollectorCfg.UseEnergy / 1000000) * energyMultiplier * 5).ToString() + "MW";
            //stationComponent.name = "轨道开采站".Translate() + " #" + stationComponent.gid.ToString() + " 效率:" + Multiplier.ToString() + "00% 能耗:" + ((int)(ItemConfig.CollectorCfg.UseEnergy / 1000000) * energyMultiplier * 5).ToString() + "MW";
            __instance.nameInput.text = name;

            __instance.powerGroupRect.sizeDelta = new Vector2(540f, 40f);

            __instance.panelDown.SetActive(true);
            __instance.shipIconButton.gameObject.SetActive(false);
            __instance.warperIconButton.gameObject.SetActive(false);
            __instance.configGroup.gameObject.SetActive(true);
            __instance.maxChargePowerGroup.gameObject.SetActive(true);
            __instance.maxTripDroneGroup.gameObject.SetActive(false);
            __instance.maxTripVesselGroup.gameObject.SetActive(false);
            __instance.includeOrbitCollectorGroup.gameObject.SetActive(false);
            __instance.warperDistanceGroup.gameObject.SetActive(false);
            __instance.warperNecessaryGroup.gameObject.SetActive(false);
            __instance.minDeliverDroneGroup.gameObject.SetActive(false);
            __instance.minDeliverVesselGroup.gameObject.SetActive(false);
            __instance.droneIconButton.gameObject.SetActive(false);

            //ItemsModifyToolPlugin.logger.LogInfo("itemProto.ID   " + stationComponent.name);
            //ItemsModifyToolPlugin.logger.LogInfo("itemProto.ID   " + __instance.factory.powerSystem.consumerPool[stationComponent.pcId].workEnergyPerTick.ToString());
        }

        //调整界面
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UIStationWindow), "_OnUpdate")]
        public static void UIStationWindow_OnUpdate(UIStationWindow __instance)
        {

            if (__instance.stationId == 0 || __instance.factory == null)
            {
                return;
            }
            if (__instance.transport?.stationPool == null)
            {
                return;
            }
            StationComponent stationComponent = __instance.transport.stationPool[__instance.stationId];
            if (stationComponent == null || stationComponent.id != __instance.stationId)
            {
                return;
            }
            ItemProto itemProto = LDB.items.Select((int)__instance.factory.entityPool[stationComponent.entityId].protoId);
            if (itemProto == null)
            {
                return;
            }
            //如是轨道开采站，则隐藏翘曲器
            if (itemProto.ID != ProtoID.物品.轨道开采站)
            {
                return;
            }
            __instance.warperIconButton.gameObject.SetActive(false);
        }


        public static void SetFrame()
        {
            ++ItemCollector.frame;
        }

        //创建矿物id-索引字典
        public static void AddVeinData(Dictionary<int, List<int>> veins, int item, int index)
        {
            if (!veins.ContainsKey(item))
            {
                veins.Add(item, new List<int>());
            }
            veins[item].Add(index);
        }

        //计算本次采矿相关数值
        //参考方法 MinerComponent.InternalUpdate()
        public static bool GetMine(VeinData[] veinPool, int veinIndex, float miningCostRate, PlanetFactory factory, int Multiplier)
        {
            if (veinPool.Length <= veinIndex || veinPool[veinIndex].productId <= 0)//检查索引合法性
                return false;

            if (veinPool[veinIndex].type == EVeinType.Oil)//如果是油
            {
                if (veinPool[veinIndex].amount > 2500)
                {
                    //未枯竭
                    if ((double)miningCostRate > 0.0)
                    {
                        seed = (uint)((ulong)(seed % 2147483646U + 1U) * 48271UL % (ulong)int.MaxValue) - 1U;
                        if ((double)seed / 2147483646.0 < (double)miningCostRate)
                        {
                            veinPool[veinIndex].amount -= ItemConfig.CollectorCfg.CollectorOilNum * Multiplier;//单个矿物总数-
                            factory.veinGroups[(int)veinPool[veinIndex].groupIndex].amount -= ItemConfig.CollectorCfg.CollectorMineNum * Multiplier;//地表矿堆总数-
                            factory.veinAnimPool[veinIndex].time = veinPool[veinIndex].amount >= 25000 ? 0.0f : (float)(1.0 - (double)veinPool[veinIndex].amount * (double)VeinData.oilSpeedMultiplier);//矿物贴图动画减少
                        }
                    }
                    return true;
                }
                else
                {
                    //已枯竭
                    return false;
                }

            }
            else
            {
                if (veinPool[veinIndex].amount > 0)
                {
                    bool flag = true;
                    //如果消耗率<1，计算本次采矿是否消耗的概率
                    if ((double)miningCostRate < 0.999989986419678)
                    {
                        seed = (uint)((ulong)(seed % 2147483646U + 1U) * 48271UL % (ulong)int.MaxValue) - 1U;
                        flag = (double)seed / 2147483646.0 < (double)miningCostRate;
                    }
                    if (flag)
                    {
                        veinPool[veinIndex].amount -= ItemConfig.CollectorCfg.CollectorMineNum * Multiplier;//单个矿物总数-
                        factory.veinGroups[(int)veinPool[veinIndex].groupIndex].amount -= ItemConfig.CollectorCfg.CollectorMineNum * Multiplier;//地表矿堆总数-
                        factory.veinAnimPool[veinIndex].time = veinPool[veinIndex].amount >= 20000 ? 0.0f : (float)(1.0 - (double)veinPool[veinIndex].amount * 4.99999987368938E-05);

                        //如果矿物耗尽则删除矿堆
                        if (veinPool[veinIndex].amount <= 0)
                        {
                            factory.RemoveVeinWithComponents(veinIndex);
                            factory.RecalculateVeinGroup(veinPool[veinIndex].groupIndex);
                            factory.NotifyVeinExhausted();
                        }
                    }
                    return true;
                }
                else
                {
                    //如果矿物耗尽则删除矿堆
                    factory.RemoveVeinWithComponents(veinIndex);
                    factory.RecalculateVeinGroup(veinPool[veinIndex].groupIndex);
                    factory.NotifyVeinExhausted();
                    return false;
                }


            }




        }

    }



}
