using HarmonyLib;

namespace Template
{
    [HarmonyPatch]
    class TemplatePatch
    {

        [HarmonyPrefix]
        [HarmonyPatch(typeof(TrashSystem), "ClearAllTrash")]
        public static void ClearAllTrashPatch(TrashSystem __instance)
        {



        }

    }
}
