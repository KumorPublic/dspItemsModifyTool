using CommonAPI.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomCreateBirthStar
{
    ///<summary>
    ///行星电网
    ///</summary>
    class ItemPlantPowerNet
    {
        private static ItemProto oldItem;
        private static RecipeProto oldRecipe;
        private static ModelProto oldModel;

        private static ItemProto NewItem;
        private static RecipeProto NewRecipe;
        private static ModelProto NewModel;


        public static void AddItem()
        {
            oldItem = LDB.items.Select(ItemsProtoID.卫星配电站);
            oldRecipe = LDB.recipes.Select(ItemsProtoRecipeID.卫星配电站);
            oldModel = LDB.models.Select(oldItem.ModelIndex);


            int ItemID = ItemsProtoID.行星电网;
            string IconPath = oldItem.IconPath;
            int Grid = ItemsProto.INDEX_GRID_PlantPowerNet;
            NewItem = ProtoRegistry.RegisterItem(ItemID, ProtoString.物品_行星电网, ProtoString.物品_行星电网_描述, IconPath, Grid, 50, EItemType.Logistics);


            int RecipeID = ItemsProtoRecipeID.行星电网;
            ERecipeType RecipeType = oldRecipe.Type;
            int TimeSpend = 900 * myConst.Second;
            int[] Input = new int[5] { ItemsProtoID.卫星配电站, ItemsProtoID.引力透镜, ItemsProtoID.框架材料, ItemsProtoID.量子芯片, ItemsProtoID.物流运输机 };
            int[] InCount = new int[5] { 500, 50, 1500, 50, 50 };
            int[] Output = new int[1] { NewItem.ID };
            int[] OutCount = new int[1] { 5 };
            int preTech = ItemsProtoTechID.古代技术_星际电力传输;
            NewRecipe = ProtoRegistry.RegisterRecipe(RecipeID, RecipeType, TimeSpend, Input, InCount, Output, OutCount, ProtoString.物品_行星电网_描述, preTech, Grid, ProtoString.物品_行星电网, IconPath);


            int ModelID = ItemsProto.INDEX_Model_PlantPowerNet;
            string PrefabPath = oldModel.PrefabPath;
            int[] descFieids = oldItem.DescFields;
            int BuildID = ItemsProto.INDEX_BUILD_PlantPowerNet;
            NewModel = ProtoRegistry.RegisterModel(ModelID, NewItem, PrefabPath, null, descFieids, BuildID);


            ProtoRegistry.onLoadingFinished += new Action(Finished);

        }

        private static void Finished()
        {
            Material material = UnityEngine.Object.Instantiate<Material>(NewModel.prefabDesc.lodMaterials[0][0]);
            material.color = new Color(255f / 255f, 165f / 255f, 0f / 255f);
            NewModel.prefabDesc.lodMaterials[0][0] = material;

            NewModel.prefabDesc.powerCoverRadius = 500f;//修改半径
            NewModel.prefabDesc.idleEnergyPerTick *= 10L;//修改待机功率
        }


    }

}
