using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace CustomCreateBirthStar
{
    //修改星系上限
    [HarmonyPatch]
    public static class GalaxyStarCountPatch
    {
        private const int starCountmax = 250;
        private const int starCountmin = 32;
        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(UIGalaxySelect), "_OnInit")]
        public static void _OnInitPatch(UIGalaxySelect __instance, ref Slider ___starCountSlider, ref InputField ___seedInput)
        {
            ___starCountSlider.maxValue = (float)starCountmax;
            ___starCountSlider.minValue = (float)starCountmin;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(UIGalaxySelect), "OnStarCountSliderValueChange")]
        public static bool OnStarCountSliderValueChangePatch(UIGalaxySelect __instance, ref Slider ___starCountSlider, ref GameDesc ___gameDesc, float val)
        {
            int num = (int)((double)__instance.starCountSlider.value + 0.100000001490116);
            if (num < starCountmin)
                num = starCountmin;
            else if (num > starCountmax)
                num = starCountmax;
            if (num == ___gameDesc.starCount)
                return false;
            ___gameDesc.starCount = num;
            __instance.SetStarmapGalaxy();
            return false;
        }


        [HarmonyPrefix]
        [HarmonyPatch(typeof(UIGalaxySelect), "UpdateUIDisplay")]
        public static void UIPrefix(UIGalaxySelect __instance, ref Slider ___starCountSlider) => ___starCountSlider.onValueChanged.RemoveListener(new UnityAction<float>(__instance.OnStarCountSliderValueChange));

        [HarmonyPostfix]
        [HarmonyPatch(typeof(UIGalaxySelect), "UpdateUIDisplay")]
        public static void UIPostfix(UIGalaxySelect __instance, ref Slider ___starCountSlider) => ___starCountSlider.onValueChanged.AddListener(new UnityAction<float>(__instance.OnStarCountSliderValueChange));




    }
}
