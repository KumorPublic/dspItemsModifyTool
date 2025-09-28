using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemsManage
{
    public class Trash2Sand
    {
        //清理垃圾时获得沙土
        [HarmonyPrefix]
        [HarmonyPatch(typeof(TrashSystem), "ClearAllTrash")]
        public static void ClearAllTrashPatch(TrashSystem __instance)
        {
            // 避免多次访问链式属性
            var pool = __instance.container.trashObjPool;
            int cursor = __instance.container.trashCursor;

            // 用字典存储权重：避免长 switch；定义垃圾转化沙土的倍率表
            var sandMultiplier = new Dictionary<int, int>
                {
                    { 1005, 50 },   // 石头
                    { 1003, 100 },  // 硅石
                    { 1012, 300 },  // 金
                    { 1013, 800 }   // 分型硅石
                };

            int sandCount = 0;

            for (int i = 0; i < cursor; i++)
            {
                ref var obj = ref pool[i];  // 用 ref 减少复制开销
                if (obj.item > 0 && sandMultiplier.TryGetValue(obj.item, out int multiplier))
                {
                    sandCount += multiplier * obj.count;
                }
            }

            if (sandCount > 0)
            {
                ItemManagePlugin.logger.LogInfo(
                    $"清理垃圾: {__instance.trashCount}   获得沙土: {sandCount}"
                );
                __instance.player.SetSandCount(__instance.player.sandCount + sandCount);
            }
        }


    }
}
