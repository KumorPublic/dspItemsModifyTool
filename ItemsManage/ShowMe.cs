using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ItemsManage
{
    public class ShowMe
    {
        //显示恒星系数据
        // 显示恒星系数据
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UIStarDetail), "OnStarDataSet")]
        public static void OnStarDataSetPatch(ref UIStarDetail __instance)
        {
            if (!ItemManagePlugin.显示恒星戴森半径.Value) return;
            var star = __instance.star;
            if (star == null) return;

            // 计算最小与最大轨道半径
            float minOrbitRadius = Mathf.Max(star.physicsRadius * 1.5f, 4000f);
            if (star.type == EStarType.GiantStar)
            {
                minOrbitRadius *= 0.6f;
            }
            minOrbitRadius = Mathf.Ceil(minOrbitRadius / 100f) * 100f;
            minOrbitRadius = minOrbitRadius / 40000;
            float maxOrbitRadius = star.dysonRadius * 2f;

            // 显示数据
            __instance.spectrValueText.text =
                $"{star.spectr}        戴森半径: {minOrbitRadius:0.00} - {maxOrbitRadius:0.00}AU";

            
        }

    }
}
