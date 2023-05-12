using CommonAPI.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
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
            ProtoRegistry.RegisterString(ProtoString.物品_行星电网, "PlantPowerNet", "行星电网");
            ProtoRegistry.RegisterString(ProtoString.物品_行星电网_描述, "One of the ancient civilization technologies, using the planetary ionosphere and UAV relay stations all over the world to expand the range of wireless power transmission", "古文明技术之一，利用行星电离层与遍布全球的无人机中继站扩大无线输电范围。警告：逆向技术尚存在缺陷，电网过载时可能引发行星级电磁脉冲。");
            ProtoRegistry.RegisterString(ProtoString.物品_能量核心, "PowerCore", "能量核心");
            ProtoRegistry.RegisterString(ProtoString.物品_能量核心_描述, "One of the technologies of ancient civilizations, using the mass effect field produced by strange matter, can store a large amount of energy! Warning: There are still flaws in the reverse technology, please do not charge the mass field beyond the Chandrasekhar limit.", "古文明技术之一，利用奇异物质产生的质量效应场，可以存储大量能量！警告：逆向技术尚存在缺陷，对质量场的充能请勿超过钱德拉塞卡极限。");
            ProtoRegistry.RegisterString(ProtoString.物品_能量核心满, "PowerCore", "能量核心（满）");
            ProtoRegistry.RegisterString(ProtoString.物品_能量核心满_描述, "One of the technologies of ancient civilizations, using the mass effect field produced by strange matter, can store a large amount of energy! Warning: There are still flaws in the reverse technology, please do not charge the mass field beyond the Chandrasekhar limit.", "古文明技术之一，利用奇异物质产生的质量效应场，可以存储大量能量！警告：逆向技术尚存在缺陷，对质量场的充能请勿超过钱德拉塞卡极限。");
            ProtoRegistry.RegisterString(ProtoString.物品_星际能量枢纽, "Interstellar Energy Hub ", "星际能量枢纽");
            ProtoRegistry.RegisterString(ProtoString.物品_星际能量枢纽_描述, "One of the technologies of ancient civilization, it greatly improves the output power of the energy hub, but the cost is high. It has perfectly reversed the technology of ancient civilizations.", "古文明技术之一，极大得提高了能量枢纽的输出功率，但造价高昂。已经完美逆向了古代文明的技术。");
            ProtoRegistry.RegisterString(ProtoString.科技_古文明_星际电力传输, "Ancient Technology - Interstellar Power Transmission", "古文明技术 - 星系级电力传输");
            ProtoRegistry.RegisterString(ProtoString.科技_古文明_星际电力传输_描述, "Ancient Technology - Interstellar Power Transmission", "通过对古代文明遗留技术的逆向解析，我们获得了......");
            ProtoRegistry.RegisterString(ProtoString.科技_古文明_行星级无线输电, "Ancient Technology - Planetary Wireless Power Transmission", "古文明技术 - 行星级无线输电");
            ProtoRegistry.RegisterString(ProtoString.科技_古文明_行星级无线输电_描述, "Ancient Technology - Planetary Wireless Power Transmission", "通过对古代文明遗留技术的逆向解析，我们获得了......");
            
        }
        public static void AddTech0()
        {
            int id = ItemsProtoTechID.古代技术_星际电力传输;
            string name = ProtoString.科技_古文明_星际电力传输;
            string description = ProtoString.科技_古文明_星际电力传输_描述;
            string conclusion = ProtoString.科技_古文明_星际电力传输;
            string iconPath = LDB.techs.Select(1).IconPath;
            int[] preTechs = LDB.techs.Select(1).PreTechs;
            int[] costItems = new int[2] { ItemsProtoID.星际能量枢纽, ItemsProtoID.能量核心};
            int[] costItemsPoints = new int[2] { 1, 1 };
            long costHash = 40000;
            int[] unlockRecipes = new int[3] { ItemsProtoRecipeID.星际能量枢纽, ItemsProtoRecipeID.能量核心, ItemsProtoRecipeID.能量核心满 };
            Vector2 position = LDB.techs.Select(1).Position;
            position.y = position.y - 0;
            NewTechProto0 = ProtoRegistry.RegisterTech(id, name, description, conclusion, iconPath, preTechs, costItems, costItemsPoints, costHash, unlockRecipes, position);
            ProtoRegistry.onLoadingFinished += new Action(Finished0);
            
        }
        public static void Finished0()
        {
            IDCNT[] PropertyOverrideItemArray = new IDCNT[1];
            PropertyOverrideItemArray[0].id = 6006;
            PropertyOverrideItemArray[0].count = 500000000;

            LDB.techs.Select(ItemsProtoTechID.古代技术_星际电力传输).PropertyItemCounts = new int[1] { 500000000 };
            LDB.techs.Select(ItemsProtoTechID.古代技术_星际电力传输).PropertyOverrideItems = new int[1] { 6006 };
            LDB.techs.Select(ItemsProtoTechID.古代技术_星际电力传输).PropertyOverrideItemArray = PropertyOverrideItemArray;
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
        }

        public static void AddTech1()
        {
            int id = ItemsProtoTechID.古代技术_行星级无线输电;
            string name = ProtoString.科技_古文明_行星级无线输电;
            string description = ProtoString.科技_古文明_行星级无线输电_描述;
            string conclusion = ProtoString.科技_古文明_行星级无线输电;
            string iconPath = LDB.techs.Select(1).IconPath;
            int[] preTechs = LDB.techs.Select(1).PreTechs;
            //int[] costItems = LDB.techs.Select(1001).Items;
            int[] costItems = new int[1] { ItemsProtoID.行星电网 };
            //int[] costItemsPoints = LDB.techs.Select(1001).ItemPoints;
            int[] costItemsPoints = new int[1] { 1 };
            long costHash = 40000;
            int[] unlockRecipes = new int[1] { ItemsProtoRecipeID.行星电网 };
            Vector2 position = LDB.techs.Select(1).Position;
            position.y = position.y - 4;
            NewTechProto1 = ProtoRegistry.RegisterTech(id, name, description, conclusion, iconPath, preTechs, costItems, costItemsPoints, costHash, unlockRecipes, position);
            ProtoRegistry.onLoadingFinished += new Action(Finished1);
        } 
        public static void Finished1()
        {
            IDCNT[] PropertyOverrideItemArray = new IDCNT[1];
            PropertyOverrideItemArray[0].id = 6006;
            PropertyOverrideItemArray[0].count = 300000000;

            LDB.techs.Select(ItemsProtoTechID.古代技术_行星级无线输电).PropertyItemCounts = new int[1] { 300000000 };
            LDB.techs.Select(ItemsProtoTechID.古代技术_行星级无线输电).PropertyOverrideItems = new int[1] { 6006 };
            LDB.techs.Select(ItemsProtoTechID.古代技术_行星级无线输电).PropertyOverrideItemArray = PropertyOverrideItemArray;
        }



    }
}
