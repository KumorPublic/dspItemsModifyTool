using CommonAPI.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCreateBirthStar
{
    class ItemPowerCore
    {
        private static ItemProto oldItem;
        private static RecipeProto oldRecipe;
        private static ModelProto oldModel;
        private static ItemProto NewItem;
        private static RecipeProto NewRecipe;
        private static ModelProto NewModel;





        public static void AddItem()
        {
            oldItem = LDB.items.Select(ItemsProtoID.蓄电器);
            oldRecipe = LDB.recipes.Select(ItemsProtoRecipeID.蓄电器);
            oldModel = LDB.models.Select(oldItem.ModelIndex);


            int ItemID = ItemsProtoID.能量核心;
            string IconPath = oldItem.IconPath;
            int Grid = ItemsProto.INDEX_GRID_POWERCORE;
            NewItem = ProtoRegistry.RegisterItem(ItemID, ProtoString.物品_能量核心, ProtoString.物品_能量核心_描述, IconPath, Grid, 50, EItemType.Logistics);


            int RecipeID = ItemsProtoRecipeID.能量核心;
            ERecipeType RecipeType = oldRecipe.Type;
            int TimeSpend = 300 * myConst.Second;
            int[] Input = new int[3] { ItemsProtoID.蓄电器, ItemsProtoID.钛合金, ItemsProtoID.奇异物质 };
            int[] InCount = new int[3] { 100, 200, 10 };
            int[] Output = new int[1] { NewItem.ID };
            int[] OutCount = new int[1] { 1 };
            int preTech = ItemsProtoTechID.古代技术_星际电力传输;
            NewRecipe = ProtoRegistry.RegisterRecipe(RecipeID, RecipeType, TimeSpend, Input, InCount, Output, OutCount, ProtoString.物品_能量核心_描述, preTech, Grid, ProtoString.物品_能量核心, IconPath);


            int ModelID = ItemsProto.INDEX_Model_POWERCORE;
            string PrefabPath = oldModel.PrefabPath;
            int[] descFieids = oldItem.DescFields;
            int BuildID = ItemsProto.INDEX_BUILD_POWERCORE;
            NewModel = ProtoRegistry.RegisterModel(ModelID, NewItem, PrefabPath, null, descFieids, BuildID);


            ProtoRegistry.onLoadingFinished += new Action(Finished);


        }


        private static void Finished()
        {
            NewModel.prefabDesc.maxAcuEnergy = ItemConfig.PowerCoreCfg.PowerCoreMaxEnergy;//修改电量
            NewModel.prefabDesc.outputEnergyPerTick = 0;//修改功率
            NewModel.prefabDesc.inputEnergyPerTick = 0;
            NewItem.CanBuild = false;
        }








    }

}
