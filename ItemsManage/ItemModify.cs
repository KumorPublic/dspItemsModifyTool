using HarmonyLib;
using System.Reflection;
using System;
using ABN;

namespace ItemsManage
{
    ///<summary>
    /// 修改储液罐容量，修改一些建筑参数
    ///</summary>
    public class ItemModiy
    {
        private static bool alreadyModiy = false;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(VFPreload), "InvokeOnLoadWorkEnded")]
        public static void InvokeOnLoadWorkEnded()
        {
            
            if (alreadyModiy)
            {
                return;
            }
            alreadyModiy = true;


            // 修改储液罐容量
            if (ItemManagePlugin.储液罐.Value)
            {
                LDB.items.Select(2106).prefabDesc.fluidStorageCount = ItemConfig.ModifyCfg.储液罐.容量;
            }

            // 充电塔链接距离
            if (ItemManagePlugin.充电塔.Value)
            {
                LDB.items.Select(2202).prefabDesc.powerConnectDistance = ItemConfig.ModifyCfg.充电塔.链接距离;
            }

            //LDB.items.Select(3004).prefabDesc.turretSpaceAttackRange = 800000;

            //ItemManagePlugin.logger.LogInfo("BuildIndex   " + LDB.items.Select(1203).BuildIndex);
            //ItemManagePlugin.logger.LogInfo("GridIndex   " + LDB.items.Select(1203).GridIndex);
            //ItemManagePlugin.logger.LogInfo("ModelIndex   " + LDB.items.Select(1203).ModelIndex);

            // 修改氚核燃料棒配方
            //LDB.recipes.Select(41).Items = new int[2] { 1106, 1121 };
            //LDB.recipes.Select(41).ItemCounts = new int[2] { 1, 20 };


            // 机甲飞行速度
            //LDB.techs.Select(2903).UnlockValues[0] = 3000;
            //TechProto techProto = LDB.techs.Select(2903);
            // for (int j = 0; j < techProto.UnlockFunctions.Length; j++)
            //{
            //    ItemManagePlugin.logger.LogInfo("UnlockFunctions: " + techProto.UnlockFunctions[j] + "UnlockValues: " + techProto.UnlockValues[j]); 
            //}
        }
    }


    


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
