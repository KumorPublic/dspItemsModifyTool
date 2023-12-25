using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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


        /// <summary>
        /// 扫描星球上的碎片物品id
        /// </summary>
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


        /// <summary>
        /// 物流站点和物流飞船的行为
        /// 1. 根据游戏的机制，一个恒星周围最多允许存在99个行星（编号为恒星ID+1到恒星ID+99），但是游戏原本只考虑了恒星自身以及恒星周围的9个行星，导致存在更多行星时运输船无法正常停靠，
        ///     这段代码参考了 GalacticScale 的代码，并在其基础上加以完善，允许运输船在更多行星上停靠
        /// 2. 参考 GalacticScale 的代码，游戏中运输船在寻路时会绕开距离恒星 2.5 倍半径的范围，导致靠近大恒星的行星无法正常停靠
        /// </summary>
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(StationComponent), "InternalTickRemote")]
        public static IEnumerable<CodeInstruction> InternalTickRemoteTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            // 首先修正搜索行星的范围
            // 确定开始搜索的位置，应该在 if (shipData.stage == 0) 的位置
            CodeMatcher matcher = new CodeMatcher(instructions);
            matcher.MatchForward(false,
                new CodeMatch(instruction => instruction.opcode == OpCodes.Br),
                new CodeMatch(instruction => instruction.opcode == OpCodes.Ldloc_S && instruction.operand.ToString().StartsWith("ShipData")),
                new CodeMatch(instruction => instruction.opcode == OpCodes.Ldfld && instruction.operand.ToString().Equals("System.Int32 stage")),
                new CodeMatch(instruction => instruction.opcode == OpCodes.Brtrue)
            );

            if (matcher.IsInvalid)
            {
                Util.Log(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, "无法找到代码 if (shipData.stage == 0) 的位置");
                return instructions;
            }

            // 找到两处形如 A < B + 10 的代码，然后将 +10 修改为 +100
            matcher.MatchForward(false,
                new CodeMatch(instruction => instruction.opcode == OpCodes.Ldloc_S && instruction.operand.ToString().StartsWith("System.Int32")),
                new CodeMatch(instruction => instruction.opcode == OpCodes.Ldloc_S && instruction.operand.ToString().StartsWith("System.Int32")),
                new CodeMatch(instruction => instruction.opcode == OpCodes.Ldc_I4_S && (sbyte)instruction.operand == 10),
                new CodeMatch(instruction => instruction.opcode == OpCodes.Add),
                new CodeMatch(instruction => instruction.opcode == OpCodes.Blt)
            );

            if (matcher.IsInvalid)
            {
                Util.Log(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, "无法找到第一处形如 A < B + 10 的代码");
                return instructions;
            }

            matcher.Advance(2);
            matcher.SetOperandAndAdvance(100);

            matcher.MatchForward(false,
                new CodeMatch(instruction => instruction.opcode == OpCodes.Ldloc_S && instruction.operand.ToString().StartsWith("System.Int32")),
                new CodeMatch(instruction => instruction.opcode == OpCodes.Ldloc_S && instruction.operand.ToString().StartsWith("System.Int32")),
                new CodeMatch(instruction => instruction.opcode == OpCodes.Ldc_I4_S && (sbyte)instruction.operand == 10),
                new CodeMatch(instruction => instruction.opcode == OpCodes.Add),
                new CodeMatch(instruction => instruction.opcode == OpCodes.Blt)
            );

            if (matcher.IsInvalid)
            {
                Util.Log(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, "无法找到第二处形如 A < B + 10 的代码");
                return instructions;
            }

            matcher.Advance(2);
            matcher.SetOperandAndAdvance(100);

            // 然后开始修正规避恒星半径的范围
            // 这段代码应该在一段形如 if (A % 100 == 0) B *= 2.5f 的代码块中
            matcher.MatchForward(true,
                new CodeMatch(instruction => instruction.opcode == OpCodes.Ldloc_S && instruction.operand.ToString().StartsWith("System.Int32")),
                new CodeMatch(instruction => instruction.opcode == OpCodes.Ldc_I4_S && (sbyte)instruction.operand == 100),
                new CodeMatch(instruction => instruction.opcode == OpCodes.Rem),
                new CodeMatch(instruction => instruction.opcode == OpCodes.Brtrue),
                new CodeMatch(instruction => instruction.opcode == OpCodes.Ldloc_S && instruction.operand.ToString().StartsWith("System.Single")),
                new CodeMatch(instruction => instruction.opcode == OpCodes.Ldc_R4 && (float)instruction.operand == 2.5f),
                new CodeMatch(instruction => instruction.opcode == OpCodes.Mul),
                new CodeMatch(instruction => instruction.opcode == OpCodes.Stloc_S && instruction.operand.ToString().StartsWith("System.Single"))
            );

            if (matcher.IsInvalid)
            {
                Util.Log(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, "无法找到形如 if (A % 100 == 0) B *= 2.5f 的代码");
                return instructions;
            }

            matcher.Advance(-2);
            matcher.SetOperandAndAdvance(1f);
            Util.Log(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, "物流系统修正完成");
            return matcher.InstructionEnumeration();
        }

        /// <summary>
        /// 垃圾系统
        /// 修正垃圾受重力影响的逻辑
        /// </summary>
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(TrashSystem), "Gravity")]
        static IEnumerable<CodeInstruction> TrashSystem_Gravity_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            CodeMatcher matcher = new CodeMatcher(instructions);

            // 某种情况下高度大于600的垃圾会直接消失，这里把这个限制改成800
            /*
            matcher.MatchForward(
                true,
                new CodeMatch(OpCodes.Ldc_R8, 600.0)
            );

            if (matcher.IsInvalid)
            {
                Util.Log(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, "无法找到数字 600.0");
                return instructions;
            }

            matcher.Set(OpCodes.Ldc_R8, 800.0);
            */

            // 寻找一段形如 A <= B + 8 的代码，然后将 +8 改为 +99
            matcher.MatchForward(false,
                new CodeMatch(instruction => instruction.opcode == OpCodes.Ldloc_S && ((LocalBuilder)instruction.operand).LocalIndex == 6),
                new CodeMatch(OpCodes.Ldloc_1),
                new CodeMatch(OpCodes.Ldc_I4_8),
                new CodeMatch(OpCodes.Add),
                new CodeMatch(OpCodes.Ble)
            );

            if (matcher.IsInvalid)
            {
                Util.Log(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, "无法找到第形如 A <= B + 8 的代码");
                return instructions;
            }

            matcher.Advance(2);
            matcher.Set(OpCodes.Ldc_I4_S, 99);

            Util.Log(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, "重力系统修正完成");

            return matcher.InstructionEnumeration();
        }

        /// <summary>
        /// 修改初始星球位置
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UniverseGen), nameof(UniverseGen.CreateGalaxy))]
        static void UniverseGen_CreateGalaxy_Postfix(GalaxyData __result)
        {
            if (CustomCreateBirthStarPlugin.BirthStarId != -1 && CustomCreateBirthStarPlugin.BirthPlanetId != -1)
            {
                __result.birthStarId = CustomCreateBirthStarPlugin.BirthStarId;
                __result.birthPlanetId = CustomCreateBirthStarPlugin.BirthPlanetId;
            }
        }


    }




}

