using HarmonyLib;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CustomCreateBirthStar
{
    public static class Util
    {
        //StarGen复制过来的工具方法， 用来生成恒星数据
        /*
         * 
        public static StarData CreateStar(GalaxyData galaxy, VectorLF3 pos, int id, int seed, EStarType needtype, ESpectrType needSpectr = ESpectrType.X, int Mass = 1)
        {
            StarData starData = new StarData()
            {
                galaxy = galaxy,
                index = id - 1
            };
            starData.level = galaxy.starCount <= 1 ? 0.0f : (float)starData.index / (float)(galaxy.starCount - 1);
            starData.id = id;
            starData.seed = seed;
            DotNet35Random dotNet35Random1 = new DotNet35Random(seed);
            int seed1 = dotNet35Random1.Next();
            int Seed = dotNet35Random1.Next();
            starData.position = pos;
            float num1 = (float)pos.magnitude / 32f;
            if ((double)num1 > 1.0)
            {
                num1 = Mathf.Log(Mathf.Log(Mathf.Log(Mathf.Log(Mathf.Log(num1) + 1f) + 1f) + 1f) + 1f) + 1f;
            }
            starData.resourceCoef = Mathf.Pow(7f, num1) * 0.6f;
            DotNet35Random dotNet35Random2 = new DotNet35Random(Seed);
            double r1 = dotNet35Random2.NextDouble();
            double r2 = dotNet35Random2.NextDouble();
            double num2 = dotNet35Random2.NextDouble();
            double rn = dotNet35Random2.NextDouble();
            double rt = dotNet35Random2.NextDouble();
            double num3 = (dotNet35Random2.NextDouble() - 0.5) * 0.2;
            double num4 = dotNet35Random2.NextDouble() * 0.2 + 0.9;
            double y = dotNet35Random2.NextDouble() * 0.4 - 0.2;
            double num5 = Math.Pow(2.0, y);
            float num6 = Mathf.Lerp(-0.98f, 0.88f, starData.level);
            float averageValue = (double)num6 >= 0.0 ? num6 + 0.65f : num6 - 0.65f;
            float standardDeviation = 0.33f;
            if (needtype == EStarType.GiantStar)
            {
                averageValue = y > -0.08 ? -1.5f : 1.6f;
                standardDeviation = 0.3f;
            }
            float num7 = RandNormal(averageValue, standardDeviation, r1, r2);
            switch (needSpectr)
            {
                case ESpectrType.M:
                    num7 = -3f;
                    break;
                case ESpectrType.O:
                    num7 = 3f;
                    break;
            }
            float p1 = (float)((double)Mathf.Clamp((double)num7 <= 0.0 ? num7 * 1f : num7 * 2f, -2.4f, 4.65f) + num3 + 1.0);
            switch (needtype)
            {
                case EStarType.WhiteDwarf:
                    starData.mass = (float)(1.0 + r2 * 5.0);
                    break;
                case EStarType.NeutronStar:
                    starData.mass = (float)(7.0 + r1 * 11.0);
                    break;
                case EStarType.BlackHole:
                    starData.mass = (float)(18.0 + r1 * r2 * 30.0);
                    break;
                default:
                    starData.mass = Mathf.Pow(2f, p1);
                    break;
            }
            //==============================================
            starData.mass = starData.mass * Mass;
            //==============================================
            double d = 5.0;
            if ((double)starData.mass < 2.0)
                d = 2.0 + 0.4 * (1.0 - (double)starData.mass);
            starData.lifetime = (float)(10000.0 * Math.Pow(0.1, Math.Log10((double)starData.mass * 0.5) / Math.Log10(d) + 1.0) * num4);
            switch (needtype)
            {
                case EStarType.GiantStar:
                    starData.lifetime = (float)(10000.0 * Math.Pow(0.1, Math.Log10((double)starData.mass * 0.58) / Math.Log10(d) + 1.0) * num4);
                    starData.age = (float)(num2 * 0.0399999991059303 + 0.959999978542328);
                    break;
                case EStarType.WhiteDwarf:
                case EStarType.NeutronStar:
                case EStarType.BlackHole:
                    starData.age = (float)(num2 * 0.400000005960464 + 1.0);
                    if (needtype == EStarType.WhiteDwarf)
                    {
                        starData.lifetime += 10000f;
                        break;
                    }
                    if (needtype == EStarType.NeutronStar)
                    {
                        starData.lifetime += 1000f;
                        break;
                    }
                    break;
                default:
                    starData.age = (double)starData.mass >= 0.5 ? ((double)starData.mass >= 0.8 ? (float)(num2 * 0.699999988079071 + 0.200000002980232) : (float)(num2 * 0.400000005960464 + 0.100000001490116)) : (float)(num2 * 0.119999997317791 + 0.0199999995529652);
                    break;
            }
            float num8 = starData.lifetime * starData.age;
            if ((double)num8 > 5000.0)
                num8 = (float)(((double)Mathf.Log(num8 / 5000f) + 1.0) * 5000.0);
            if ((double)num8 > 8000.0)
                num8 = (float)(((double)Mathf.Log(Mathf.Log(Mathf.Log(num8 / 8000f) + 1f) + 1f) + 1.0) * 8000.0);
            starData.lifetime = num8 / starData.age;
            float f = (float)(1.0 - (double)Mathf.Pow(Mathf.Clamp01(starData.age), 20f) * 0.5) * starData.mass;
            starData.temperature = (float)(Math.Pow((double)f, 0.56 + 0.14 / (Math.Log10((double)f + 4.0) / Math.Log10(5.0))) * 4450.0 + 1300.0);
            double num9 = Math.Log10(((double)starData.temperature - 1300.0) / 4500.0) / Math.Log10(2.6) - 0.5;
            if (num9 < 0.0)
                num9 *= 4.0;
            if (num9 > 2.0)
                num9 = 2.0;
            else if (num9 < -4.0)
                num9 = -4.0;
            starData.spectr = (ESpectrType)Mathf.RoundToInt((float)num9 + 4f);
            starData.color = Mathf.Clamp01((float)((num9 + 3.5) * 0.200000002980232));
            starData.classFactor = (float)num9;
            starData.luminosity = Mathf.Pow(f, 0.7f);
            starData.radius = (float)(Math.Pow((double)starData.mass, 0.4) * num5);
            starData.acdiskRadius = 0.0f;
            float p2 = (float)num9 + 2f;
            starData.habitableRadius = Mathf.Pow(1.7f, p2) + 0.25f * Mathf.Min(1f, starData.orbitScaler);
            starData.lightBalanceRadius = Mathf.Pow(1.7f, p2);
            starData.orbitScaler = Mathf.Pow(1.35f, p2);
            if ((double)starData.orbitScaler < 1.0)
                starData.orbitScaler = Mathf.Lerp(starData.orbitScaler, 1f, 0.6f);
            StarGen.SetStarAge(starData, starData.age, rn, rt);
            starData.dysonRadius = starData.orbitScaler * 0.28f;
            if ((double)starData.dysonRadius * 40000.0 < (double)starData.physicsRadius * 1.5)
                starData.dysonRadius = (float)((double)starData.physicsRadius * 1.5 / 40000.0);
            starData.uPosition = starData.position * 2400000.0;
            starData.name = NameGen.RandomStarName(seed1, starData, galaxy);
            starData.overrideName = "";
            return starData;
        }
        */


        /// <summary>
        /// 创建行星
        /// </summary>
        /// <param name="galaxy">银河系<para /></param>
        /// <param name="star">恒星<para /></param>
        /// <param name="行星主题数组">行星主题数组<para /></param>
        /// <param name="index">行星ID，0 - planetCount<para /></param>
        /// <param name="orbitAround">绕谁转(0 = 恒星)<para /></param>
        /// <param name="orbitIndex">轨道ID，1 - 13<para /></param>
        /// <param name="number">卫星ID（如果本身是卫星，则此参数固定为0）<para /></param>
        /// <param name="gasGiant">是否为巨星<para /></param>
        /// <param name="info_seed">种子1<para /></param>
        /// <param name="gen_seed">种子2<para /></param>
        /// <returns>PlanetData</returns>
        public static PlanetData CreatePlanet(GalaxyData galaxy, StarData star, int[] 行星主题数组, int 行星ID, int 绕谁转, int 轨道ID, int 卫星ID, bool 是否为巨星, int info_seed, int gen_seed)
        {
            return PlanetGen.CreatePlanet(galaxy, star, 行星主题数组, 行星ID, 绕谁转, 轨道ID, 卫星ID, 是否为巨星, info_seed, gen_seed);
        }

        private static float RandNormal(float averageValue, float standardDeviation, double r1, double r2)
        {
            return averageValue + standardDeviation * (float)(Math.Sqrt(-2.0 * Math.Log(1.0 - r1)) * Math.Sin(2.0 * Math.PI * r2));
        }

        //输出恒星数据
        public static void OutputStarData(StarData Star)
        {
            if (!CustomCreateBirthStarPlugin.Debug)
            {
                return;
            }
            CustomCreateBirthStarPlugin.logger.LogInfo("seed = " + Star.seed.ToString());
            CustomCreateBirthStarPlugin.logger.LogInfo("index = " + Star.index.ToString());
            CustomCreateBirthStarPlugin.logger.LogInfo("id = " + Star.id.ToString());
            CustomCreateBirthStarPlugin.logger.LogInfo("name(名称) = " + Star.name);
            CustomCreateBirthStarPlugin.logger.LogInfo("overrideName(名称) = " + Star.overrideName);
            CustomCreateBirthStarPlugin.logger.LogInfo("mass(质量) = " + Star.mass.ToString());
            CustomCreateBirthStarPlugin.logger.LogInfo("lifetime(寿命) = " + Star.lifetime.ToString());
            CustomCreateBirthStarPlugin.logger.LogInfo("age(年龄) = " + Star.age.ToString());

            CustomCreateBirthStarPlugin.logger.LogInfo("type(类型) = " + Star.type);
            CustomCreateBirthStarPlugin.logger.LogInfo("temperature(温度) = " + Star.temperature.ToString());
            CustomCreateBirthStarPlugin.logger.LogInfo("spectr(光谱)= " + Star.spectr.ToString());
            CustomCreateBirthStarPlugin.logger.LogInfo("classFactor = " + Star.classFactor.ToString());

            CustomCreateBirthStarPlugin.logger.LogInfo("color = " + Star.color);
            CustomCreateBirthStarPlugin.logger.LogInfo("luminosity(光度) = " + Star.luminosity.ToString());
            CustomCreateBirthStarPlugin.logger.LogInfo("radius(半径) = " + Star.radius.ToString());
            CustomCreateBirthStarPlugin.logger.LogInfo("acdiskRadius = " + Star.acdiskRadius.ToString());

            CustomCreateBirthStarPlugin.logger.LogInfo("habitableRadius(宜居半径) = " + Star.habitableRadius);
            CustomCreateBirthStarPlugin.logger.LogInfo("lightBalanceRadius = " + Star.lightBalanceRadius.ToString());
            CustomCreateBirthStarPlugin.logger.LogInfo("dysonRadius(戴森半径) = " + Star.dysonRadius.ToString());
            CustomCreateBirthStarPlugin.logger.LogInfo("orbitScaler = " + Star.orbitScaler.ToString());

            CustomCreateBirthStarPlugin.logger.LogInfo("asterBelt1OrbitIndex = " + Star.asterBelt1OrbitIndex);
            CustomCreateBirthStarPlugin.logger.LogInfo("asterBelt2OrbitIndex = " + Star.asterBelt2OrbitIndex.ToString());
            CustomCreateBirthStarPlugin.logger.LogInfo("asterBelt1Radius = " + Star.asterBelt1Radius.ToString());
            CustomCreateBirthStarPlugin.logger.LogInfo("asterBelt2Radius = " + Star.asterBelt2Radius.ToString());

            CustomCreateBirthStarPlugin.logger.LogInfo("planetCount(行星数量) = " + Star.planetCount);
            CustomCreateBirthStarPlugin.logger.LogInfo("level = " + Star.level.ToString());
            CustomCreateBirthStarPlugin.logger.LogInfo("resourceCoef(资源倍率) = " + Star.resourceCoef.ToString());

            CustomCreateBirthStarPlugin.logger.LogInfo("x = " + Star.position.x.ToString());
            CustomCreateBirthStarPlugin.logger.LogInfo("y = " + Star.position.y.ToString());
            CustomCreateBirthStarPlugin.logger.LogInfo("z = " + Star.position.z.ToString());
            CustomCreateBirthStarPlugin.logger.LogInfo("ux = " + Star.uPosition.x.ToString());
            CustomCreateBirthStarPlugin.logger.LogInfo("uy = " + Star.uPosition.y.ToString());
            CustomCreateBirthStarPlugin.logger.LogInfo("uz = " + Star.uPosition.z.ToString());

            CustomCreateBirthStarPlugin.logger.LogInfo("---------------------战斗相关----------------------");
            CustomCreateBirthStarPlugin.logger.LogInfo("initialHiveCount(巢穴数量) = " + Star.initialHiveCount.ToString());
            CustomCreateBirthStarPlugin.logger.LogInfo("hivePatternLevel(巢穴等级) = " + Star.hivePatternLevel.ToString());
            CustomCreateBirthStarPlugin.logger.LogInfo("safetyFactor(安全因数) = " + Star.safetyFactor.ToString());
            CustomCreateBirthStarPlugin.logger.LogInfo("maxHiveCount(最大巢穴数量) = " + Star.maxHiveCount.ToString());



        CustomCreateBirthStarPlugin.logger.LogInfo("-----------------------------------------------------");
        }

        // 日志
        public static void Log(object data)
        {
            CustomCreateBirthStarPlugin.logger.LogInfo(data);
        }

        public static void Log(string className, string methodName, string data)
        {
            CustomCreateBirthStarPlugin.logger.LogInfo(className + ":" + methodName + ":" + data);
        }

        //导出主题数据
        public static void outputThemeInfo(int[] themeIds)
        {
            int length1 = themeIds.Length;
            for (int index1 = 0; index1 < length1; ++index1)
            {
                ThemeProto themeProto = LDB.themes.Select(themeIds[index1]);
                CustomCreateBirthStarPlugin.logger.LogInfo("ID: " + themeProto.ID);
                CustomCreateBirthStarPlugin.logger.LogInfo("DisplayName: " + themeProto.DisplayName);
                CustomCreateBirthStarPlugin.logger.LogInfo("PlanetType: " + themeProto.PlanetType.ToString());
                CustomCreateBirthStarPlugin.logger.LogInfo("Algos: " + string.Join(" ", themeProto.Algos));
                CustomCreateBirthStarPlugin.logger.LogInfo("Temperature: " + themeProto.Temperature.ToString());
                CustomCreateBirthStarPlugin.logger.LogInfo("Distribute: " + themeProto.Distribute.ToString());
                CustomCreateBirthStarPlugin.logger.LogInfo("Vegetables0: " + string.Join(" ", themeProto.Vegetables0));
                CustomCreateBirthStarPlugin.logger.LogInfo("VeinSpot: " + string.Join(" ", themeProto.VeinSpot));
                CustomCreateBirthStarPlugin.logger.LogInfo("VeinCount: " + string.Join(" ", themeProto.VeinCount));
                CustomCreateBirthStarPlugin.logger.LogInfo("VeinOpacity: " + string.Join(" ", themeProto.VeinOpacity));
                CustomCreateBirthStarPlugin.logger.LogInfo("RareVeins: " + string.Join(" ", themeProto.RareVeins));
                CustomCreateBirthStarPlugin.logger.LogInfo("RareSettings: " + string.Join(" ", themeProto.RareSettings));
                CustomCreateBirthStarPlugin.logger.LogInfo("Wind: " + themeProto.Wind.ToString());
                CustomCreateBirthStarPlugin.logger.LogInfo("IonHeight: " + themeProto.IonHeight.ToString());
                CustomCreateBirthStarPlugin.logger.LogInfo("CullingRadius: " + themeProto.CullingRadius.ToString());
                CustomCreateBirthStarPlugin.logger.LogInfo("===================================");

            }
        }

        //检测对应物品是否为离散矿石
        public static bool IsStone(int protoID)
        {
            if (protoID >= 1 && protoID <= 12)
            {
                return true;
            }
            else if (protoID >= 19 && protoID <= 21)
            {
                return true;
            }
            else if (protoID >= 48 && protoID <= 59)
            {
                return true;
            }
            else if (protoID >= 66 && protoID <= 90)
            {
                return true;
            }
            else if (protoID >= 601 && protoID <= 605)
            {
                return true;
            }
            else if (protoID >= 611 && protoID <= 734)
            {
                return true;
            }
            else if (protoID >= 1041 && protoID <= 1044)
            {
                return true;
            }
            else if (protoID >= 1051 && protoID <= 1055)
            {
                return true;
            }
            else if (protoID >= 1061 && protoID <= 1066)
            {
                return true;
            }
            else if (protoID >= 1071 && protoID <= 1074)
            {
                return true;
            }
            return false;
        }


    }
}
