using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using CommonAPI;
using CommonAPI.Systems;


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
            oldItem = LDB.items.Select(ItemsProtoID.能量枢纽);
            oldRecipe = LDB.recipes.Select(ItemsProtoRecipeID.能量枢纽);
            oldModel = LDB.models.Select(oldItem.ModelIndex);


            int ItemID = ItemsProtoID.星际能量枢纽;
            string IconPath = oldItem.IconPath;
            int Grid = ItemsProto.INDEX_GRID_PowerExchange;
            NewItem = ProtoRegistry.RegisterItem(ItemID, ProtoString.物品_星际能量枢纽, ProtoString.物品_星际能量枢纽_描述, IconPath, Grid, 250, EItemType.Logistics);


            int RecipeID = ItemsProtoRecipeID.星际能量枢纽;
            ERecipeType RecipeType = oldRecipe.Type;
            int TimeSpend = 900 * myConst.Second;
            int[] Input = new int[5] { ItemsProtoID.能量枢纽, ItemsProtoID.湮灭约束球, ItemsProtoID.单极磁石, ItemsProtoID.量子芯片, ItemsProtoID.棱镜 };
            int[] InCount = new int[5] { 1, 100, 1500, 100, 200 };
            int[] Output = new int[1] { NewItem.ID };
            int[] OutCount = new int[1] { 1 };
            int preTech = ItemsProtoTechID.古代技术_星际电力传输;
            NewRecipe = ProtoRegistry.RegisterRecipe(RecipeID, RecipeType, TimeSpend, Input, InCount, Output, OutCount, ProtoString.物品_星际能量枢纽_描述, preTech, Grid, ProtoString.物品_星际能量枢纽, IconPath);


            int ModelID = ItemsProto.INDEX_Model_PowerExchange;
            string PrefabPath = oldModel.PrefabPath;
            int[] descFieids = oldItem.DescFields;
            int BuildID = ItemsProto.INDEX_BUILD_PowerExchange;
            NewModel = ProtoRegistry.RegisterModel(ModelID, NewItem, PrefabPath, null, descFieids, BuildID);


            ProtoRegistry.onLoadingFinished += new Action(Finished);


        }


        private static void Finished()
        {
            NewModel.prefabDesc.maxAcuEnergy = ItemConfig.PowerCoreCfg.PowerCoreMaxEnergy;//最大电量
            NewModel.prefabDesc.maxExcEnergy = ItemConfig.PowerCoreCfg.PowerCoreMaxEnergy;
            NewModel.prefabDesc.exchangeEnergyPerTick = (long)(ItemConfig.PowerCoreCfg.ExchangeEnergyPerTick);//输出功率
            NewModel.prefabDesc.fullId = ItemsProto.ID_ITEM_POWERCOREFULL;//燃料id
            NewModel.prefabDesc.emptyId = ItemsProto.ID_ITEM_POWERCORE;//燃料产物id

            Material material = UnityEngine.Object.Instantiate<Material>(NewModel.prefabDesc.lodMaterials[0][0]);
            material.color = Color.red;
            NewModel.prefabDesc.lodMaterials[0][0] = material;
        }

        




    }
}
