using CommonAPI.Systems;
using CommonAPI.Systems.ModLocalization;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;


namespace ItemsManage
{
    ///<summary>
    ///星球矿机
    ///</summary>
    public class ItemCollector
    {
        private static ItemProto oldItem;
        private static RecipeProto oldRecipe;
        private static ModelProto oldModel;
        private static ItemProto NewItem;
        private static RecipeProto NewRecipe;
        private static ModelProto NewModel;

        private static double costFrac;// 它用于将浮点采矿速率转为整数消耗，避免因速率过低导致采集不到资源

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
                "可以快速开采整个行星的矿物，但是矿物利用率不高（损失20%的矿物）",
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

            float tick = 120.0f;


            if (miningSpeedScale <= 0f)
            {
                return;
            }

            // 原始采矿速度下，每120Tick开采一次
            int miningFrame = (int)(tick / miningSpeedScale);
            if (miningFrame < 12)
            {
                miningFrame = 12; // 采矿速度上限为1000%
            }
            
            if (ItemManagePlugin.frame % miningFrame > 0)
            {
                // 如果有余数，说明不到采矿帧
                return;
            }

            //ItemManagePlugin.logger.LogInfo("tick = " + tick.ToString()+ " frame = "+ frame.ToString());

            float miningCostRate = GameMain.history.miningCostRate; // 消耗速率
            int needEnergy;

            // 获取矿池
            VeinData[] veinPool = __instance.factory.veinPool;
            Dictionary<int, List<int>> veins = new Dictionary<int, List<int>>();
            
            for (int index = 0; index < veinPool.Length; ++index)
            {
                VeinData veinData = veinPool[index];
                if (veinData.amount > 0 && veinData.productId > 0)
                {
                    AddVeinData(veins, veinData.productId, index);
                }

            }
            // 注册生产统计
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
                // 匹配开采站的id
                if (__instance.planet.factory.entityPool[stationComponent.entityId].protoId != ProtoID.物品.轨道开采站)
                {
                    continue;
                }
                // 计算开采倍率
                float energyMultiplier = 0;
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
                        else if (Store.itemId == ProtoID.物品.可燃冰)
                        {
                            stationComponent.storage[StorageIndex].count -= 50;//减掉库存
                            stationComponent.energy += (long)(50 * 480000 * 5); //2.7MJ
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
                                            numOil += (int)(ItemConfig.CollectorCfg.CollectorOilNum * Multiplier);//增加油
                                        }
                                        else
                                        {
                                            //如果石油枯竭，50%概率增加1个油
                                            //if (new System.Random().Next(1, 100) > 50)
                                            //{
                                            //    numOil++;
                                            //}
                                            //numOil += Multiplier; // 增加油
                                            // 油井枯竭后无法采集
                                        }
                                    }
                                }
                                //如果获得的矿物>0，则计算耗电量、库存和统计数据
                                if (numOil > 0)
                                {
                                    needEnergy++;
                                    numOil = (int)(numOil * 0.8f); // 没收20%的矿
                                    stationComponent.storage[StorageIndex].count += numOil;
                                    if (flag)
                                    {
                                        numArray[Store.itemId] += numOil;
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
                                        numVein += (int)(ItemConfig.CollectorCfg.CollectorMineNum * Multiplier);
                                    }
                                    if (MineTime > 500)
                                    {
                                        break;
                                    }
                                }
                                //如果获得的矿物>0，则计算耗电量、库存和统计数据
                                if (numVein > 0)
                                {
                                    needEnergy++;
                                    //ItemManagePlugin.logger.LogInfo("numVein = " + numVein.ToString());
                                    numVein = (int)(numVein * 0.8f); // 没收20%的矿
                                    //ItemManagePlugin.logger.LogInfo("numVein = " + numVein.ToString());
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
                                numWater = (int)(ItemConfig.CollectorCfg.CollectorWaterNum * c * Multiplier);
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
        
        // 在界面显示挖矿倍率
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
            float energyMultiplier = 0;
            float Multiplier = GetMultiplier(__instance.factory.powerSystem.consumerPool[stationComponent.pcId].workEnergyPerTick, stationComponent.energy, ref energyMultiplier);
            //__instance.maxChargePowerValue.rectTransform.localScale = new Vector3(10,1,1);
            //__instance.maxChargePowerValue.text = "效率:"+Multiplier.ToString() + "00%" ;
            //ItemManagePlugin.logger.LogInfo(__instance.maxChargePowerValue.text);
            //ItemManagePlugin.logger.LogInfo("效率: " + Multiplier.ToString() + "× 能耗: " + energyMultiplier.ToString() + "×" + ((int)(ItemConfig.CollectorCfg.UseEnergy / 1000000)).ToString() + "MJ×5");
            
            string name = "轨道开采站".Translate() + " #" + stationComponent.gid.ToString() + " 效率:" + (Multiplier * 100).ToString("F0") + " %" + " 能耗:" + ((int)(ItemConfig.CollectorCfg.UseEnergy / 1000000) * energyMultiplier * 5).ToString("F2") + " MW";
            __instance.nameInput.text = name;
        }

        //计算挖矿倍率
        private static int GetMultiplier(long workEnergyPerTick, long energy, ref float energyMultiplier)
        {
            int Multiplier = (int)(workEnergyPerTick / (ItemConfig.CollectorCfg.MaxEnergyPerTick1 / 4));
            //倍数必须在1-3之间
            Multiplier = Multiplier > 4 ? 4 : Multiplier < 1 ? 1 : Multiplier;
            energyMultiplier = (float)Math.Pow(Multiplier, 1.6);
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
            float energyMultiplier = 0;
            float Multiplier = GetMultiplier(__instance.factory.powerSystem.consumerPool[stationComponent.pcId].workEnergyPerTick, stationComponent.energy, ref energyMultiplier);
            string name = "轨道开采站".Translate() + " #" + stationComponent.gid.ToString() + " 效率:" + (Multiplier * 100).ToString("F0") + " %" + " 能耗:" + ((int)(ItemConfig.CollectorCfg.UseEnergy / 1000000) * energyMultiplier * 5).ToString("F2") + " MW";
            //stationComponent.name = "轨道开采站".Translate() + " #" + stationComponent.gid.ToString() + " 效率:" + Multiplier.ToString() + "00% 能耗:" + ((int)(ItemConfig.CollectorCfg.UseEnergy / 1000000) * energyMultiplier * 5).ToString() + "MW";
            __instance.nameInput.text = name;

            //__instance.windowTrans.sizeDelta = new Vector2(600f, (float)(100 + 76 * 5 + 36));

            __instance.powerGroupRect.sizeDelta = new Vector2(540f, 40f);


            
           

            __instance.panelDown.SetActive(true);

            //__instance.transportTabButton.gameObject.SetActive(false);
            //__instance.settingTabButton.gameObject.SetActive(false);
            //__instance.sepLineButton.gameObject.SetActive(false);

            

            __instance.shipIconButton.gameObject.SetActive(false);
            __instance.shipAutoReplenishButton.gameObject.SetActive(false);
            __instance.droneIconButton.gameObject.SetActive(false);
            __instance.warperIconButton.gameObject.SetActive(false);
            // 启用配置面板
            __instance.configGroup.gameObject.SetActive(true);
            // 禁用优先级面板
            __instance.priorityGroup.gameObject.SetActive(false);
            // 修改功率
            __instance.maxChargePowerGroup.gameObject.SetActive(true);
            // 飞机
            __instance.maxTripDroneGroup.gameObject.SetActive(false);
            __instance.maxTripVesselGroup.gameObject.SetActive(false);
            // 是否从采集器收取货物
            __instance.includeOrbitCollectorGroup.gameObject.SetActive(false);
            // 曲速
            __instance.warperDistanceGroup.gameObject.SetActive(false);
            __instance.warperNecessaryGroup.gameObject.SetActive(false);
            // 最小运输量
            __instance.minDeliverDroneGroup.gameObject.SetActive(false);
            __instance.minDeliverVesselGroup.gameObject.SetActive(false);
            
            // 自动补充飞船
            __instance.shipAutoReplenishButton.gameObject.SetActive(false);
            __instance.droneAutoReplenishButton.gameObject.SetActive(false);



            // 开采速度
            //__instance.maxMiningSpeedGroup.gameObject.SetActive(true);
            //__instance.maxMiningSpeedGroup.anchoredPosition = new Vector2(__instance.maxChargePowerGroup.anchoredPosition.x, -24f);

            // 是否堆叠
            //__instance.minPilerGroup.gameObject.SetActive(false);
            //__instance.pilerTechGroup.gameObject.SetActive(false);
            //__instance.minPilerGroup.anchoredPosition = new Vector2(__instance.minPilerGroup.anchoredPosition.x, -44f);
            //__instance.pilerTechGroup.anchoredPosition = new Vector2(__instance.pilerTechGroup.anchoredPosition.x, -44f);

            //__instance.droneBox.SetActive(false);
            //__instance.shipBox.SetActive(false);
            //__instance.warperBox.SetActive(false);
            //__instance.powerStateObj.SetActive(false);


            __instance.stationSettingTabBtn.gameObject.SetActive(value: false);
            __instance.prioritySettingTabBtn.gameObject.SetActive(value: false);
            __instance.configGroup.gameObject.SetActive(value: false);
            __instance.priorityGroup.gameObject.SetActive(value: false);
            __instance.uiRoutePanel.gameObject.SetActive(false);

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
        public static bool GetMine(VeinData[] veinPool, int veinIndex, float miningRate, PlanetFactory factory, int Multiplier)
        {
            if (veinPool.Length <= veinIndex || veinPool[veinIndex].productId <= 0)//检查索引合法性
                return false;

            if (veinPool[veinIndex].type == EVeinType.Oil)//如果是油
            {
                // 油是否枯竭
                if (veinPool[veinIndex].amount > 2500)
                {
                    // 把每次采矿的小数点累计起来，满1则扣除
                    bool flag = false;
                    if (miningRate > 0f)
                    {
                        costFrac += miningRate;
                        flag = (int)costFrac > 0;
                        costFrac -= (flag ? 1 : 0);
                    }
                    if (flag)
                    {
                        veinPool[veinIndex].amount -= ItemConfig.CollectorCfg.CollectorOilNum * Multiplier;//单个矿物总数-
                        factory.veinGroups[(int)veinPool[veinIndex].groupIndex].amount -= ItemConfig.CollectorCfg.CollectorMineNum * Multiplier;//地表矿堆总数-
                        factory.veinAnimPool[veinIndex].time = veinPool[veinIndex].amount >= 25000 ? 0.0f : (float)(1.0 - (double)veinPool[veinIndex].amount * (double)VeinData.oilSpeedMultiplier);//矿物贴图动画减少
                    }
                    return true;
                }
                else
                {
                    // 已枯竭
                    return false;
                }

            }
            else
            {
                if (veinPool[veinIndex].amount > 0)
                {
                    // 把每次采矿的小数点累计起来，满1则扣除
                    bool flag = false;
                    if (miningRate > 0f)
                    {
                        costFrac += miningRate;
                        flag = (int)costFrac > 0;
                        costFrac -= (flag ? 1 : 0);
                    }

                    if (flag)
                    {
                        veinPool[veinIndex].amount -= ItemConfig.CollectorCfg.CollectorMineNum * Multiplier;//单个矿物总数-
                        factory.veinGroups[(int)veinPool[veinIndex].groupIndex].amount -= ItemConfig.CollectorCfg.CollectorMineNum * Multiplier;//地表矿堆总数-
                        //如果矿物耗尽则删除矿堆
                        if (veinPool[veinIndex].amount <= 0)
                        {
                            short groupIndex = veinPool[veinIndex].groupIndex;
                            factory.veinGroups[groupIndex].count--;
                            factory.RemoveVeinWithComponents(veinIndex);
                            factory.RecalculateVeinGroup(groupIndex);
                        }
                    }

                    return true;
                }
                else
                {
                    //如果矿物耗尽则删除矿堆

                    short groupIndex = veinPool[veinIndex].groupIndex;
                    factory.veinGroups[groupIndex].count--;
                    factory.RemoveVeinWithComponents(veinIndex);
                    factory.RecalculateVeinGroup(groupIndex);
                    return false;
                }


            }




        }

    }



}
