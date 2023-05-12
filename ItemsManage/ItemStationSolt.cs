using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ItemsManage
{
    ///<summary>
    ///修改运输站可选重复槽位, 重写了原方法，游戏更新时需要注意
    ///</summary>
    class ItemStationSolt
    {
        /*
        //重写了原方法，游戏更新时需要注意
        [HarmonyPrefix]
        [HarmonyPatch(typeof(UIStationStorage), "OnItemPickerReturn")]
        public static bool OnItemPickerReturn(UIStationStorage __instance, ItemProto itemProto)
        {
            if (itemProto == null || __instance.station == null || __instance.index >= __instance.station.storage.Length)
                return false;
            ItemProto itemProto1 = LDB.items.Select((int)__instance.stationWindow.factory.entityPool[__instance.station.entityId].protoId);
            if (itemProto1 == null)
                return false;
            int additionStorage = __instance.GetAdditionStorage();
            __instance.stationWindow.transport.SetStationStorage(__instance.station.id, __instance.index, itemProto.ID, itemProto1.prefabDesc.stationMaxItemCount + additionStorage, ELogisticStorage.Supply, __instance.station.isStellar ? ELogisticStorage.Supply : ELogisticStorage.None, GameMain.mainPlayer);
            return false;
        }*/

        //修改字节码方式
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(UIStationStorage), "OnItemPickerReturn")]
        public static IEnumerable<CodeInstruction> UIStationStorageOnItemPickerReturn(
        IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> list = instructions.ToList<CodeInstruction>();
            list[42].opcode = OpCodes.Nop;
            list[43].opcode = OpCodes.Nop;
            list[44].opcode = OpCodes.Nop;
            list[45].opcode = OpCodes.Nop;
            list[46].opcode = OpCodes.Nop;
            list[47].opcode = OpCodes.Nop;
            return ((IEnumerable<CodeInstruction>)list).AsEnumerable<CodeInstruction>();
        }


        [HarmonyPrefix]
        [HarmonyPatch(typeof(StationComponent), "AddItem")]
        [HarmonyPriority(Priority.First)]
        public static bool StationComponentAddItem(StationComponent __instance, ref int itemId, ref int count, ref int inc, ref int __result)
        {
            __result = 0;
            if (itemId <= 0)
            {
                return false;
            }
            lock (__instance.storage)
            {
                int index0 = -1;//最后一个itemid命中的位置，如果其他条件不符合，货物就放在这里
                int _count = count;
                count = 0;//防止别的mod再次累加

                int dt = 0;
                int max = 0;
                int indexHit = -1;

                //扫描哪个槽位的货物最少
                for (int index = 0; index < __instance.storage.Length; ++index)
                {
                    if ((__instance.storage[index].localLogic == ELogisticStorage.Demand || __instance.storage[index].remoteLogic == ELogisticStorage.Demand) && __instance.storage[index].itemId == itemId)
                    {
                        index0 = index;//itemID命中
                        dt = __instance.storage[index].max - __instance.storage[index].count;
                        //ItemManagePlugin.logger.LogInfo("index=" + index.ToString() + ",localDemandCount = " + __instance.storage[index].localDemandCount.ToString() );
                        //ItemManagePlugin.logger.LogInfo("index=" + index.ToString() + "localOrder = " + __instance.storage[index].localOrder.ToString());
                        //ItemManagePlugin.logger.LogInfo("index=" + index.ToString() + "localSupplyCount = " + __instance.storage[index].localSupplyCount.ToString());
                        //ItemManagePlugin.logger.LogInfo("index=" + index.ToString() + "totalOrdered = " + __instance.storage[index].totalOrdered.ToString());
                        if (max < dt)
                        {
                            max = dt;
                            indexHit = index;

                        }
                    }
                }
                //ItemManagePlugin.logger.LogInfo("indexHit="+ indexHit.ToString() + ",dt = " + dt.ToString() + ",max=" + max.ToString());
                if (max > 0 && indexHit > -1)
                {
                    __instance.storage[indexHit].count += _count;
                    __instance.storage[indexHit].inc += inc;
                    __result = _count;

                }
                else if (index0 > -1)
                {
                    __instance.storage[index0].count += _count;
                    __instance.storage[index0].inc += inc;
                    __result = _count;
                }

            }
            return false;
        }


        //patch重载函数，匹配参数和参数类型
        [HarmonyPrefix]
        [HarmonyPatch(typeof(StationComponent), "TakeItem", new Type[] { typeof(int), typeof(int), typeof(int) }, new ArgumentType[] { ArgumentType.Ref, ArgumentType.Ref, ArgumentType.Out })]
        [HarmonyPriority(Priority.First)]
        public static bool StationComponentTakeItem2(StationComponent __instance, ref int itemId, ref int count, out int inc)
        {
            int indexHit = -1; //命中标记
            int indexMaxHit = -1; //命中标记
            int max = 0;//最大数量标记
            inc = 0;
            if (itemId > 0 && count > 0)//合法性检测
            {
                //ItemManagePlugin.logger.LogInfo("TakeItem2::" + itemId.ToString() + ",count = " + count.ToString() + ",inc = " + inc.ToString());
                lock (__instance.storage)
                {
                    for (int index = 0; index < __instance.storage.Length; ++index)//扫描每个槽位
                    {
                        if (__instance.storage[index].itemId == itemId && __instance.storage[index].count > 0)//合法性检测
                        {
                            if (count <= __instance.storage[index].count)//如果库存数量大于订单数量就直接取
                            {
                                indexHit = index;
                                indexMaxHit = -1;
                                /**
                                ItemManagePlugin.logger.LogInfo("id = " + index.ToString());
                                ItemManagePlugin.logger.LogInfo("库存 = " + __instance.storage[index].count.ToString());
                                ItemManagePlugin.logger.LogInfo("count = " + count.ToString());
                                ItemManagePlugin.logger.LogInfo("inc = " + inc.ToString());
                                ItemManagePlugin.logger.LogInfo("localDemandCount = " + __instance.storage[index].localDemandCount.ToString());
                                ItemManagePlugin.logger.LogInfo("localLogic = " + __instance.storage[index].localLogic.ToString());
                                ItemManagePlugin.logger.LogInfo("localOrder = " + __instance.storage[index].localOrder.ToString());
                                ItemManagePlugin.logger.LogInfo("localSupplyCount = " + __instance.storage[index].localSupplyCount.ToString());
                                ItemManagePlugin.logger.LogInfo("remoteDemandCount = " + __instance.storage[index].remoteDemandCount.ToString());
                                ItemManagePlugin.logger.LogInfo("remoteLogic = " + __instance.storage[index].remoteLogic.ToString());
                                ItemManagePlugin.logger.LogInfo("remoteOrder = " + __instance.storage[index].remoteOrder.ToString());
                                ItemManagePlugin.logger.LogInfo("remoteSupplyCount = " + __instance.storage[index].remoteSupplyCount.ToString());
                                ItemManagePlugin.logger.LogInfo("totalDemandCount = " + __instance.storage[index].totalDemandCount.ToString());
                                ItemManagePlugin.logger.LogInfo("totalOrdered = " + __instance.storage[index].totalOrdered.ToString());
                                ItemManagePlugin.logger.LogInfo("totalSupplyCount = " + __instance.storage[index].totalSupplyCount.ToString());
                                */

                                break;
                            }
                            else//否则记录最大值，选取库存最多的一个槽搬空
                            {
                                if (max < __instance.storage[index].count)
                                {
                                    max = __instance.storage[index].count;
                                    indexMaxHit = index;
                                }
                            }
                        }
                    }

                    if (indexHit > -1)//直接取
                    {
                        itemId = __instance.storage[indexHit].itemId;
                        inc = split_inc(ref __instance.storage[indexHit].count, ref __instance.storage[indexHit].inc, count);
                        return false;
                    }
                    else if (indexMaxHit > -1)//搬空
                    {
                        count = count < __instance.storage[indexMaxHit].count ? count : __instance.storage[indexMaxHit].count;
                        itemId = __instance.storage[indexMaxHit].itemId;
                        inc = split_inc(ref __instance.storage[indexMaxHit].count, ref __instance.storage[indexMaxHit].inc, count);
                        return false;
                    }
                    //其他情况则让小飞机原路返回

                }
            }
            itemId = 0;
            count = 0;
            inc = 0;
            return false;
        }

        private static int split_inc(ref int n, ref int m, int p)
        {
            if (n == 0)
                return 0;
            int num1 = m / n;//inc➗库存
            int num2 = m - num1 * n;//某余数
            n -= p;//减少库存
            int num3 = num2 - n;
            int num4 = num3 > 0 ? num1 * p + num3 : num1 * p;
            m -= num4;
            return num4;
        }













    }

}
