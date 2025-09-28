using ABN;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemsManage
{
    ///<summary>
    /// 屏蔽检测
    ///</summary>
    public class NoAbnormalLogic
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(AbnormalityLogic), "NotifyBeforeGameSave")]
        [HarmonyPatch(typeof(AbnormalityLogic), "NotifyOnAssemblerRecipePick")]
        [HarmonyPatch(typeof(AbnormalityLogic), "NotifyOnGameBegin")]
        [HarmonyPatch(typeof(AbnormalityLogic), "NotifyOnMechaForgeTaskComplete")]
        [HarmonyPatch(typeof(AbnormalityLogic), "NotifyOnUnlockTech")]
        [HarmonyPatch(typeof(AbnormalityLogic), "NotifyOnUseConsole")]
        private static bool DisableAbnormalLogic()
        {
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AbnormalityLogic), "GameTick")]
        public static bool GameTick()
        {
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(GameAbnormalityData_0925), "TriggerAbnormality")]
        public static bool TriggerAbnormality()
        {
            return false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameAbnormalityData_0925), "Import")]
        public static void Import(ref GameAbnormalityData_0925 __instance)
        {
            __instance.runtimeDatas = new AbnormalityRuntimeData[3000];
        }


    }
}
