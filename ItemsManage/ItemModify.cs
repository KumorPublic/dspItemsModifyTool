using HarmonyLib;

namespace ItemsManage
{
    ///<summary>
    ///修改储液罐容量
    ///</summary>
    class ItemTankStorage
    {
        private static bool alreadyInitializedTankStorage;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(VFPreload), "InvokeOnLoadWorkEnded")]
        public static void InvokeOnLoadWorkEnded()
        {
            if (alreadyInitializedTankStorage)
            {
                return;
            }
            alreadyInitializedTankStorage = true;

            LDB.items.Select(2106).prefabDesc.fluidStorageCount = ItemConfig.ModifyCfg.TankStorage.fluidStorageCount;

            //ItemManagePlugin.logger.LogInfo("BuildIndex   " + LDB.items.Select(1203).BuildIndex);
            //ItemManagePlugin.logger.LogInfo("GridIndex   " + LDB.items.Select(1203).GridIndex);
            //ItemManagePlugin.logger.LogInfo("ModelIndex   " + LDB.items.Select(1203).ModelIndex);

            // 修改氚核燃料棒配方
            LDB.recipes.Select(41).Items = new int[2] { 1107, 1121};
            LDB.recipes.Select(41).ItemCounts = new int[2] { 1, 20 };

        }
    }

    ///<summary>
    ///修改机甲翘曲器堆叠
    ///</summary>
    /**
    class ItemWarpStorage
    {
        public static void Init()
        {
            GameMain.mainPlayer.mecha.warpStorage.SetFilter(0, ItemsProtoID.空间翘曲器, ItemConfig.ModifyCfg.WarpStorage.StackSize);
            GameMain.mainPlayer.mecha.warpStorage.Sort();
        }


        [HarmonyPostfix]
        [HarmonyPatch(typeof(Mecha), "Init")]
        public static void MechaInit(Mecha __instance)
        {
            __instance.warpStorage.SetFilter(0, ItemsProtoID.空间翘曲器, ItemConfig.ModifyCfg.WarpStorage.StackSize);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Mecha), "SetForNewGame")]
        public static void MechaSetForNewGame(Mecha __instance)
        {
            __instance.warpStorage.SetFilter(0, ItemsProtoID.空间翘曲器, ItemConfig.ModifyCfg.WarpStorage.StackSize);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StorageComponent), "SetFilter")]
        public static void StorageComponentSetFilter(int gridIndex, int itemId, ref int stackSize)
        {
            //ItemManagePlugin.logger.LogInfo("ModelIndex111111111111" + itemId.ToString());
            if (itemId == ItemsProtoID.空间翘曲器)
            {
                stackSize = ItemConfig.ModifyCfg.WarpStorage.StackSize;
            }
        }

    }
    **/




    public class ItemManageTest
    {
        /**
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PowerSystem), "line_arragement_for_add_node")]
        private static bool PowerSystem_line_arragement_for_add_node(PowerNetworkStructures.Node node)
        {
            return false;
        }
        **/

        /**
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PowerSystem), "RemoveNodeComponent")]
        private static bool PowerSystem_RemoveNodeComponent(int id)
        {
            ItemManagePlugin.logger.LogInfo(id);


            return true;
        }
         **/

    }




}
