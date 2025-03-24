using HarmonyLib;
using Steamworks;
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
    ///创建特殊的黑洞星区
    ///</summary>
    public static class SuperMassBlackHole
    {
        ///<summary>
        ///超大黑洞
        ///</summary>
        private const int STAR_ID_BigHole = 28;
        ///<summary>
        ///白巨星A
        ///</summary>
        private const int STAR_ID_Custom1 = 29;
        ///<summary>
        ///黄巨星G
        ///</summary>
        private const int STAR_ID_Custom2 = 30;
        ///<summary>
        ///蓝巨星O
        ///</summary>
        private const int STAR_ID_Custom3 = 31;
        ///<summary>
        ///红巨星M
        ///</summary>
        private const int STAR_ID_Custom4 = 32;
        
        ///<summary>
        ///超大黑洞位置
        ///</summary>
        private static VectorLF3 BigHoleVector;
        

        ///<summary>
        ///准备一个巨大黑洞及其附属恒星群，设置恒星类型与坐标
        ///</summary>
        public static void Prepare(ref GalaxyData galaxy, ref VectorLF3 pos, ref int id, ref int seed, ref EStarType needtype, ref ESpectrType needSpectr)
        {
            if (!CustomCreateBirthStarPlugin.NewSuperMassBlackHole) { return; }
            //超大黑洞
            if (id == STAR_ID_BigHole)
            {
                //Util.Log("------------------------------------------------------");
                //Util.Log("设置超重黑洞: id = " + id.ToString() + " starCount = " + galaxy.starCount.ToString());
                //DotNet35Random dotNet35Random = new DotNet35Random(galaxy.seed);
                //double Rand = (float)dotNet35Random.NextDouble();
                double Rand = 1;
                needtype = EStarType.BlackHole;
                needSpectr = ESpectrType.X;
                pos.x = Rand * 20;
                pos.y = Rand * 20;
                pos.z = 135;
                BigHoleVector = pos;
            }
            //生成特殊颜色的巨星
            else if (id == STAR_ID_Custom1)
            {
                //Util.Log("------------------------------------------------------");
                //Util.Log("设置白巨星: id = " + id.ToString() + " starCount = " + galaxy.starCount.ToString());
                needtype = EStarType.GiantStar;
                needSpectr = ESpectrType.A;
                pos.x = BigHoleVector.x - 2.5;
                pos.y = BigHoleVector.y;
                pos.z = BigHoleVector.z - 2.5;
            }
            else if (id == STAR_ID_Custom2)
            {
                //Util.Log("------------------------------------------------------");
                //Util.Log("设置黄巨星: id = " + id.ToString() + " starCount = " + galaxy.starCount.ToString());
                needtype = EStarType.GiantStar;
                needSpectr = ESpectrType.G;
                pos.x = BigHoleVector.x + 2.5;
                pos.y = BigHoleVector.y;
                pos.z = BigHoleVector.z + 2.5;
            }
            else if (id == STAR_ID_Custom3)
            {
                //Util.Log("------------------------------------------------------");
                //Util.Log("设置蓝巨星: id = " + id.ToString() + " starCount = " + galaxy.starCount.ToString());
                needtype = EStarType.GiantStar;
                needSpectr = ESpectrType.O;
                pos.x = BigHoleVector.x - 2.5;
                pos.y = BigHoleVector.y;
                pos.z = BigHoleVector.z + 2.5;
            }
            else if (id == STAR_ID_Custom4)
            {
                //Util.Log("------------------------------------------------------");
                //Util.Log("设置红巨星: id = " + id.ToString() + " starCount = " + galaxy.starCount.ToString());
                needtype = EStarType.GiantStar;
                needSpectr = ESpectrType.M;
                pos.x = BigHoleVector.x + 2.5;
                pos.y = BigHoleVector.y;
                pos.z = BigHoleVector.z - 2.5;
            }
        }

        ///<summary>
        ///设置参数， 创建一个巨大黑洞及其附属恒星群，设置恒星属性
        ///</summary>
        public static void Create(ref StarData __result, int id, int seed)
        {
            if (!CustomCreateBirthStarPlugin.NewSuperMassBlackHole) { return; }
            //DotNet35Random dotNet35Random = new DotNet35Random(seed);
            //double Rand = (float)dotNet35Random.NextDouble();
            double Rand = 1;
            if (id == STAR_ID_Custom1)
            {
                //Util.Log("创建白巨星: id = " + id.ToString());
                __result.mass = 4.280622f + (float)(Rand * 0.5);
                __result.lifetime = 194.1946f + (float)(Rand * 20);
                __result.age = 0.9697264f + (float)(Rand * 0.08);
                __result.type = EStarType.GiantStar;
                __result.temperature = 9900.992f + (float)(Rand * 100);
                __result.spectr = ESpectrType.A;
                __result.classFactor = 0.4409298f;
                __result.color = 0.788186f;
                __result.luminosity = 4.147003f;
                __result.radius = 4.960629f + (float)(Rand * 0.5);
                __result.habitableRadius = 35.11641f + 100;
                __result.lightBalanceRadius = 105.3492f;
                __result.dysonRadius = 1.92224f;
                __result.orbitScaler = 6.865142f;
                __result.level = 0.9919679f;
                __result.resourceCoef = 60.0f;
                __result.name = "残破庇护所 B";
                __result.safetyFactor = 0.09f;
                Util.OutputStarData(__result);
            }
            else if (id == STAR_ID_Custom2)
            {
                //Util.Log("创建黄巨星: id = " + id.ToString());
                __result.mass = 0.9786646f + (float)(Rand * 0.5);
                __result.lifetime = 3705.706f + (float)(Rand * 20);
                __result.age = 0.9654025f + (float)(Rand * 0.08);
                __result.type = EStarType.GiantStar;
                __result.temperature = 4466.139f + (float)(Rand * 100);
                __result.spectr = ESpectrType.G;
                __result.classFactor = -2.383485f;
                __result.color = 0.2233031f;
                __result.luminosity = 1.476674f;
                __result.radius = 14.48091f + (float)(Rand * 0.5);
                __result.habitableRadius = 9.592921f;
                __result.lightBalanceRadius = 28.77876f;
                __result.dysonRadius = 0.8838208f;
                __result.orbitScaler = 3.156503f;
                __result.level = 0.9919679f;
                __result.resourceCoef = 60.0f;
                __result.name = "残破庇护所 A";
                __result.safetyFactor = 0.89f;
                Util.OutputStarData(__result);

            }
            else if (id == STAR_ID_Custom3)
            {
                //Util.Log("创建蓝巨星: id = " + id.ToString());
                __result.spectr = ESpectrType.O;
                __result.luminosity = 28.656674f + (float)(Rand * 0.3);//修改恒星光度
                //__result.habitableRadius = 60.5f;
                __result.resourceCoef = 60.0f;
                __result.name = "残破工业区";

                // 战斗相关
                __result.safetyFactor = 0.50f;
                __result.initialHiveCount = 1 + (int)(CustomCreateBirthStarPlugin.DarkFogLv.Value * 0.5);
                __result.hivePatternLevel = 1 + (int)(CustomCreateBirthStarPlugin.DarkFogLv.Value * 0.5);
                __result.maxHiveCount = 8;
                Util.OutputStarData(__result);
            }
            else if (id == STAR_ID_Custom4)
            {
                //Util.Log("创建红巨星: id = " + id.ToString());
                __result.spectr = ESpectrType.M;
                __result.name = "古战场";
                __result.safetyFactor = 0.001f;
                __result.resourceCoef = 60.0f;
                // 战斗相关
                __result.safetyFactor = 0.001f;
                __result.initialHiveCount = 1 + (int)(CustomCreateBirthStarPlugin.DarkFogLv.Value * 0.8);
                __result.hivePatternLevel = 1 + (int)(CustomCreateBirthStarPlugin.DarkFogLv.Value * 0.8);
                __result.maxHiveCount = 8;
                Util.OutputStarData(__result);
            }
            else if (id == STAR_ID_BigHole)
            {
                //Util.Log("创建超重黑洞: id = " + id.ToString());
                __result.mass = 100.57f + (float)(Rand * 100); //300
                __result.lifetime = 0.001710792f + 1000 + (float)(Rand * 200);
                __result.age = 1.122148f + 1000 + (float)(Rand * 100);
                __result.type = EStarType.BlackHole;
                __result.temperature = 0f;
                __result.spectr = ESpectrType.X;
                __result.classFactor = 2f;
                __result.color = 1f;
                __result.luminosity = 0.000001f;
                __result.radius = 18.73534f + (float)(Rand * 2.5);// 48
                __result.acdiskRadius = 243.6767f;
                __result.habitableRadius = 0f;
                __result.lightBalanceRadius = 3.178286f;
                __result.dysonRadius = 0.19309f * 0;
                __result.orbitScaler = 3.321507f;
                __result.level = 1f;
                // 修改资源系数
                __result.resourceCoef = CustomCreateBirthStarPlugin.resourceCoef.Value;
                
                __result.name = "墓地";
                // 战斗相关
                __result.safetyFactor = 0.001f;
                __result.initialHiveCount = CustomCreateBirthStarPlugin.DarkFogLv.Value;
                __result.hivePatternLevel = CustomCreateBirthStarPlugin.DarkFogLv.Value;
                __result.maxHiveCount = 8;
                Util.OutputStarData(__result);

                


            }
            else {
                return;
            }
            
            //Util.OutputStarData(__result);
        }

        ///<summary>
        ///创建行星
        ///</summary>
        public static bool CreatePlanet(ref double[] ___pGas, ref GalaxyData galaxy, ref StarData star, ref GameDesc gameDesc)
        {
            if (!CustomCreateBirthStarPlugin.NewSuperMassBlackHole) { return true; }
            if (star.id == STAR_ID_BigHole)
            {
                //Util.Log("创建黑洞行星: id = " + star.id.ToString());
                CreateStarPlanets(ref ___pGas, ref galaxy, ref star, ref gameDesc);
                return false;
            }
            else if(star.id == STAR_ID_Custom1)
            {
                //Util.Log("创建白巨星行星: id = " + star.id.ToString());
                CreateStarPlanets(ref ___pGas, ref galaxy, ref star, ref gameDesc);
                return false;
            }
            else if(star.id == STAR_ID_Custom2)
            {
                //Util.Log("创建黄巨星行星: id = " + star.id.ToString());
                CreateStarPlanets(ref ___pGas, ref galaxy, ref star, ref gameDesc);
                return false;
            }
            else if(star.id == STAR_ID_Custom3)
            {
                //Util.Log("创建蓝巨星行星: id = " + star.id.ToString());
                CreateStarPlanets(ref ___pGas, ref galaxy, ref star, ref gameDesc);
                return false;
            }
            else if(star.id == STAR_ID_Custom4)
            {
                //Util.Log("创建红巨星行星: id = " + star.id.ToString());
                CreateStarPlanets(ref ___pGas, ref galaxy, ref star, ref gameDesc);
                return false;
            }
            return true;
        }
        ///<summary>
        ///内部方法，创建行星
        ///</summary>
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
            DotNet35Random dotNet35Random3 = new DotNet35Random(dotNet35Random1.Next());

            // 不知道是什么玩意
            // StarGen.SetHiveOrbitsConditionsTrue();
            AccessTools.Method(typeof(StarGen), "SetHiveOrbitsConditionsTrue").Invoke(null, new object[] { /* 传递方法所需的参数 */ });

            if (star.type == EStarType.BlackHole)
            {

                int[] ThemeIds = { 7, 10, 20, 24 };
                int[] ThemeIds_Gas = { 2, 3, 4, 5, 21 };
                star.planetCount = 8;
                star.planets = new PlanetData[star.planetCount];
                //galaxy,  star, 行星主题数组， index：行星编号，orbitAround=绕谁转(0=恒星)，orbitIndex=轨道ID    ，number=卫星id（如果本身是卫星，则此参数固定为0），是否巨星
                star.planets[0] = Util.CreatePlanet(galaxy, star, ThemeIds, 0, 0, 3, 1, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                star.planets[1] = Util.CreatePlanet(galaxy, star, ThemeIds, 1, 0, 4, 1, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                star.planets[2] = Util.CreatePlanet(galaxy, star, ThemeIds, 2, 0, 5, 1, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                star.planets[3] = Util.CreatePlanet(galaxy, star, ThemeIds, 3, 0, 6, 1, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                star.planets[4] = Util.CreatePlanet(galaxy, star, ThemeIds, 4, 0, 7, 1, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                star.planets[5] = Util.CreatePlanet(galaxy, star, ThemeIds, 5, 0, 10, 1, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                star.planets[6] = Util.CreatePlanet(galaxy, star, ThemeIds, 6, 0, 12, 1, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                star.planets[7] = Util.CreatePlanet(galaxy, star, ThemeIds_Gas, 7, 0, 13, 1, true, dotNet35Random2.Next(), dotNet35Random2.Next());

            }
            else if (star.type == EStarType.NeutronStar)
            {
                
            }
            else if (star.type == EStarType.WhiteDwarf)
            {
                
            }
            else if (star.type == EStarType.GiantStar)
            {
                if (star.id == STAR_ID_Custom1)
                {
                    int[] ThemeIds = { 6, 12, 17, 19, 20, 23 };
                    star.planetCount = 5;
                    star.planets = new PlanetData[star.planetCount];
                    //galaxy,star,行星主题数组，行星index（行星的编号），卫星(归属哪颗行星)，轨道index（占用第几轨道），卫星id（如果本身是卫星，则此参数固定为0），是否巨星
                    star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, ThemeIds, 0, 0, 1, 1, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                    star.planets[1] = PlanetGen.CreatePlanet(galaxy, star, ThemeIds, 1, 0, 3, 1, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                    star.planets[2] = PlanetGen.CreatePlanet(galaxy, star, ThemeIds, 2, 0, 4, 1, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                    star.planets[3] = PlanetGen.CreatePlanet(galaxy, star, ThemeIds, 3, 0, 5, 1, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                    star.planets[4] = PlanetGen.CreatePlanet(galaxy, star, ThemeIds, 4, 0, 7, 1, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                }
                else if (star.id == STAR_ID_Custom2)
                {
                    int[] ThemeIds = { 8, 15, 16, 18, 22, 25 };
                    foreach (var Id in ThemeIds)
                    {
                        LDB.themes.Select(Id).PlanetType = EPlanetType.Desert;
                    }
                    star.planetCount = 6;
                    star.planets = new PlanetData[star.planetCount];
                    star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, ThemeIds, 0, 0, 1, 1, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                    star.planets[1] = PlanetGen.CreatePlanet(galaxy, star, ThemeIds, 1, 0, 3, 1, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                    star.planets[2] = PlanetGen.CreatePlanet(galaxy, star, ThemeIds, 2, 0, 4, 1, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                    star.planets[3] = PlanetGen.CreatePlanet(galaxy, star, ThemeIds, 3, 0, 6, 1, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                    star.planets[4] = PlanetGen.CreatePlanet(galaxy, star, ThemeIds, 4, 0, 8, 1, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                    star.planets[5] = PlanetGen.CreatePlanet(galaxy, star, ThemeIds, 5, 0, 9, 1, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                    foreach (var Id in ThemeIds)
                    {
                        LDB.themes.Select(Id).PlanetType = EPlanetType.Ocean;
                    }
                }
                else if(star.id == STAR_ID_Custom3)
                {
                    //五个巨星, 一堆卫星
                    int[] 气态巨星主题数组 = { 2, 3, 4, 5, 21 };
                    int[] 宜居星球主题数组 = { 1, 8, 14, 15, 18, 22, 25 };
                    int[] 随机星球主题数组 = gameDesc.savedThemeIds;
                    
                    if (CustomCreateBirthStarPlugin.MorePlanet)
                    {
                        star.planetCount = 13;
                    }
                    else
                    {
                        star.planetCount = 8;
                    }

                    star.planets = new PlanetData[star.planetCount];
                    // 创建一颗巨星
                    star.planets[0] = Util.CreatePlanet(galaxy, star, 气态巨星主题数组, 0, 0, 1, 1, true, dotNet35Random2.Next(), dotNet35Random2.Next());
                    // 它的卫星
                    {
                        star.planets[1] = Util.CreatePlanet(galaxy, star, 随机星球主题数组, 1, 1, 1, 0, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                        star.planets[2] = Util.CreatePlanet(galaxy, star, 随机星球主题数组, 2, 1, 2, 0, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                        star.planets[3] = Util.CreatePlanet(galaxy, star, 随机星球主题数组, 3, 1, 4, 0, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                        star.planets[4] = Util.CreatePlanet(galaxy, star, 随机星球主题数组, 4, 1, 5, 0, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                    }

                    // 创建一颗巨星
                    star.planets[5] = Util.CreatePlanet(galaxy, star, 气态巨星主题数组, 5, 0, 3, 3, true, dotNet35Random2.Next(), dotNet35Random2.Next());
                    // 它的卫星
                    {
                        star.planets[6] = Util.CreatePlanet(galaxy, star, 随机星球主题数组, 6, 3, 2, 0, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                        // 设置为母星
                        if (CustomCreateBirthStarPlugin.BirthStarAtSuperMassBlackHole.Value)
                        {
                            CustomCreateBirthStarPlugin.BirthStarId = star.id;
                            CustomCreateBirthStarPlugin.BirthPlanetId = star.planets[6].id;
                        }
                    }
                    // 创建一颗巨星
                    star.planets[7] = Util.CreatePlanet(galaxy, star, 气态巨星主题数组, 7, 0, 5, 5, true, dotNet35Random2.Next(), dotNet35Random2.Next());


                    if (CustomCreateBirthStarPlugin.MorePlanet)
                    {
                        // 创建一颗巨星
                        star.planets[8] = Util.CreatePlanet(galaxy, star, 气态巨星主题数组, 8, 0, 8, 8, true, dotNet35Random2.Next(), dotNet35Random2.Next());
                        // 它的卫星
                        {
                            Util.ModifyPlanetTheme(宜居星球主题数组, EPlanetType.Desert);
                            star.planets[9] = Util.CreatePlanet(galaxy, star, 宜居星球主题数组, 9, 8, 1, 0, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                            star.planets[10] = Util.CreatePlanet(galaxy, star, 宜居星球主题数组, 10, 8, 3, 0, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                            Util.ModifyPlanetTheme(宜居星球主题数组, EPlanetType.Ocean);
                            // 设置为母星，覆盖上面的
                            if (CustomCreateBirthStarPlugin.BirthStarAtSuperMassBlackHole.Value)
                            {
                                CustomCreateBirthStarPlugin.BirthStarId = star.id;
                                CustomCreateBirthStarPlugin.BirthPlanetId = star.planets[10].id;
                            }

                        }

                        star.planets[11] = Util.CreatePlanet(galaxy, star, 随机星球主题数组, 11, 0, 6, 6, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                        star.planets[12] = Util.CreatePlanet(galaxy, star, 气态巨星主题数组, 12, 0, 7, 7, true, dotNet35Random2.Next(), dotNet35Random2.Next());

                    }





                }
                else if(star.id == STAR_ID_Custom4)
                {
                    //9个火山星球
                    //将火山星球的类型改为荒漠, 欺骗系统
                    LDB.themes.Select(9).PlanetType = EPlanetType.Desert;
                    int[] ThemeIds = { 9 };
                    star.planetCount = 8;
                    star.planets = new PlanetData[star.planetCount];
                    star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, ThemeIds, 0, 0, 1, 1, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                    star.planets[1] = PlanetGen.CreatePlanet(galaxy, star, ThemeIds, 1, 0, 2, 1, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                    star.planets[2] = PlanetGen.CreatePlanet(galaxy, star, ThemeIds, 2, 0, 3, 1, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                    star.planets[3] = PlanetGen.CreatePlanet(galaxy, star, ThemeIds, 3, 0, 4, 1, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                    star.planets[4] = PlanetGen.CreatePlanet(galaxy, star, ThemeIds, 4, 0, 5, 1, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                    star.planets[5] = PlanetGen.CreatePlanet(galaxy, star, ThemeIds, 5, 0, 7, 1, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                    star.planets[6] = PlanetGen.CreatePlanet(galaxy, star, ThemeIds, 6, 0, 8, 1, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                    star.planets[7] = PlanetGen.CreatePlanet(galaxy, star, ThemeIds, 7, 0, 9, 1, false, dotNet35Random2.Next(), dotNet35Random2.Next());
                    //将类型改回去
                    LDB.themes.Select(9).PlanetType = EPlanetType.Vocano;
                }
            }

            // 尾巴
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
                    //StarGen.SetHiveOrbitConditionFalse(planet.orbitIndex, planet.orbitAroundPlanet != null ? planet.orbitAroundPlanet.orbitIndex : 0, planet.sunDistance / star.orbitScaler, star.index);
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
            if (!CustomCreateBirthStarPlugin.NewSuperMassBlackHole) { return; }

            if (star.id == STAR_ID_BigHole)
            {
                return ;
            }
            else if (star.id == STAR_ID_Custom1)
            {
                //6, 19, 23，给沙漠星球增加风速特质
                if(__result.theme == 6) // 干旱荒漠
                {
                    //Util.Log("添加风速特质: id = " + star.id.ToString());
                    __result.windStrength = 5.0f;
                    __result.luminosity = 2.0f;

                }
                else if(__result.theme == 19) // 三色荒漠
                {
                    __result.windStrength = 9.8f;
                    __result.luminosity = 3.0f;

                }else if(__result.theme == 23) // 橙晶荒漠
                {
                    __result.windStrength = 7.0f;
                    __result.luminosity = 4.0f;
                }
                //if (__result.windStrength >=1f)
                //{
                //    //Util.Log("添加风速特质: id = " + star.id.ToString());
                //    __result.windStrength *= 10;
                //}

                //if (__result.luminosity >=1f)
                //{
                //    //Util.Log("添加光强特质: id = " + star.id.ToString());
                //    __result.luminosity *= 10;
                //}
                return;
            }
            else if (star.id == STAR_ID_Custom2)
            {

                return ;
            }
            else if (star.id == STAR_ID_Custom3)
            {
                
                return ;
            }
            else if (star.id == STAR_ID_Custom4)
            {
                
                return ;
            }
            return ;
        }


        ///<summary>
        ///当矿石被清理时，触发抽奖
        ///</summary>
        public static void OnMine(ref PlanetFactory __instance, int id)
        {
            //检查全局功能开关
            if (!CustomCreateBirthStarPlugin.NewSuperMassBlackHole) { return; }
            if (!CustomCreateBirthStarPlugin.Relics) { return; }
            //参数合法性校验
            if (__instance.vegePool[id].id == 0) { return; }
            //校验是否为指定离散矿石类型
            if (Util.IsStone(__instance.vegePool[id].protoId) == false) { return; }
            //校验是否为指定恒星系
            if (__instance.planet.star.id != SuperMassBlackHole.STAR_ID_BigHole) { return; }
            int ItemProtoID = 0;
            int ItemCount = 0;
            DotNet35Random DotNet35Random = new DotNet35Random();
            double Random = DotNet35Random.NextDouble();


            if(LDB.items.Select(ProtoID.物品.星际能量枢纽) == null)
            {
                return;
            }

            bool isFirstPlant = false;
            if (__instance.planet.star.planets.Length > 0)
            {
                isFirstPlant = __instance.planet.star.planets[0].id == __instance.planet.id ? true : false;
            }

            

            //0.010概率获得星际电力枢纽
            if (Random > 0.00001 && Random <= 0.01001)
            {
                // 科技未解锁时才能挖到
                if (__instance.gameData.history.TechUnlocked(ProtoID.Tech.古代技术_星际电力传输) == false)
                {
                    ItemProtoID = ProtoID.物品.星际能量枢纽;
                    ItemCount = isFirstPlant ? 5 : 1;
                }
                    
            }
            //0.008概率直接解锁蓝图
            else if (Random > 0.0100 && Random <= 0.0180)
            {
                if (isFirstPlant)
                {
                    if (__instance.gameData.history.TechUnlocked(ProtoID.Tech.古代技术_星际电力传输) == false)
                    {
                        __instance.gameData.history.UnlockTech(ProtoID.Tech.古代技术_星际电力传输);
                        //Util.Log("解锁科技：" + LDB.techs.Select(ProtoID.Tech.古代技术_星际电力传输).name);
                        ItemProtoID = ProtoID.物品.星际能量枢纽;
                        ItemCount = 10;
                    }
                    else if (__instance.gameData.history.TechUnlocked(ProtoID.Tech.古代技术_行星级无线输电) == false)
                    {
                        __instance.gameData.history.UnlockTech(ProtoID.Tech.古代技术_行星级无线输电);
                        //Util.Log("解锁科技：" + LDB.techs.Select(ProtoID.Tech.古代技术_行星级无线输电).name);
                        ItemProtoID = ProtoID.物品.星际能量枢纽MK2;
                        ItemCount = 5;
                    }
                }
            }
            //0.01概率获得能量核心*5
            else if (Random > 0.201 && Random <= 0.211)
            {
                ItemProtoID = ProtoID.物品.能量核心;
                ItemCount = isFirstPlant ? 50 : 5;
            }//0.02概率获得能量核心*2
            else if (Random > 0.211 && Random <= 0.231)
            {
                ItemProtoID = ProtoID.物品.能量核心;
                ItemCount = isFirstPlant ? 20 : 2;
            }//0.04概率获得能量核心*1
            else if (Random > 0.231 && Random <= 0.271)
            {
                ItemProtoID = ProtoID.物品.能量核心;
                ItemCount = isFirstPlant ? 10 : 1;
            }

            if (ItemProtoID < 1)
            {
                //Util.Log("未获得道具，随机数：" + Random.ToString());
                return;
            }
            //Util.Log("恒星ID：" + __instance.planet.star.id.ToString() + "  行星ID：" + __instance.planet.id.ToString() + "  对象ID：" + id.ToString() + "  对象类型：" + __instance.vegePool[id].protoId.ToString());
            

            GainTechAwards(ItemProtoID, ItemCount);

        }


        ///<summary>
        ///添加物品奖励
        ///</summary>
        public static void GainTechAwards(int itemId, int count)
        {
            int package = GameMain.mainPlayer.TryAddItemToPackage(itemId, count, count, true);
            if (package < count)
            {
                UIRealtimeTip.Popup("无法获得科技奖励".Translate());
                //Util.Log("物品添加失败");
                Util.Log(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, "无法获得科技奖励");
            }
            else
            {
                UIItemup.Up(itemId, count);
                UIRealtimeTip.Popup("?!!".Translate());
                //Util.Log("添加物品至背包：" + itemId.ToString());
                Util.Log(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, "添加物品至背包");
            }
            
        }

    }

}
