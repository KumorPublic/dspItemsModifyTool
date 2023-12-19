using CommonAPI.Systems;
using CommonAPI.Systems.ModLocalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomCreateBirthStar
{
    class ItemManage
    {
        private static TechProto NewTechProto0;
        private static TechProto NewTechProto1;


        public static void AddString()
        {
            LocalizationModule.RegisterTranslation(ProtoID.String.物品_星际能量枢纽MK2,
                "Planetary Energy Hub MK2",
                "行星能量枢纽 MK2",
                "Planetary Energy Hub MK2");
            LocalizationModule.RegisterTranslation(ProtoID.String.物品_星际能量枢纽MK2_描述,
                "One of the technologies of ancient civilizations uses the planetary ionosphere and drone relay stations around the world to expand the range of wireless power transmission. Warning: There are still flaws in the reverse engineering technology. If the system is destroyed, it may trigger a planetary electromagnetic pulse.", 
                "古文明技术之一，利用行星电离层与遍布全球的无人机中继站扩大无线输电范围。警告：逆向技术尚存在缺陷，系统被摧毁时可能引发行星级电磁脉冲。",
                "One of the technologies of ancient civilizations uses the planetary ionosphere and drone relay stations around the world to expand the range of wireless power transmission. Warning: There are still flaws in the reverse engineering technology. If the system is destroyed, it may trigger a planetary electromagnetic pulse.");
            LocalizationModule.RegisterTranslation(ProtoID.String.物品_能量核心, 
                "PowerCore", 
                "能量核心",
                "PowerCore");
            LocalizationModule.RegisterTranslation(ProtoID.String.物品_能量核心_描述, 
                "One of the technologies of ancient civilizations, using the mass effect field produced by strange matter, can store a large amount of energy! Warning: There are still flaws in the reverse technology, please do not charge the mass field beyond the Chandrasekhar limit.", 
                "古文明技术之一，利用奇异物质产生的质量效应场，可以存储大量能量！警告：逆向技术尚存在缺陷，对质量场的充能请勿超过钱德拉塞卡极限。" ,
                "One of the technologies of ancient civilizations, using the mass effect field produced by strange matter, can store a large amount of energy! Warning: There are still flaws in the reverse technology, please do not charge the mass field beyond the Chandrasekhar limit.");
            LocalizationModule.RegisterTranslation(ProtoID.String.物品_能量核心满, 
                "PowerCore",
                "能量核心（满）",
                "PowerCore");
            LocalizationModule.RegisterTranslation(ProtoID.String.物品_能量核心满_描述, 
                "One of the technologies of ancient civilizations, using the mass effect field produced by strange matter, can store a large amount of energy! Warning: There are still flaws in the reverse technology, please do not charge the mass field beyond the Chandrasekhar limit.", 
                "古文明技术之一，利用奇异物质产生的质量效应场，可以存储大量能量！警告：逆向技术尚存在缺陷，对质量场的充能请勿超过钱德拉塞卡极限。",
                "One of the technologies of ancient civilizations, using the mass effect field produced by strange matter, can store a large amount of energy! Warning: There are still flaws in the reverse technology, please do not charge the mass field beyond the Chandrasekhar limit.");
            LocalizationModule.RegisterTranslation(ProtoID.String.物品_星际能量枢纽, "Planetary energy hub",
                "行星能量枢纽",
                "Planetary energy hub");
            LocalizationModule.RegisterTranslation(ProtoID.String.物品_星际能量枢纽_描述, 
                "One of the technologies of ancient civilization, it greatly improves the output power of the energy hub, but the cost is high. It has perfectly reversed the technology of ancient civilizations.", 
                "古文明技术之一，极大得提高了能量枢纽的输出功率，但造价高昂。已经完美逆向了古代文明的技术。",
                "One of the technologies of ancient civilization, it greatly improves the output power of the energy hub, but the cost is high. It has perfectly reversed the technology of ancient civilizations.");
            LocalizationModule.RegisterTranslation(ProtoID.String.科技_古文明_星际电力传输,
                "Ancient Civilization Technology - Planetary Power Transmission",
                "遗落科技 - 行星级电力传输",
                "Ancient Civilization Technology - Planetary Power Transmission");
            LocalizationModule.RegisterTranslation(ProtoID.String.科技_古文明_星际电力传输_描述,
                "By reverse engineering the legacy technology of ancient civilizations, we have obtained ancient but still advanced technology (it is said that caches of its prototype technology are scattered at the edge of the star sector closest to the black hole)", 
                "通过对古代文明遗留技术的逆向解析，我们获得了古老但仍然先进的技术（据说在星区边缘离黑洞最近的地方散落着它的原型技术缓存）",
                "By reverse engineering the legacy technology of ancient civilizations, we have obtained ancient but still advanced technology (it is said that caches of its prototype technology are scattered at the edge of the star sector closest to the black hole)");
            LocalizationModule.RegisterTranslation(ProtoID.String.科技_古文明_行星级无线输电,
                "Ancient Civilization Technology - Planetary Wireless Power Transmission",
                "遗落科技 - 行星级无线输电",
                "Ancient Civilization Technology - Planetary Wireless Power Transmission");
            LocalizationModule.RegisterTranslation(ProtoID.String.科技_古文明_行星级无线输电_描述,
                "By reverse engineering the legacy technology of ancient civilizations, we have obtained ancient but still advanced technology (it is said that caches of its prototype technology are scattered at the edge of the star sector closest to the black hole)",
                "通过对古代文明遗留技术的逆向解析，我们获得了古老但仍然先进的技术（据说在星区边缘离黑洞最近的地方散落着它的原型技术缓存）",
                "By reverse engineering the legacy technology of ancient civilizations, we have obtained ancient but still advanced technology (it is said that caches of its prototype technology are scattered at the edge of the star sector closest to the black hole)");

        }
        public static void AddTech0()
        {
            int id = ProtoID.Tech.古代技术_星际电力传输;
            string name = ProtoID.String.科技_古文明_星际电力传输;
            string description = ProtoID.String.科技_古文明_星际电力传输_描述;
            string conclusion = ProtoID.String.科技_古文明_星际电力传输;
            string iconPath = LDB.techs.Select(ProtoID.Tech.星际电力运输).IconPath;
            int[] preTechs = LDB.techs.Select(1).PreTechs;
            int[] costItems = new int[2] { ProtoID.物品.星际能量枢纽, ProtoID.物品.能量核心};
            long costHash = 54000;
            int[] costItemsPoints = new int[2] { (int)(15L * 3600L / costHash), (int)(120L * 3600L / costHash) };

            int[] unlockRecipes = new int[3] { ProtoID.配方.星际能量枢纽, ProtoID.配方.能量核心, ProtoID.配方.能量核心满 };
            Vector2 position = LDB.techs.Select(1).Position;
            position.y = position.y - 0;
            NewTechProto0 = ProtoRegistry.RegisterTech(id, name, description, conclusion, iconPath, preTechs, costItems, costItemsPoints, costHash, unlockRecipes, position);
            ProtoRegistry.onLoadingFinished += new Action(Finished0);
            
        }
        public static void Finished0()
        {
            // 封杀使用元数据购买
            IDCNT[] PropertyOverrideItemArray = new IDCNT[1];
            PropertyOverrideItemArray[0].id = 6006;
            PropertyOverrideItemArray[0].count = 500000000;

            LDB.techs.Select(ProtoID.Tech.古代技术_星际电力传输).PropertyItemCounts = new int[1] { 500000000 };
            LDB.techs.Select(ProtoID.Tech.古代技术_星际电力传输).PropertyOverrideItems = new int[1] { 6006 };
            LDB.techs.Select(ProtoID.Tech.古代技术_星际电力传输).PropertyOverrideItemArray = PropertyOverrideItemArray;
            //LDB.techs.Select(ItemsProtoTechID.古代技术_星际电力传输).unlockNeedItemArray = LDB.techs.Select(1001).unlockNeedItemArray;

            //Util.Log(LDB.techs.Select(1001).unlockNeedItemArray[0].count);
            //Util.Log(LDB.techs.Select(1001).unlockNeedItemArray[0].id);

            //Util.Log(LDB.techs.Select(1507).AddItemCounts[0]);
            //Util.Log(LDB.techs.Select(1507).AddItems[0]);
            //Util.Log(LDB.techs.Select(1507).PreTechsImplicit[0]);
            //Util.Log(LDB.techs.Select(1507).PropertyItemCounts[0]);
            //Util.Log(LDB.techs.Select(1507).PropertyOverrideItems[0]);
            //Util.Log(LDB.techs.Select(1507).PropertyItemCounts[1]);
            //Util.Log(LDB.techs.Select(1507).PropertyOverrideItems[1]);
            //Util.Log(LDB.techs.Select(1507).UnlockFunctions[0]);
            //Util.Log(String.Join(" ", LDB.techs.Select(1507).unlockNeedItemArray[0].id);
            //Util.Log(String.Join(" ", LDB.techs.Select(1507).UnlockValues[0]));

            Util.Log(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, "科技添加完成");
        }

        public static void AddTech1()
        {
            int id = ProtoID.Tech.古代技术_行星级无线输电;
            string name = ProtoID.String.科技_古文明_行星级无线输电;
            string description = ProtoID.String.科技_古文明_行星级无线输电_描述;
            string conclusion = ProtoID.String.科技_古文明_行星级无线输电;
            string iconPath = LDB.techs.Select(ProtoID.Tech.星际电力运输).IconPath;
            int[] preTechs = LDB.techs.Select(1).PreTechs;
            //int[] costItems = LDB.techs.Select(1001).Items;
            int[] costItems = new int[1] { ProtoID.物品.星际能量枢纽 };
            //int[] costItemsPoints = LDB.techs.Select(1001).ItemPoints;
            long costHash = 720000;
            int[] costItemsPoints = new int[1] { (int)(200L * 3600L / costHash) };
            
            int[] unlockRecipes = new int[1] { ProtoID.配方.星际能量枢纽MK2 };
            Vector2 position = LDB.techs.Select(1).Position;
            position.y = position.y - 4;
            NewTechProto1 = ProtoRegistry.RegisterTech(id, name, description, conclusion, iconPath, preTechs, costItems, costItemsPoints, costHash, unlockRecipes, position);
            ProtoRegistry.onLoadingFinished += new Action(Finished1);
        } 
        public static void Finished1()
        {
            IDCNT[] PropertyOverrideItemArray = new IDCNT[1];
            PropertyOverrideItemArray[0].id = 6006;
            PropertyOverrideItemArray[0].count = 500000000;

            LDB.techs.Select(ProtoID.Tech.古代技术_行星级无线输电).PropertyItemCounts = new int[1] { 500000000 };
            LDB.techs.Select(ProtoID.Tech.古代技术_行星级无线输电).PropertyOverrideItems = new int[1] { 6006 };
            LDB.techs.Select(ProtoID.Tech.古代技术_行星级无线输电).PropertyOverrideItemArray = PropertyOverrideItemArray;

            Util.Log(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, "科技添加完成");

        }



    }
}
