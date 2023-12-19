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
    ///<summary>
    ///在母星系附近创建四个富有星系
    ///</summary>
    public static class Stellar4Stars
    {

        ///<summary>
        ///蓝巨星
        ///</summary>
        private const int STAR_ID_GIANT = 2;
        ///<summary>
        ///黑洞
        ///</summary>
        private const int STAR_BLACKHOLE = 3;
        ///<summary>
        ///中子星
        ///</summary>
        private const int STAR_NEUTRONSTAR = 4;
        ///<summary>
        ///O型主序星
        ///</summary>
        private const int STAR_MAINSEQSTAR_O = 5;


        ///<summary>
        ///准备圣地参数
        ///</summary>
        public static void Prepare(ref GalaxyData galaxy, ref VectorLF3 pos, ref int id, ref int seed, ref EStarType needtype, ref ESpectrType needSpectr)
        {
            if (!CustomCreateBirthStarPlugin.StarTypePatch) { return; }
            switch (id)
            {
                case STAR_ID_GIANT:
                    needtype = EStarType.GiantStar;
                    needSpectr = ESpectrType.O;
                    break;
                case STAR_BLACKHOLE:
                    needtype = EStarType.BlackHole;
                    needSpectr = ESpectrType.X;
                    break;
                case STAR_NEUTRONSTAR:
                    needtype = EStarType.NeutronStar;
                    needSpectr = ESpectrType.X;
                    break;
                case STAR_MAINSEQSTAR_O:
                    needtype = EStarType.MainSeqStar;
                    needSpectr = ESpectrType.O;
                    break;
                default:
                    break;
            }
        }

        ///<summary>
        ///创建圣地
        ///</summary>
        public static void Create(ref StarData __result, int id, int seed)
        {
            if (!CustomCreateBirthStarPlugin.StarTypePatch) { return; }
            switch (__result.id)
            {
                case STAR_ID_GIANT:
                    __result.name = "先知的隐居地";
                    __result.habitableRadius = __result.habitableRadius / 2.5f;
                    break;
                case STAR_BLACKHOLE:
                    __result.name = "光之终结";
                    break;
                case STAR_NEUTRONSTAR:
                    __result.name = "围墙花园";
                    break;
                case STAR_MAINSEQSTAR_O:
                    __result.name = "至纯瑰宝";
                    break;
                default:
                    //Util.outputStarData(__result);
                    break;
            }
        }
        
        ///<summary>
        ///创建行星
        ///</summary>
        public static bool CreatePlanet(ref double[] ___pGas, ref GalaxyData galaxy, ref StarData star, ref GameDesc gameDesc)
        {
            if (!CustomCreateBirthStarPlugin.StarTypePatch) { return true; }
            
            switch (star.id)
            {
                case STAR_ID_GIANT: // 蓝巨星
                    CreateStarPlanets(ref ___pGas, ref galaxy, ref star, ref gameDesc);
                    return false;
                case STAR_BLACKHOLE://黑洞
                    break;
                case STAR_NEUTRONSTAR://中子星
                    break;
                case STAR_MAINSEQSTAR_O://O星
                    CreateStarPlanets(ref ___pGas, ref galaxy, ref star, ref gameDesc);
                    return false;
                default:
                    break;
            }
            return true;
        }

        public static void CreateStarPlanets(ref double[] ___pGas, ref GalaxyData galaxy, ref StarData star, ref GameDesc gameDesc)
        {


            DotNet35Random dotNet35Random1 = new DotNet35Random(star.seed);
            dotNet35Random1.Next();
            dotNet35Random1.Next();
            dotNet35Random1.Next();
            DotNet35Random dotNet35Random2 = new DotNet35Random(dotNet35Random1.Next());
            double num1 = dotNet35Random2.NextDouble();
            double num2 = dotNet35Random2.NextDouble();
            double num3 = dotNet35Random2.NextDouble();
            double num4 = dotNet35Random2.NextDouble();
            double num5 = dotNet35Random2.NextDouble();
            double num6 = dotNet35Random2.NextDouble() * 0.2 + 0.9;
            double num7 = dotNet35Random2.NextDouble() * 0.2 + 0.9;
            DotNet35Random dotNet35Random3 = new DotNet35Random(dotNet35Random1.Next());
            AccessTools.Method(typeof(StarGen), "SetHiveOrbitsConditionsTrue").Invoke(null, new object[] { /* 传递方法所需的参数 */ });

            if (star.type == EStarType.BlackHole)
            {
                
            }
            else if (star.type == EStarType.NeutronStar)
            {
              
            }
            else if (star.type == EStarType.WhiteDwarf)
            {
                
            }
            else if (star.type == EStarType.GiantStar)
            {
                if (star.id == STAR_ID_GIANT)
                {
                    star.planetCount = 8;
                    star.planets = new PlanetData[star.planetCount];

                    int info_seed5 = dotNet35Random2.Next();
                    int gen_seed5 = dotNet35Random2.Next();
                    int info_seed6 = dotNet35Random2.Next();
                    int gen_seed6 = dotNet35Random2.Next();
                    int info_seed7 = dotNet35Random2.Next();
                    int gen_seed7 = dotNet35Random2.Next();
                    info_seed6 = dotNet35Random2.Next();
                    gen_seed6 = dotNet35Random2.Next();
                    star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 0, 0, 1, 1, true, info_seed6, gen_seed6);
                    if (info_seed6 > gen_seed6)
                    {
                        star.planets[1] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 1, 1, 1, 0, false, info_seed6, gen_seed6);
                        info_seed6 = dotNet35Random2.Next();
                        gen_seed6 = dotNet35Random2.Next();
                        star.planets[2] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 2, 1, 2, 0, false, info_seed6, gen_seed6);
                        info_seed6 = dotNet35Random2.Next();
                        gen_seed6 = dotNet35Random2.Next();
                        star.planets[3] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 3, 1, 5, 0, false, info_seed6, gen_seed6);
                    }
                    else
                    {
                        star.planets[1] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 1, 1, 1, 0, false, info_seed6, gen_seed6);
                        info_seed6 = dotNet35Random2.Next();
                        gen_seed6 = dotNet35Random2.Next();
                        star.planets[2] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 2, 0, 2, 0, false, info_seed6, gen_seed6);
                        info_seed6 = dotNet35Random2.Next();
                        gen_seed6 = dotNet35Random2.Next();
                        star.planets[3] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 3, 0, 3, 0, false, info_seed6, gen_seed6);
                    }

                    info_seed6 = dotNet35Random2.Next();
                    gen_seed6 = dotNet35Random2.Next();
                    star.planets[4] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 4, 0, 4, 0, false, info_seed6, gen_seed6);
                    info_seed7 = dotNet35Random2.Next();
                    gen_seed7 = dotNet35Random2.Next();
                    star.planets[5] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 5, 0, 5, 0, false, info_seed7, gen_seed7);
                    info_seed7 = dotNet35Random2.Next();
                    gen_seed7 = dotNet35Random2.Next();
                    star.planets[6] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 6, 0, 6, 7, true, info_seed7, gen_seed7);
                    info_seed7 = dotNet35Random2.Next();
                    gen_seed7 = dotNet35Random2.Next();
                    star.planets[7] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 7, 7, 1, 0, false, info_seed7, gen_seed7);
                    info_seed7 = dotNet35Random2.Next();
                    gen_seed7 = dotNet35Random2.Next();

                }else
                {
                    {
                        if (num1 < 0.30000001192092896)
                        {
                            star.planetCount = 1;
                            star.planets = new PlanetData[star.planetCount];
                            int info_seed = dotNet35Random2.Next();
                            int gen_seed = dotNet35Random2.Next();
                            int orbitIndex = num3 > 0.5 ? 3 : 2;
                            star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 0, 0, orbitIndex, 1, false, info_seed, gen_seed);
                        }
                        else if (num1 < 0.800000011920929)
                        {
                            star.planetCount = 2;
                            star.planets = new PlanetData[star.planetCount];
                            if (num2 < 0.25)
                            {
                                int info_seed5 = dotNet35Random2.Next();
                                int gen_seed5 = dotNet35Random2.Next();
                                int orbitIndex1 = num3 > 0.5 ? 3 : 2;
                                star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 0, 0, orbitIndex1, 1, false, info_seed5, gen_seed5);
                                int info_seed6 = dotNet35Random2.Next();
                                int gen_seed6 = dotNet35Random2.Next();
                                int orbitIndex2 = num3 > 0.5 ? 4 : 3;
                                star.planets[1] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 1, 0, orbitIndex2, 2, false, info_seed6, gen_seed6);
                            }
                            else
                            {
                                int info_seed7 = dotNet35Random2.Next();
                                int gen_seed7 = dotNet35Random2.Next();
                                star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 0, 0, 3, 1, true, info_seed7, gen_seed7);
                                int info_seed8 = dotNet35Random2.Next();
                                int gen_seed8 = dotNet35Random2.Next();
                                star.planets[1] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 1, 1, 1, 1, false, info_seed8, gen_seed8);
                            }
                        }
                        else
                        {
                            star.planetCount = 3;
                            star.planets = new PlanetData[star.planetCount];
                            if (num2 < 0.15000000596046448)
                            {
                                int info_seed9 = dotNet35Random2.Next();
                                int gen_seed9 = dotNet35Random2.Next();
                                int orbitIndex3 = num3 > 0.5 ? 3 : 2;
                                star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 0, 0, orbitIndex3, 1, false, info_seed9, gen_seed9);
                                int info_seed10 = dotNet35Random2.Next();
                                int gen_seed10 = dotNet35Random2.Next();
                                int orbitIndex4 = num3 > 0.5 ? 4 : 3;
                                star.planets[1] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 1, 0, orbitIndex4, 2, false, info_seed10, gen_seed10);
                                int info_seed11 = dotNet35Random2.Next();
                                int gen_seed11 = dotNet35Random2.Next();
                                int orbitIndex5 = num3 > 0.5 ? 5 : 4;
                                star.planets[2] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 2, 0, orbitIndex5, 3, false, info_seed11, gen_seed11);
                            }
                            else if (num2 < 0.75)
                            {
                                int info_seed12 = dotNet35Random2.Next();
                                int gen_seed12 = dotNet35Random2.Next();
                                int orbitIndex = num3 > 0.5 ? 3 : 2;
                                star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 0, 0, orbitIndex, 1, false, info_seed12, gen_seed12);
                                int info_seed13 = dotNet35Random2.Next();
                                int gen_seed13 = dotNet35Random2.Next();
                                star.planets[1] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 1, 0, 4, 2, true, info_seed13, gen_seed13);
                                int info_seed14 = dotNet35Random2.Next();
                                int gen_seed14 = dotNet35Random2.Next();
                                star.planets[2] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 2, 2, 1, 1, false, info_seed14, gen_seed14);
                            }
                            else
                            {
                                int info_seed15 = dotNet35Random2.Next();
                                int gen_seed15 = dotNet35Random2.Next();
                                int orbitIndex = num3 > 0.5 ? 4 : 3;
                                star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 0, 0, orbitIndex, 1, true, info_seed15, gen_seed15);
                                int info_seed16 = dotNet35Random2.Next();
                                int gen_seed16 = dotNet35Random2.Next();
                                star.planets[1] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 1, 1, 1, 1, false, info_seed16, gen_seed16);
                                int info_seed17 = dotNet35Random2.Next();
                                int gen_seed17 = dotNet35Random2.Next();
                                star.planets[2] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, 2, 1, 2, 2, false, info_seed17, gen_seed17);
                            }
                        }
                    }
                    
                }

                
            }
            else
            {
                Array.Clear((Array)___pGas, 0, ___pGas.Length);
                if (star.index == 0)
                {
                    star.planetCount = 4;
                    ___pGas[0] = 0.0;
                    ___pGas[1] = 0.0;
                    ___pGas[2] = 0.0;
                }
                else if (star.spectr == ESpectrType.M)
                {
                    star.planetCount = num1 >= 0.1 ? (num1 >= 0.3 ? (num1 >= 0.8 ? 4 : 3) : 2) : 1;
                    if (star.planetCount <= 3)
                    {
                        ___pGas[0] = 0.2;
                        ___pGas[1] = 0.2;
                    }
                    else
                    {
                        ___pGas[0] = 0.0;
                        ___pGas[1] = 0.2;
                        ___pGas[2] = 0.3;
                    }
                }
                else if (star.spectr == ESpectrType.K)
                {
                    star.planetCount = num1 >= 0.1 ? (num1 >= 0.2 ? (num1 >= 0.7 ? (num1 >= 0.95 ? 5 : 4) : 3) : 2) : 1;
                    if (star.planetCount <= 3)
                    {
                        ___pGas[0] = 0.18;
                        ___pGas[1] = 0.18;
                    }
                    else
                    {
                        ___pGas[0] = 0.0;
                        ___pGas[1] = 0.18;
                        ___pGas[2] = 0.28;
                        ___pGas[3] = 0.28;
                    }
                }
                else if (star.spectr == ESpectrType.G)
                {
                    star.planetCount = num1 >= 0.4 ? (num1 >= 0.9 ? 5 : 4) : 3;
                    if (star.planetCount <= 3)
                    {
                        ___pGas[0] = 0.18;
                        ___pGas[1] = 0.18;
                    }
                    else
                    {
                        ___pGas[0] = 0.0;
                        ___pGas[1] = 0.2;
                        ___pGas[2] = 0.3;
                        ___pGas[3] = 0.3;
                    }
                }
                else if (star.spectr == ESpectrType.F)
                {
                    star.planetCount = num1 >= 0.35 ? (num1 >= 0.8 ? 5 : 4) : 3;
                    if (star.planetCount <= 3)
                    {
                        ___pGas[0] = 0.2;
                        ___pGas[1] = 0.2;
                    }
                    else
                    {
                        ___pGas[0] = 0.0;
                        ___pGas[1] = 0.22;
                        ___pGas[2] = 0.31;
                        ___pGas[3] = 0.31;
                    }
                }
                else if (star.spectr == ESpectrType.A)
                {
                    star.planetCount = num1 >= 0.3 ? (num1 >= 0.75 ? 5 : 4) : 3;
                    if (star.planetCount <= 3)
                    {
                        ___pGas[0] = 0.2;
                        ___pGas[1] = 0.2;
                    }
                    else
                    {
                        ___pGas[0] = 0.1;
                        ___pGas[1] = 0.28;
                        ___pGas[2] = 0.3;
                        ___pGas[3] = 0.35;
                    }
                }
                else if (star.spectr == ESpectrType.B)
                {
                    star.planetCount = num1 >= 0.3 ? (num1 >= 0.75 ? 6 : 5) : 4;
                    if (star.planetCount <= 3)
                    {
                        ___pGas[0] = 0.2;
                        ___pGas[1] = 0.2;
                    }
                    else
                    {
                        ___pGas[0] = 0.1;
                        ___pGas[1] = 0.22;
                        ___pGas[2] = 0.28;
                        ___pGas[3] = 0.35;
                        ___pGas[4] = 0.35;
                    }
                }
                else if (star.spectr == ESpectrType.O)
                {
                    star.planetCount = num1 >= 0.5 ? 6 : 5;
                    ___pGas[0] = 0.1;
                    ___pGas[1] = 0.2;
                    ___pGas[2] = 0.25;
                    ___pGas[3] = 0.3;
                    ___pGas[4] = 0.32;
                    ___pGas[5] = 0.35;
                }
                else
                    star.planetCount = 1;
                star.planets = new PlanetData[star.planetCount];
                int num8 = 0;
                int num9 = 0;
                int orbitAround = 0;
                int num10 = 1;
                for (int index = 0; index < star.planetCount; ++index)
                {
                    int info_seed = dotNet35Random2.Next();
                    int gen_seed = dotNet35Random2.Next();
                    double num11 = dotNet35Random2.NextDouble();
                    double num12 = dotNet35Random2.NextDouble();
                    bool gasGiant = false;
                    if (orbitAround == 0)
                    {
                        ++num8;
                        if (index < star.planetCount - 1 && num11 < ___pGas[index])
                        {
                            gasGiant = true;
                            if (num10 < 3)
                                num10 = 3;
                        }
                        for (; star.index != 0 || num10 != 3; ++num10)
                        {
                            int num13 = star.planetCount - index;
                            int num14 = 9 - num10;
                            if (num14 > num13)
                            {
                                float a = (float)num13 / (float)num14;
                                float num15 = num10 <= 3 ? Mathf.Lerp(a, 1f, 0.15f) + 0.01f : Mathf.Lerp(a, 1f, 0.45f) + 0.01f;
                                if (dotNet35Random2.NextDouble() < (double)num15)
                                    goto label_62;
                            }
                            else
                                goto label_62;
                        }
                        gasGiant = true;
                    }
                    else
                    {
                        ++num9;
                        gasGiant = false;
                    }
                label_62:
                    star.planets[index] = PlanetGen.CreatePlanet(galaxy, star, gameDesc.savedThemeIds, index, orbitAround, orbitAround == 0 ? num10 : num9, orbitAround == 0 ? num8 : num9, gasGiant, info_seed, gen_seed);
                    ++num10;
                    if (gasGiant)
                    {
                        orbitAround = num8;
                        num9 = 0;
                    }
                    if (num9 >= 1 && num12 < 0.8)
                    {
                        orbitAround = 0;
                        num9 = 0;
                    }
                }
            }


            // 尾巴
            {
                int num16 = 0;
                int num17 = 0;
                int index1 = 0;
                for (int index2 = 0; index2 < star.planetCount; ++index2)
                {
                    if (star.planets[index2].type == EPlanetType.Gas)
                    {
                        num16 = star.planets[index2].orbitIndex;
                        break;
                    }
                }
                for (int index3 = 0; index3 < star.planetCount; ++index3)
                {
                    if (star.planets[index3].orbitAround == 0)
                        num17 = star.planets[index3].orbitIndex;
                }
                if (num16 > 0)
                {
                    int num18 = num16 - 1;
                    bool flag = true;
                    for (int index4 = 0; index4 < star.planetCount; ++index4)
                    {
                        if (star.planets[index4].orbitAround == 0 && star.planets[index4].orbitIndex == num16 - 1)
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag && num4 < 0.2 + (double)num18 * 0.2)
                        index1 = num18;
                }
                int index5 = num5 >= 0.2 ? (num5 >= 0.4 ? (num5 >= 0.8 ? 0 : num17 + 1) : num17 + 2) : num17 + 3;
                if (index5 != 0 && index5 < 5)
                    index5 = 5;
                star.asterBelt1OrbitIndex = (float)index1;
                star.asterBelt2OrbitIndex = (float)index5;
                if (index1 > 0)
                    star.asterBelt1Radius = StarGen.orbitRadius[index1] * (float)num6 * star.orbitScaler;
                if (index5 > 0)
                    star.asterBelt2Radius = StarGen.orbitRadius[index5] * (float)num7 * star.orbitScaler;
                for (int index6 = 0; index6 < star.planetCount; ++index6)
                {
                    PlanetData planet = star.planets[index6];
                    AccessTools.Method(typeof(StarGen), "SetHiveOrbitConditionFalse").Invoke(null, new object[] { planet.orbitIndex, planet.orbitAroundPlanet != null ? planet.orbitAroundPlanet.orbitIndex : 0, planet.sunDistance / star.orbitScaler, star.index });

                }
                star.hiveAstroOrbits = new AstroOrbitData[8];
                AstroOrbitData[] hiveAstroOrbits = star.hiveAstroOrbits;
                int number = 0;
                for (int index7 = 0; index7 < StarGen.hiveOrbitCondition.Length; ++index7)
                {
                    if (StarGen.hiveOrbitCondition[index7])
                        ++number;
                }
                for (int index8 = 0; index8 < 8; ++index8)
                {
                    double num19 = dotNet35Random3.NextDouble() * 2.0 - 1.0;
                    double num20 = dotNet35Random3.NextDouble();
                    double num21 = dotNet35Random3.NextDouble();
                    double num22 = (double)Math.Sign(num19) * Math.Pow(Math.Abs(num19), 0.7) * 90.0;
                    double num23 = num20 * 360.0;
                    double num24 = num21 * 360.0;
                    float num25 = 0.3f;
                    Assert.Positive(number);
                    if (number > 0)
                    {
                        int num26 = star.index != 0 ? 5 : 2;
                        int maxValue = (number > num26 ? num26 : number) * 100;
                        int num27 = maxValue * 100;
                        int num28 = dotNet35Random3.Next(maxValue);
                        int num29 = num28 * num28 / num27;
                        for (int index9 = 0; index9 < StarGen.hiveOrbitCondition.Length; ++index9)
                        {
                            if (StarGen.hiveOrbitCondition[index9])
                            {
                                if (num29 == 0)
                                {
                                    num25 = StarGen.hiveOrbitRadius[index9];
                                    StarGen.hiveOrbitCondition[index9] = false;
                                    --number;
                                    break;
                                }
                                --num29;
                            }
                        }
                    }
                    hiveAstroOrbits[index8] = new AstroOrbitData();
                    hiveAstroOrbits[index8].orbitRadius = num25 * star.orbitScaler;
                    hiveAstroOrbits[index8].orbitInclination = (float)num22;
                    hiveAstroOrbits[index8].orbitLongitude = (float)num23;
                    hiveAstroOrbits[index8].orbitPhase = (float)num24;
                    hiveAstroOrbits[index8].orbitalPeriod = Math.Sqrt(39.478417604357432 * (double)num25 * (double)num25 * (double)num25 / (1.3538551990520382E-06 * (double)star.mass));
                    hiveAstroOrbits[index8].orbitRotation = Quaternion.AngleAxis(hiveAstroOrbits[index8].orbitLongitude, Vector3.up) * Quaternion.AngleAxis(hiveAstroOrbits[index8].orbitInclination, Vector3.forward);
                    hiveAstroOrbits[index8].orbitNormal = Maths.QRotateLF(hiveAstroOrbits[index8].orbitRotation, new VectorLF3(0.0f, 1f, 0.0f)).normalized;
                }
            }

            
        }

        ///<summary>
        ///添加行星特质
        ///</summary>
        public static void AddPlantTraits(GalaxyData galaxy, StarData star, int[] themeIds, int index, int orbitAround, int orbitIndex, int number, bool gasGiant, int info_seed, int gen_seed, ref PlanetData __result)
        {
            if (!CustomCreateBirthStarPlugin.StarTypePatch) { return; }

            switch (star.id)
            {
                case STAR_ID_GIANT:
                    //潮汐锁定
                    //不能是巨星或者卫星
                    if (!gasGiant && orbitAround < 1)
                    {
                        if (index < 7 && index > 3)//潮汐锁定总数不超过3颗
                        {
                            __result.obliquity *= 0.01f;
                            __result.rotationPeriod = __result.orbitalPeriod;
                            __result.singularity = EPlanetSingularity.TidalLocked;
                        }
                    }
                    break;
                case STAR_BLACKHOLE:
                    break;
                case STAR_NEUTRONSTAR:
                    break;
                case STAR_MAINSEQSTAR_O:
                    if (!gasGiant && orbitAround < 1)
                    {
                        if (index < 7 && index > 3)//潮汐锁定总数不超过3颗
                        {
                            __result.obliquity *= 0.01f;
                            __result.rotationPeriod = __result.orbitalPeriod;
                            __result.singularity = EPlanetSingularity.TidalLocked;
                        }
                    }
                    break;
                default:
                    break;
            }
        }



    }
}
