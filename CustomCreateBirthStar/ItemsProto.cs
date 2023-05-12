using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCreateBirthStar
{
    class myConst
    {
        public const int Second = 60;
    }
    class ItemsProto
    {

        public const int ID_RECIPE_POWERCORE = 452;
        public const int ID_RECIPE_Collector = 453;
        public const int ID_RECIPE_POWERCOREFULL = 454;
        public const int ID_RECIPE_PlantPowerNet = 455;
        public const int ID_RECIPE_PowerExchange = 456;

        public const int ID_ITEM_POWERCORE = 9446;
        public const int ID_ITEM_POWERCOREFULL = 9447;
        public const int ID_ITEM_Collector = 9448;
        public const int ID_ITEM_PlantPowerNet = 9450;
        public const int ID_ITEM_PowerExchange = 9449;

        public const int INDEX_GRID_POWERCOREFULL = 2712;
        public const int INDEX_GRID_POWERCORE = 2711;
        public const int INDEX_GRID_PlantPowerNet = 2710;
        public const int INDEX_GRID_PowerExchange = 2709;
        public const int INDEX_GRID_Collector = 2704;

        public const int INDEX_BUILD_POWERCORE = 0;
        public const int INDEX_BUILD_POWERCOREFULL = 0;
        public const int INDEX_BUILD_Collector = 210;
        public const int INDEX_BUILD_PowerExchange = 709;
        public const int INDEX_BUILD_PlantPowerNet = 710;


        public const int INDEX_Model_POWERCORE = 280;
        public const int INDEX_Model_POWERCOREFULL = 281;
        public const int INDEX_Model_Collector = 283;
        public const int INDEX_Model_PlantPowerNet = 284;
        public const int INDEX_Model_PowerExchange = 282;

    }

    class ItemsProtoID
    {
        public const int 能量核心 = ItemsProto.ID_ITEM_POWERCORE;
        public const int 能量核心满 = ItemsProto.ID_ITEM_POWERCOREFULL;
        public const int 行星电网 = ItemsProto.ID_ITEM_PlantPowerNet;
        public const int 星际能量枢纽 = ItemsProto.ID_ITEM_PowerExchange;
        public const int 轨道开采站 = ItemsProto.ID_ITEM_Collector;

        public const int 煤 = 1006;
        public const int 原油 = 1007;
        public const int 单极磁石 = 1016;

        public const int 钛合金 = 1107;
        public const int 棱镜 = 1111;
        public const int 框架材料 = 1125;
        public const int 奇异物质 = 1127;

        public const int 引力透镜 = 1209;
        public const int 空间翘曲器 = 1210;

        public const int 量子芯片 = 1305;

        public const int 湮灭约束球 = 1403;

        public const int 氘核燃料罐 = 1802;

        public const int 星际物流运输站 = 2104;
        public const int 轨道采集器 = 2105;

        public const int 蓄电器 = 2206;
        public const int 蓄电器满 = 2207;
        public const int 能量枢纽 = 2209;
        public const int 卫星配电站 = 2212;

        public const int 抽水站 = 2306;
        public const int 原油萃取站 = 2307;
        public const int 大型采矿机 = 2316;

        public const int 物流运输机 = 5001;



    }

    class ItemsProtoTechID
    {
        public const int 光子聚束采矿科技 = 1304;
        public const int 行星电离层应用 = 1505;
        public const int 星际电力运输 = 1512;
        public const int 古代技术_星际电力传输 = 8000;
        public const int 古代技术_行星级无线输电 = 8001;
        
    }

    class ItemsProtoRecipeID
    {
        public const int 能量核心 = ItemsProto.ID_RECIPE_POWERCORE;
        public const int 能量核心满 = ItemsProto.ID_RECIPE_POWERCOREFULL;
        public const int 轨道开采站 = ItemsProto.ID_RECIPE_Collector;
        public const int 行星电网 = ItemsProto.ID_RECIPE_PlantPowerNet;
        public const int 星际能量枢纽 = ItemsProto.ID_RECIPE_PowerExchange;

        public const int 卫星配电站 = 73;
        public const int 蓄电器 = 76;
        public const int 能量枢纽 = 77;
        public const int 星际物流运输站 = 95;

    }

    class ProtoString
    {
        public const string 科技_古文明_星际电力传输 = "T000001";
        public const string 科技_古文明_星际电力传输_描述 = "T000002";
        public const string 科技_古文明_行星级无线输电 = "T000003";
        public const string 科技_古文明_行星级无线输电_描述 = "T000004";

        

        public const string 物品_行星电网 = "T000101";
        public const string 物品_行星电网_描述 = "T000102";
        public const string 物品_能量核心 = "T000201";
        public const string 物品_能量核心_描述 = "T000202";
        public const string 物品_能量核心满 = "T000301";
        public const string 物品_能量核心满_描述 = "T000302";
        public const string 物品_星际能量枢纽 = "T000401";
        public const string 物品_星际能量枢纽_描述 = "T000402";
    }

}
