using CommonAPI.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            oldItem = LDB.items.Select(ProtoID.物品.蓄电器);
            oldRecipe = LDB.recipes.Select(ProtoID.配方.蓄电器);
            oldModel = LDB.models.Select(oldItem.ModelIndex);
            

            int ItemID = ProtoID.物品.能量核心;
            string IconPath = oldItem.IconPath;
            int Grid = ProtoID.GRID.能量核心;
            NewItem = ProtoRegistry.RegisterItem(ItemID, ProtoID.String.物品_能量核心, ProtoID.String.物品_能量核心_描述, IconPath, Grid, 50, EItemType.Logistics);


            int RecipeID = ProtoID.配方.能量核心;
            ERecipeType RecipeType = oldRecipe.Type;
            int TimeSpend = 300 * myConst.Second;
            int[] Input = new int[3] { ProtoID.物品.蓄电器, ProtoID.物品.钛合金, ProtoID.物品.奇异物质 };
            int[] InCount = new int[3] { 100, 200, 10 };
            int[] Output = new int[1] { NewItem.ID };
            int[] OutCount = new int[1] { 1 };
            int preTech = ProtoID.Tech.古代技术_星际电力传输;
            NewRecipe = ProtoRegistry.RegisterRecipe(RecipeID, RecipeType, TimeSpend, Input, InCount, Output, OutCount, ProtoID.String.物品_能量核心_描述, preTech, Grid, ProtoID.String.物品_能量核心, IconPath);


            int ModelID = ProtoID.ModelID.能量核心;
            string PrefabPath = oldModel.PrefabPath;
            int[] descFieids = oldItem.DescFields;
            int BuildID = ProtoID.BuildID.能量核心;
            NewModel = ProtoRegistry.RegisterModel(ModelID, NewItem, PrefabPath, null, descFieids, BuildID);


            ProtoRegistry.onLoadingFinished += new Action(Finished);


            
        }


        private static void Finished()
        {
            NewModel.prefabDesc.maxAcuEnergy = ItemConfig.PowerCoreCfg.PowerCoreMaxEnergy;//修改电量
            NewModel.prefabDesc.outputEnergyPerTick = 0;//修改功率
            NewModel.prefabDesc.inputEnergyPerTick = 0;
            NewItem.CanBuild = false;
            
            
            Util.Log(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, "原型添加完成");
        }








    }
    
    class ItemPowerCoreFull
    {
        private static ItemProto oldItem;
        private static ModelProto oldModel;
        private static ItemProto NewItem;
        private static ModelProto NewModel;
        private static RecipeProto NewRecipe;

        public static void AddItem()
        {
            oldItem = LDB.items.Select(ProtoID.物品.蓄电器满);
            oldModel = LDB.models.Select(oldItem.ModelIndex);

            int ItemID = ProtoID.物品.能量核心满;
            string IconPath = oldItem.IconPath;
            int Grid = ProtoID.GRID.能量核心满;
            NewItem = ProtoRegistry.RegisterItem(ItemID, ProtoID.String.物品_能量核心满, ProtoID.String.物品_能量核心满_描述, IconPath, Grid, 50, EItemType.Product);

            int RecipeID = ProtoID.配方.能量核心满;
            ERecipeType RecipeType = ERecipeType.Assemble;
            int TimeSpend = 300 * myConst.Second;
            int[] Input = new int[2] { ProtoID.物品.能量核心, ProtoID.物品.氘核燃料罐 };
            int[] InCount = new int[2] { 1, 250 };
            int[] Output = new int[1] { ProtoID.物品.能量核心满 };
            int[] OutCount = new int[1] { 1 };
            int preTech = ProtoID.Tech.古代技术_星际电力传输;
            NewRecipe = ProtoRegistry.RegisterRecipe(RecipeID, RecipeType, TimeSpend, Input, InCount, Output, OutCount, ProtoID.String.物品_能量核心满_描述, preTech, Grid, ProtoID.String.物品_能量核心满, IconPath);



            int ModelID = ProtoID.ModelID.能量核心满;
            string PrefabPath = oldModel.PrefabPath;
            int[] descFieids = oldItem.DescFields;
            int BuildID = ProtoID.BuildID.能量核心满;
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
            Util.Log(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, "原型添加完成");
        }







    }

}
