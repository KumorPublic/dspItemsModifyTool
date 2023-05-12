using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using xiaoye97;
using BepInEx;


namespace ItemsModifyTool
{
    class ItemPenroseBall : BaseUnityPlugin
    {
        public static Sprite iconPenroseBall;

        public static void AddItemPenroseBall()
        {
            iconPenroseBall = LDB.items.Select(2210).iconSprite;

            ItemProto oldBall = LDB.items.Select(2210);
            ItemProto PenroseBall = oldBall.Copy();
            RecipeProto PenroseBallRecipe = LDB.recipes.Select(43).Copy();

            PenroseBallRecipe.ID = ItemsProto.ID_RECIPE_PenroseBall;
            PenroseBallRecipe.Name = "人造黑洞";
            PenroseBallRecipe.name = "人造黑洞".Translate();
            PenroseBallRecipe.Description = "人造黑洞描述";
            PenroseBallRecipe.description = "人造黑洞描述".Translate();

            PenroseBallRecipe.Items = new int[] { 2210, 1125, 1127, 1501, 6006 };
            PenroseBallRecipe.ItemCounts = new int[] { 1, 50, 50, 50, 50 };
            PenroseBallRecipe.Results = new int[] { ItemsProto.ID_ITEM_PenroseBall };

            PenroseBallRecipe.GridIndex = ItemsProto.INDEX_GRID_PenroseBall;
            PenroseBallRecipe.preTech = LDB.techs.Select(1508);
            PenroseBallRecipe.SID = PenroseBallRecipe.GridIndex.ToString();
            PenroseBallRecipe.sid = PenroseBallRecipe.GridIndex.ToString();

            PenroseBall.ID = ItemsProto.ID_ITEM_PenroseBall;
            PenroseBall.Name = "人造黑洞";
            PenroseBall.name = "人造黑洞".Translate();
            PenroseBall.Description = "人造黑洞描述";
            PenroseBall.description = "人造黑洞描述".Translate();
            PenroseBall.BuildIndex = ItemsProto.INDEX_BUILD_PenroseBall;
            PenroseBall.GridIndex = ItemsProto.INDEX_GRID_PenroseBall;

            PenroseBall.handcraft = PenroseBallRecipe;
            PenroseBall.maincraft = PenroseBallRecipe;
            PenroseBall.handcrafts = new List<RecipeProto>() { PenroseBallRecipe };
            PenroseBall.recipes = new List<RecipeProto>() { PenroseBallRecipe };
            PenroseBall.makes = new List<RecipeProto>() { };

            PenroseBall.prefabDesc = oldBall.prefabDesc.Copy();
            PenroseBall.prefabDesc.modelIndex = PenroseBall.ModelIndex;


            PenroseBall.prefabDesc.isCollectStation = false;
            PenroseBall.prefabDesc.stationCollectSpeed = 0;

            Traverse.Create(PenroseBall).Field("_iconSprite").SetValue(iconPenroseBall);
            Traverse.Create(PenroseBallRecipe).Field("_iconSprite").SetValue(iconPenroseBall);

            
            //改燃料类型
            List<int[]> fuelNeedCopy = new List<int[]>();
            foreach (int[] line in ItemProto.fuelNeeds)
            {
                fuelNeedCopy.Add(line);
            }

            List<int> addFuel = new List<int>();
            foreach (int id in ItemProto.itemIds)
            {
                addFuel.Add(id);
            }
            fuelNeedCopy.Add(addFuel.ToArray());
            ItemProto.fuelNeeds = fuelNeedCopy.ToArray();
            PenroseBall.prefabDesc.fuelMask = ItemProto.fuelNeeds.Length - 1;

            //改为用电设备
            PenroseBall.prefabDesc.isPowerCharger = true;
            PenroseBall.prefabDesc.workEnergyPerTick = 350000 * 5000;
            PenroseBall.prefabDesc.idleEnergyPerTick = 350000 * 5000;
            PenroseBall.prefabDesc.genEnergyPerTick = (long)350000 * (long)10000;
            PenroseBall.prefabDesc.useFuelPerTick *= 1000;

            foreach (var mat in PenroseBall.prefabDesc.materials)
            {
                mat.color = new Color(34f / 255f, 217f / 255f, 188f / 255f);
            }

            LDBTool.PostAddProto(ProtoType.Item, PenroseBall);
            LDBTool.PostAddProto(ProtoType.Recipe, PenroseBallRecipe);

            try
            {
                LDBTool.SetBuildBar(8, 5, PenroseBall.ID);
            }
            catch
            {

            }
        }

    }
}
