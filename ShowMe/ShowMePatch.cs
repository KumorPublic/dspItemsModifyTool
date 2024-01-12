using HarmonyLib;
using UnityEngine;


namespace ShowMe
{
    [HarmonyPatch]
    class ShowMePatch
    {
        /*
        //显示矿物采集速度
        [HarmonyPrefix]
        [HarmonyPatch(typeof(UIVeinDetailNode), "_OnUpdate")]
        public static bool _OnUpdatePatch(ref UIVeinDetailNode __instance, ref int ___counter, ref long ___showingAmount, ref VeinProto ___veinProto)
        {

            if (__instance.inspectFactory == null)
            {
                __instance._Close();
                return false;
            }
            VeinGroup veinGroup = __instance.inspectFactory.veinGroups[__instance.veinGroupIndex];
            string str;
            if (veinGroup.count == 0 || veinGroup.type == EVeinType.None)
            {
                __instance._Close();
                return false;
            }
            else
            {
                if (___counter % 4 == 0)// && ___showingAmount != veinGroup.amount
                {
                    ___showingAmount = veinGroup.amount;
                    if (veinGroup.type != EVeinType.Oil)
                    {
                        double num = (double)__instance.inspectFactory.gameData.history.miningSpeedScale * (double)ShowMePatch.CountTotalMinedVeinsInVeinGroup(__instance.veinGroupIndex, __instance.inspectFactory);
                        str = veinGroup.count.ToString() + "空格个".Translate() + ___veinProto.name + "储量".Translate() + veinGroup.amount.ToString("#,##0");
                        if (num > 0)
                        {
                            str += "\n " + "产量 " + (60.0 * num / 2).ToString("0") + "/m";
                        }
                        __instance.infoText.text = str;

                    }
                    else
                    {
                        str = veinGroup.count.ToString() + "空格个".Translate() + ___veinProto.name + "产量".Translate() + ((float)veinGroup.amount * VeinData.oilSpeedMultiplier).ToString("0.0000") + "/s";

                        __instance.infoText.text = str;
                        // ((60.0 * (float)veinGroup.amount * VeinData.oilSpeedMultiplier).ToString("0")) + "/m";
                    }

                }
                ++___counter;
            }
            return false;

        }

        private static int CountTotalMinedVeinsInVeinGroup(int veinGroupIndex, PlanetFactory inspectFactory)
        {
            int num = 0;
            foreach (VeinData veinData in inspectFactory.veinPool)
            {
                if ((int)veinData.groupIndex == veinGroupIndex)
                {
                    num += veinData.minerCount;
                }
            }
            return num;
        }
        */
        
        
        //显示恒星系数据
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UIStarDetail), "OnStarDataSet")]
        public static void OnStarDataSetPatch(ref UIStarDetail __instance)
        {
            if (__instance.star != null)
            {

                float minOrbitRadius;
                float maxOrbitRadius;


                minOrbitRadius = __instance.star.physicsRadius * 1.5f;
                if ((double)minOrbitRadius < 4000.0)
                {
                    minOrbitRadius = 4000f;
                }
                maxOrbitRadius = __instance.star.dysonRadius * 2f;
                if (__instance.star.type == EStarType.GiantStar)
                {
                    minOrbitRadius *= 0.6f;
                }
                minOrbitRadius = Mathf.Ceil(minOrbitRadius / 100f) * 100f;


                __instance.spectrValueText.text = __instance.star.spectr.ToString() + "        " + "戴森半径(Max): " + maxOrbitRadius.ToString("0.00") + "AU";
                //__instance.temperatureValueText.text = __instance.star.temperature.ToString("#,##0") + " K" + "戴森半径(Min)：" + minOrbitRadius.ToString("0.00") + "m";

            }



        }

        //夜灯
        /**
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PostEffectController), "Update")]
        public static void PostEffectControllerUpdatePatch(ref PostEffectController __instance)
        {
            
            if(GameCamera.sceneIndex == 0 || GameCamera.sceneIndex == 2 || !GameMain.isRunning)
            {
                return;
            }
           // if (PostEffectController.headlight && GameMain.localPlanet != null && GameMain.mainPlayer != null && GameMain.mainPlayer.controller != null && GameMain.mainPlayer.movementState < EMovementState.Sail && !FactoryModel.whiteMode0)

                if (PostEffectController.headlight && GameMain.localPlanet != null && GameMain.mainPlayer != null && !FactoryModel.whiteMode0)
            { 
                //foreach (PowerNodeComponent node in GameMain.localPlanet.factory.powerSystem.nodePool)
               
                    Shader.SetGlobalVector("_Global_PointLightPos", new Vector4(GameMain.mainPlayer.position.x+ ShowMePlugin.LightRangeX, GameMain.mainPlayer.position.y+ ShowMePlugin.LightRangeY, GameMain.mainPlayer.position.z+ ShowMePlugin.LightRangeZ, ShowMePlugin.LightRangeW));
                    ShowMePlugin.logger.LogInfo("LightRangeX: " + GameMain.mainPlayer.position.x.ToString()+","+ GameMain.mainPlayer.position.y.ToString() + ","+ GameMain.mainPlayer.position.z.ToString() );
            }
           
                


            


        }
        

        /**
        public static float GetLightRange()
        {
            return ShowMePlugin.LightRange;
        }
        
        
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(PostEffectController), "Update")]
        public static IEnumerable<CodeInstruction> PostEffectControllerUpdatePatch(
        IEnumerable<CodeInstruction> instructions)
        {
            var methodInfo = AccessTools.Method(typeof(ShowMePatch), nameof(GetLightRange)); 
            List<CodeInstruction> list = instructions.ToList<CodeInstruction>();
            list[432].opcode = OpCodes.Ldc_R4;
            list[432].operand = 8f;
            //list[432] = new CodeInstruction(OpCodes.Call, methodInfo);
            

            return ((IEnumerable<CodeInstruction>)list).AsEnumerable<CodeInstruction>();
        }
        **/

    }
}
