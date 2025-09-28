using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using CommonAPI;
using CommonAPI.Systems;
using System.Reflection;


namespace CustomCreateBirthStar
{
    class ItemPowerExchange
    {

        private static ItemProto oldItem;
        private static RecipeProto oldRecipe;
        private static ModelProto oldModel;
        private static ItemProto NewItem;
        private static RecipeProto NewRecipe;
        private static ModelProto NewModel;



        public static void AddItem()
        {
            oldItem = LDB.items.Select(ProtoID.物品.能量枢纽);
            oldRecipe = LDB.recipes.Select(ProtoID.配方.能量枢纽);
            oldModel = LDB.models.Select(oldItem.ModelIndex);

            
            int ItemID = ProtoID.物品.星际能量枢纽;
            string IconPath = oldItem.IconPath;
            int Grid = ProtoID.GRID.星际能量枢纽;
            NewItem = ProtoRegistry.RegisterItem(ItemID, ProtoID.String.物品_星际能量枢纽, ProtoID.String.物品_星际能量枢纽_描述, IconPath, Grid, 250, EItemType.Logistics);


            int RecipeID = ProtoID.配方.星际能量枢纽;
            ERecipeType RecipeType = oldRecipe.Type;
            int TimeSpend = 900 * myConst.Second;
            int[] Input = new int[5] { ProtoID.物品.能量枢纽, ProtoID.物品.湮灭约束球, ProtoID.物品.单极磁石, ProtoID.物品.量子芯片, ProtoID.物品.棱镜 };
            int[] InCount = new int[5] { 1, 100, 1500, 100, 200 };
            int[] Output = new int[1] { ProtoID.物品.星际能量枢纽 };
            int[] OutCount = new int[1] { 1 };
            int preTech = ProtoID.Tech.古代技术_星际电力传输;
            NewRecipe = ProtoRegistry.RegisterRecipe(RecipeID, RecipeType, TimeSpend, Input, InCount, Output, OutCount, ProtoID.String.物品_星际能量枢纽_描述, preTech, Grid, ProtoID.String.物品_星际能量枢纽, IconPath);


            int ModelID = ProtoID.ModelID.星际能量枢纽;
            string PrefabPath = oldModel.PrefabPath;
            int[] descFieids = oldItem.DescFields;
            int BuildID = ProtoID.BuildID.星际能量枢纽;
            NewModel = ProtoRegistry.RegisterModel(ModelID, NewItem, PrefabPath, null, descFieids, BuildID);
            

            ProtoRegistry.onLoadingFinished += new Action(Finished);


        }


        private static void Finished()
        {
            NewModel.prefabDesc.maxAcuEnergy = ItemConfig.PowerCoreCfg.PowerCoreMaxEnergy;//最大电量
            NewModel.prefabDesc.maxExcEnergy = ItemConfig.PowerCoreCfg.PowerCoreMaxEnergy;
            NewModel.prefabDesc.exchangeEnergyPerTick = (long)(ItemConfig.PowerCoreCfg.ExchangeEnergyPerTick);//输出功率
            NewModel.prefabDesc.fullId = ProtoID.物品.能量核心满;//燃料id
            NewModel.prefabDesc.emptyId = ProtoID.物品.能量核心;//燃料产物id
            
            // 给模型上色
            Material material = UnityEngine.Object.Instantiate<Material>(NewModel.prefabDesc.lodMaterials[0][0]);
            material.color = Color.red;
            NewModel.prefabDesc.lodMaterials[0][0] = material;

            
            NewModel.HpMax = 1500000;
            NewModel.HpRecover = 10000;
            NewItem.HpMax = 1500000;

            Util.Log(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, "原型添加完成");
        }

        




    }

    class ItemPowerExchangeMK2
    {

        private static ItemProto oldItem;
        private static RecipeProto oldRecipe;
        private static ModelProto oldModel;
        private static ItemProto NewItem;
        private static RecipeProto NewRecipe;
        private static ModelProto NewModel;



        public static void AddItem()
        {
            oldItem = LDB.items.Select(ProtoID.物品.能量枢纽);
            oldRecipe = LDB.recipes.Select(ProtoID.配方.能量枢纽);
            oldModel = LDB.models.Select(oldItem.ModelIndex);


            int ItemID = ProtoID.物品.星际能量枢纽MK2;
            string IconPath = oldItem.IconPath;
            int Grid = ProtoID.GRID.星际能量枢纽MK2;
            NewItem = ProtoRegistry.RegisterItem(ItemID, ProtoID.String.物品_星际能量枢纽MK2, ProtoID.String.物品_星际能量枢纽MK2_描述, IconPath, Grid, 250, EItemType.Logistics);


            int RecipeID = ProtoID.配方.星际能量枢纽MK2;
            ERecipeType RecipeType = oldRecipe.Type;
            int TimeSpend = 900 * myConst.Second;
            int[] Input = new int[5] { ProtoID.物品.星际能量枢纽, ProtoID.物品.卫星配电站, ProtoID.物品.引力透镜, ProtoID.物品.框架材料, ProtoID.物品.物流运输机 };
            int[] InCount = new int[5] { 1, 300, 80, 500, 300 };
            int[] Output = new int[1] { ProtoID.物品.星际能量枢纽MK2 };
            int[] OutCount = new int[1] { 1 };
            int preTech = ProtoID.Tech.古代技术_星际电力传输;
            NewRecipe = ProtoRegistry.RegisterRecipe(RecipeID, RecipeType, TimeSpend, Input, InCount, Output, OutCount, ProtoID.String.物品_星际能量枢纽MK2_描述, preTech, Grid, ProtoID.String.物品_星际能量枢纽MK2, IconPath);


            int ModelID = ProtoID.ModelID.星际能量枢纽MK2;
            string PrefabPath = oldModel.PrefabPath;
            int[] descFieids = oldItem.DescFields;
            int BuildID = ProtoID.BuildID.星际能量枢纽MK2;
            NewModel = ProtoRegistry.RegisterModel(ModelID, NewItem, PrefabPath, null, descFieids, BuildID);


            ProtoRegistry.onLoadingFinished += new Action(Finished);


        }


        private static void Finished()
        {
            NewModel.prefabDesc.maxAcuEnergy = ItemConfig.PowerCoreCfg.PowerCoreMaxEnergy;//最大电量
            NewModel.prefabDesc.maxExcEnergy = ItemConfig.PowerCoreCfg.PowerCoreMaxEnergy;
            NewModel.prefabDesc.exchangeEnergyPerTick = (long)(ItemConfig.PowerCoreCfg.ExchangeEnergyPerTick);//输出功率
            NewModel.prefabDesc.fullId = ProtoID.物品.能量核心满;//燃料id
            NewModel.prefabDesc.emptyId = ProtoID.物品.能量核心;//燃料产物id
            NewModel.prefabDesc.powerCoverRadius = 460f;//修改半径
            //NewModel.prefabDesc.powerConnectDistance = 80f;// 链接距离
            NewModel.prefabDesc.isPowerNode = true;

            // 没有效果
             //NewModel.prefabDesc.idleEnergyPerTick = 120000000L / 60;//修改待机功率
             //NewModel.prefabDesc.enemyIdleEnergy = 60000000 / 60;

            // 给模型上色
            Material material = UnityEngine.Object.Instantiate<Material>(NewModel.prefabDesc.lodMaterials[0][0]);
            material.color = new Color(255/255, 40 / 255, 40 / 255, 0.8f);
            NewModel.prefabDesc.lodMaterials[0][0] = material;
            
            
            NewModel.HpMax = 4500000;
            NewModel.HpRecover = 10000;
            NewItem.HpMax = 4500000;

            Util.Log(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, "原型添加完成");

        }






    }
}
