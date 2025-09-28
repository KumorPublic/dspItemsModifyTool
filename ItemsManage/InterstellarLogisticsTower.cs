using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ItemsManage
{
    public class InterstellarLogisticsTower
    {
        // 判断Mod是否禁用
        private static bool isEnable = ItemManagePlugin.自动填充翘曲器.Value;
        // 获取传输间隔Tick数
        public static int WarperTickCount = 600;
        // 是否启用传输消耗
        public static bool WarperTransportCost = true;
        // 本地传输消耗
        public static int WarperLocalTransportCost = 0;
        // 远程传输消耗
        public static int WarperRemoteTransportCost = 2;
        // 曲速器物品ID
        private static int warperId = ItemProto.kWarperId;
        // 最大曲速器数量
        private static int maxWarperCount = 50;
        // 数量降低到多少时开始运输
        private static int minWarperCount = 10;
        private static readonly object _lock = new object();

        // Harmony补丁，后缀到PlanetTransport.GameTick方法
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlanetTransport), "GameTick", typeof(long), typeof(bool), typeof(bool))]
        public static void PlanetTransport_GameTick_Postfix(PlanetTransport __instance, long time, bool isActive, bool isMultithreadMode)
        {
            // 如果 Mod 禁用或 Tick 不匹配，直接返回
            if (!isEnable || ItemManagePlugin.frame % WarperTickCount != 0){return;}
               
            // 检查 __instance 是否为空
            if (__instance == null){ return;}

            lock (_lock)
            {
                try
                {
                    // ⚠️ 建议：只在调试时打印日志，否则频繁输出会拖慢性能
                    // ItemManagePlugin.logger.LogInfo($"按设定Tick间隔执行: {ItemManagePlugin.frame}");

                    // 收集所有星球和银河物流站
                    var stations = CollectStations(__instance);

                    // 分离供应站与需求站（预估容量，减少扩容开销）
                    var supplierStations = new List<StationComponent>(stations.Count);
                    var receiverStations = new List<StationComponent>(stations.Count);
                    IdentifyStations(stations, supplierStations, receiverStations);


                    // 从供应站向需求站分发曲速器
                    if (supplierStations == null || supplierStations.Count == 0)
                    {
                        // 可以选择记录日志 / 提示
                        //ItemManagePlugin.logger.LogInfo("没有可用的供应站，跳过分发流程");
                        return; // 或者 break，看你的函数设计
                    }

                    foreach (var receiver in receiverStations)
                    {
                        TransferWarpersToReceiver(__instance, receiver, supplierStations);
                    }
                }
                catch (Exception ex)
                {
                    ItemManagePlugin.logger.LogInfo("等待游戏存档载入...");
                }
            }

                
        }


        // 收集所有相关的物流站
        private static List<StationComponent> CollectStations(PlanetTransport planetTransport)
        {
            List<StationComponent> stations = new List<StationComponent>(planetTransport.stationCursor); // 初始化列表
            
            if(planetTransport.stationPool == null)
            {
                return stations;
            }

            // 遍历星球上的物流站
            StationComponent[] stationPool = planetTransport.stationPool;
            for (int j = 1; j < planetTransport.stationCursor; j++)
            {
                StationComponent station = stationPool[j];
                // 筛选有效的物流站
                if (station != null && station.id == j && !station.isCollector && station.isStellar)
                {
                    //ItemManagePlugin.logger.LogInfo($"添加星球站点：{station.id}");
                    stations.Add(station);
                }
            }


            // 遍历银河物流站
            GalacticTransport galacticTransport = UIRoot.instance.uiGame.gameData.galacticTransport;

            // 遍历银河物流站池
            foreach (StationComponent station in galacticTransport.stationPool)
            {
                // 筛选远程供应曲速器的星际站
                if (station != null && !station.isCollector && station.isStellar)
                {
                    //ItemManagePlugin.logger.LogInfo($"添加银河站点：{station.id}");
                    stations.Add(station);
                }
            }

            return stations; // 返回所有相关站点
        }

        // 识别供应站和需求站
        private static void IdentifyStations(List<StationComponent> stations, List<StationComponent> supplierStations, List<StationComponent> receiverStations)
        {
            foreach (var station in stations)
            {
                // 判断是否为供应站
                if (IsWarperSupplier(station))
                {
                    //ItemManagePlugin.logger.LogInfo($"站点：{station.id} 是供应站");
                    supplierStations.Add(station);
                }
                // 判断是否为需求站
                else if (NeedsWarpers(station))
                {
                    //ItemManagePlugin.logger.LogInfo($"站点：{station.id} 是接收站");
                    receiverStations.Add(station);
                }
            }
        }

        // 判断是否为曲速器供应站
        private static bool IsWarperSupplier(StationComponent station)
        {
            bool isSupplier = station.warperCount > 0 && station.storage.Any(s => s.itemId == warperId && s.remoteLogic == ELogisticStorage.Supply);
            return isSupplier;
        }


        // 判断站点是否需要曲速器
        private static bool NeedsWarpers(StationComponent station)
        {
            bool needsWarpers = (station.warperCount <= minWarperCount && station.warperNecessary);
            if (needsWarpers)
            {
                //ItemManagePlugin.logger.LogInfo($"站点：{station.id} 需要翘曲器 (当前: {station.warperCount}, 最大: {maxWarperCount}).");
            }
            return needsWarpers;
        }

        // 从供应站向需求站转移曲速器
        private static void TransferWarpersToReceiver(PlanetTransport planetTransport, StationComponent receiver, List<StationComponent> suppliers)
        {
            //int neededWarperCount = maxWarperCount - receiver.warperCount; // 计算需求量
            int neededWarperCount = receiver.warperMaxCount - receiver.warperCount; // 计算需求量; 直接获取可存储的最大翘曲器数量
            if (neededWarperCount <= 0)
            {
                // 接收站已足够
                return;
            }
            //ItemManagePlugin.logger.LogInfo($"接收站：{receiver.id} 需要翘曲器：{neededWarperCount}");


            // 遍历所有供应站
            foreach (var supplier in suppliers)
            {
                if (supplier.warperCount <= 0 || supplier?.storage == null)
                {
                    continue;
                }
                    
                int transportCost = CalculateTransportCost(supplier, receiver); // 计算运输消耗
                int supplierWarperCount = 0;
                int firstMatchIndex = -1;

                lock (supplier.storage)
                {
                    // 计算供应站可用曲速器数量；遍历找到第一个匹配存储槽
                    for (int i = 0; i < supplier.storage.Length; i++)
                    {
                        var s = supplier.storage[i];
                        if (s.itemId == warperId && s.remoteLogic == ELogisticStorage.Supply)
                        {
                            supplierWarperCount = s.count; // 只记录这个槽的数量
                            firstMatchIndex = i;           // 记录索引
                            break;                         // 找到第一个就退出
                        }
                    }

                    if (firstMatchIndex == -1)
                    {
                        continue; // 没有可用翘曲器
                    }
                        
                    int transferableWarperCount = supplierWarperCount - transportCost; // 可转移数量
                    if (transferableWarperCount <= 0)
                    {
                        continue; // 不够运输消耗
                    }

                    int transferAmount = Mathf.Min(neededWarperCount, transferableWarperCount); // 实际转移数量
                    if (transferAmount <= 0)
                    {
                        continue;
                    }
                    int itemCountToRemove = transferAmount + transportCost; // 需要从供应站移除的总数量
                    supplier.storage[firstMatchIndex].count -= itemCountToRemove; // 从供应站移除曲速器 直接从找到的槽扣减
                    lock (receiver)
                    {
                        receiver.warperCount += transferAmount; // 增加需求站曲速器数量
                    }
                    UpdateTraffic(planetTransport, receiver); // 更新物流流量
                    neededWarperCount -= transferAmount; // 更新剩余需求量
                    if (neededWarperCount <= 0)
                    {
                        break;
                    }
                }
   
            }
        }

        // 计算运输消耗
        private static int CalculateTransportCost(StationComponent supplier, StationComponent receiver)
        {
            return WarperTransportCost ? (supplier.planetId == receiver.planetId ? WarperLocalTransportCost : WarperRemoteTransportCost) : 0;
        }

        // 更新物流流量
        private static void UpdateTraffic(PlanetTransport planetTransport, StationComponent receiver)
        {
            receiver.UpdateNeeds(); // 更新需求
            planetTransport.RefreshStationTraffic(); // 刷新星球物流流量
            planetTransport.RefreshDispenserTraffic(); // 刷新分发器流量
            planetTransport.gameData.galacticTransport.RefreshTraffic(receiver.gid); // 刷新银河物流流量
        }
    }
}

