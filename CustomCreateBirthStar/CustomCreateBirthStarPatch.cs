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
    [HarmonyPatch]
    public static class CustomCreateBirthStarPatch
    {
        //重写恒星创建逻辑
        [HarmonyPrefix]
        [HarmonyPatch(typeof(StarGen), "CreateStar")]
        public static void CreateStarPatch(ref GalaxyData galaxy, ref VectorLF3 pos, int id, int seed, ref EStarType needtype, ref ESpectrType needSpectr)
        {
            Stellar4Stars.Prepare(ref galaxy, ref pos, ref id, ref seed, ref needtype, ref needSpectr);
            SuperMassBlackHole.Prepare(ref galaxy, ref pos, ref id, ref seed, ref needtype, ref needSpectr);
        }
        
        //重写恒星创建逻辑
        [HarmonyPostfix]
        [HarmonyPatch(typeof(StarGen), "CreateStar")]
        public static void CreateStarPatch(ref StarData __result,int id, int seed)
        {
            Stellar4Stars.Create(ref __result, id, seed);
            SuperMassBlackHole.Create(ref __result, id, seed);
        }

        //重写行星创建逻辑
        [HarmonyPrefix]
        [HarmonyPatch(typeof(StarGen), "CreateStarPlanets")]
        public static bool CreateStarPlanetsPatch(ref double[] ___pGas, GalaxyData galaxy, ref StarData star, GameDesc gameDesc)
        {
            
            if (!Stellar4Stars.CreatePlanet(ref ___pGas, ref galaxy, ref star, ref gameDesc)) { return false; }
            
            if (!SuperMassBlackHole.CreatePlanet(ref ___pGas, ref galaxy, ref star, ref gameDesc)) { return false; }

            return true;
        }

        //修改行星特质
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlanetGen), "CreatePlanet")]
        public static void CreatePlanetPatch(GalaxyData galaxy, StarData star, int[] themeIds, int index, int orbitAround, int orbitIndex, int number, bool gasGiant, int info_seed, int gen_seed, ref PlanetData __result)
        {
            Stellar4Stars.AddPlantTraits(galaxy, star, themeIds, index, orbitAround, orbitIndex, number, gasGiant, info_seed, gen_seed, ref __result);
            SuperMassBlackHole.AddPlantTraits(galaxy, star, themeIds, index, orbitAround, orbitIndex, number, gasGiant, info_seed, gen_seed, ref __result);
        }

        //修改风力涡轮机转速
        //[HarmonyTranspiler]
        //[HarmonyPatch(typeof(PowerSystem), "GameTick")]
        //public static IEnumerable<CodeInstruction> PowerSystemGameTickFix(
        //IEnumerable<CodeInstruction> instructions)
        //{
        //    List<CodeInstruction> list = instructions.ToList<CodeInstruction>();
        //    list[1390].opcode = OpCodes.Ldc_R4;
        //    if (10 < 1.01f)
        //    {
        //        list[1390].operand = 0.7f;
        //    }
        //    else
        //    {
        //        list[1390].operand = 0.64f / 10;
        //    }

        //    return ((IEnumerable<CodeInstruction>)list).AsEnumerable<CodeInstruction>();
        //}

        /**修改行星主题
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlanetGen), "SetPlanetTheme")]
        public static void SetPlanetThemePatch(ref PlanetData planet, int[] themeIds, double rand1, double rand2, double rand3, double rand4, int theme_seed)
        {
            switch (planet.star.index)
            {
                case STAR_ID_GIANT:
                    //__result.name = "先知的隐居地";
                    //CustomCreateBirthStarPlugin.logger.LogInfo(planet.star.index);
                    //int length1 = themeIds.Length;
                    //for (int index1 = 0; index1 < length1; ++index1)
                    //{
                    //   ThemeProto themeProto = LDB.themes.Select(themeIds[index1]);
                    //   CustomCreateBirthStarPlugin.logger.LogInfo("ID: " + themeProto.ID);
                    //   CustomCreateBirthStarPlugin.logger.LogInfo("DisplayName: " + themeProto.DisplayName);
                    //   CustomCreateBirthStarPlugin.logger.LogInfo("PlanetType: " + themeProto.PlanetType.ToString());
                    //   CustomCreateBirthStarPlugin.logger.LogInfo("Algos: " + string.Join(" ", themeProto.Algos));
                    //   CustomCreateBirthStarPlugin.logger.LogInfo("Temperature: " + themeProto.Temperature.ToString());
                    //   CustomCreateBirthStarPlugin.logger.LogInfo("Distribute: " + themeProto.Distribute.ToString());
                    //   CustomCreateBirthStarPlugin.logger.LogInfo("Vegetables0: " + string.Join(" ", themeProto.Vegetables0));
                    //   CustomCreateBirthStarPlugin.logger.LogInfo("VeinSpot: " + string.Join(" ", themeProto.VeinSpot));
                    //   CustomCreateBirthStarPlugin.logger.LogInfo("VeinCount: " + string.Join(" ", themeProto.VeinCount));
                    //   CustomCreateBirthStarPlugin.logger.LogInfo("VeinOpacity: " + string.Join(" ", themeProto.VeinOpacity));
                    //   CustomCreateBirthStarPlugin.logger.LogInfo("RareVeins: " + string.Join(" ", themeProto.RareVeins));
                    //   CustomCreateBirthStarPlugin.logger.LogInfo("RareSettings: " + string.Join(" ", themeProto.RareSettings));
                    //   CustomCreateBirthStarPlugin.logger.LogInfo("Wind: " + themeProto.Wind.ToString());
                    //   CustomCreateBirthStarPlugin.logger.LogInfo("IonHeight: " + themeProto.IonHeight.ToString());
                    //   CustomCreateBirthStarPlugin.logger.LogInfo("CullingRadius: " + themeProto.CullingRadius.ToString());
                    //   CustomCreateBirthStarPlugin.logger.LogInfo("===================================");

                    // }
                    
                    break;
                case STAR_BLACKHOLE:
                    // __result.name = "卡冈都亚";
                    break;
                case STAR_NEUTRONSTAR:
                    //__result.name = "围墙花园";
                    break;
                case STAR_MAINSEQSTAR_O:
                    //__result.name = "至纯瑰宝";
                    if (CustomCreateBirthStarPlugin.windStrength > 1)
                    {
                        if (planet.theme == 19)//飓风石林
                        {
                            planet.windStrength = 1.51f * CustomCreateBirthStarPlugin.windStrength;

                        }
                        else if (planet.theme == 6)//沙漠
                        {
                            planet.windStrength = 2.16f * CustomCreateBirthStarPlugin.windStrength;
                        }

                    }
                    if (CustomCreateBirthStarPlugin.luminosity > 1)
                    {
                        if (planet.theme == 11)//贫瘠荒漠
                        {
                            planet.luminosity *= CustomCreateBirthStarPlugin.luminosity;
                        }
                    }

                    break;
                default:
                    break;
            }
        }
        **/


        //开采石头时拦截
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlanetFactory), "RemoveVegeWithComponents")]
        public static void RemoveVegeWithComponents(ref PlanetFactory __instance, int id)
        {
            SuperMassBlackHole.OnMine(ref __instance, id);
        }

        //[HarmonyPrefix]
        //[HarmonyPatch(typeof(GameScenarioLogic), "NotifyOnVegetableMined")]
        //public static void GameScenarioLogic_NotifyOnVegetableMined_Prefix(ref GameScenarioLogic __instance, int protoId)
        //{
        //    VegeProto vegeProto = LDB.veges.Select(protoId);
        //    Util.Log((object)(string.Format("Mined proto ID {0} (", (object)protoId) + vegeProto.Name.Translate() + ")"));
        //    Util.Log("恒星ID：" + __instance.gameData.localStar.id);
        //    Util.Log("恒星ID：" + __instance.gameData.localPlaneNewModel.ID);
        //   // __instance.gameData.player.TryAddItemToPackage(num4, num5, 0, true);

        //}




    }




}

