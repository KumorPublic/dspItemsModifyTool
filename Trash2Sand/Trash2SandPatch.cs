using HarmonyLib;

namespace Trash2Sand
{
    [HarmonyPatch]
    class Trash2SandPatch
    {
        /**
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MinerComponent), "InternalUpdate")]
        public static void InternalUpdatePatch(
             MinerComponent __instance,
             uint __result,
             PlanetFactory factory,
             VeinData[] veinPool,
             float power,
             float miningRate,
             float miningSpeed,
             int[] productRegister)
        {
            if (__result < 1 || __instance.productId != 1005)
            {
                return;
            }
           
            //ItemsModifyToolPlugin.logger.LogInfo(GameMain.data.mainPlayer.sandCount);
            //GameMain.mainPlayer.sandCount + 
            //if((int)factory.entityPool[__instance.entityId].protoId != 2301)
            // {
            //     return;
            //  }
            if (ItemsModifyToolPatch.mainPlayer == null)
            {
                ItemsModifyToolPlugin.logger.LogInfo("not in game!");
                return;
            }
                

                int result = 1000;

            ItemsModifyToolPatch.mainPlayer.SetSandCount(GameMain.mainPlayer.sandCount + result);

        }
        **/

        /*
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlanetFactory), "BuildFinally")]
        public static void BuildFinallyPatch(PlanetFactory __instance, Player player, int prebuildId)
        {
            player.SetSandCount(player.sandCount + 100);
        }
        */


        //清理垃圾时获得沙土
        [HarmonyPrefix]
        [HarmonyPatch(typeof(TrashSystem), "ClearAllTrash")]
        public static void ClearAllTrashPatch(TrashSystem __instance)
        {
            int sandCount = 0;
            for (int index = 0; index < __instance.container.trashCursor; ++index)
            {
                if (__instance.container.trashObjPool[index].item > 0)
                {
                    int itemId = __instance.container.trashObjPool[index].item;
                    int count = __instance.container.trashObjPool[index].count;
                    switch (itemId)
                    {
                        case 1005://石头
                            sandCount += Trash2SandPlugin.Trash2SandMult.Value * 50 * count;
                            break;
                        case 1003://硅石
                            sandCount += Trash2SandPlugin.Trash2SandMult.Value * 100 * count;
                            break;
                        case 1012://金
                            sandCount += Trash2SandPlugin.Trash2SandMult.Value * 800 * count;
                            break;
                        case 1013://分型硅石
                            sandCount += Trash2SandPlugin.Trash2SandMult.Value * 800 * count;
                            break;
                        default:
                            if (Trash2SandPlugin.FreeloaderMode.Value)
                            {
                                sandCount += Trash2SandPlugin.Trash2SandMult.Value * count;
                            }
                            break;
                    }

                }
            }
            if (sandCount > 0)
            {
                //Trash2SandPlugin.logger.LogInfo("垃圾个数: " + __instance.trashCount.ToString());
                Trash2SandPlugin.logger.LogInfo("获得沙土: " + sandCount.ToString());
                __instance.player.SetSandCount(__instance.player.sandCount + sandCount);
            }

        }

    }
}
