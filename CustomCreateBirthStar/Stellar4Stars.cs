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

        private static void CreateStarPlanets(ref double[] ___pGas, ref GalaxyData galaxy, ref StarData star, ref GameDesc gameDesc)
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




                }
                
            }
            else
            {
                Array.Clear((Array)___pGas, 0, ___pGas.Length);
                star.planetCount = 6;
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
            if (index5 <= 0)
                return;
            star.asterBelt2Radius = StarGen.orbitRadius[index5] * (float)num7 * star.orbitScaler;
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
