using CommonAPI.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCreateBirthStar
{
    class ItemPowerCoreFull
    {
        private static ItemProto oldItem;
        private static ModelProto oldModel;
        private static ItemProto NewItem;
        private static ModelProto NewModel;
        private static RecipeProto NewRecipe;

        public static void AddItem()
        {
            oldItem = LDB.items.Select(ItemsProtoID.蓄电器满);
            oldModel = LDB.models.Select(oldItem.ModelIndex);

            int ItemID = ItemsProtoID.能量核心满;
            string IconPath = oldItem.IconPath;
            int Grid = ItemsProto.INDEX_GRID_POWERCOREFULL;
            NewItem = ProtoRegistry.RegisterItem(ItemID, ProtoString.物品_能量核心满, ProtoString.物品_能量核心满_描述, IconPath, Grid, 50, EItemType.Product);

            int RecipeID = ItemsProtoRecipeID.能量核心满;
            ERecipeType RecipeType = ERecipeType.Assemble;
            int TimeSpend = 300 * myConst.Second;
            int[] Input = new int[2] { ItemsProtoID.能量核心, ItemsProtoID.氘核燃料罐 };
            int[] InCount = new int[2] { 1, 250 };
            int[] Output = new int[1] { ItemsProtoID.能量核心满 };
            int[] OutCount = new int[1] { 1 };
            int preTech = ItemsProtoTechID.古代技术_星际电力传输;
            NewRecipe = ProtoRegistry.RegisterRecipe(RecipeID, RecipeType, TimeSpend, Input, InCount, Output, OutCount, ProtoString.物品_能量核心满_描述, preTech, Grid, ProtoString.物品_能量核心满, IconPath);



            int ModelID = ItemsProto.INDEX_Model_POWERCOREFULL;
            string PrefabPath = oldModel.PrefabPath;
            int[] descFieids = oldItem.DescFields;
            int BuildID = ItemsProto.INDEX_BUILD_POWERCOREFULL;
            NewModel = ProtoRegistry.RegisterModel(ModelID, NewItem, PrefabPath, null, descFieids, BuildID);


            ProtoRegistry.onLoadingFinished += new Action(Finished);


        }


        private static void Finished()
        {
            NewModel.prefabDesc.maxAcuEnergy = ItemConfig.PowerCoreCfg.PowerCoreMaxEnergy;//修改最大电量
            NewItem.HeatValue = ItemConfig.PowerCoreCfg.PowerCoreMaxEnergy;//修改当前电量
            NewModel.prefabDesc.outputEnergyPerTick = 0;//修改功率
            NewModel.prefabDesc.inputEnergyPerTick = 0;
            NewItem.CanBuild = false;
        }







    }

}
