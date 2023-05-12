using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using xiaoye97;


namespace ItemsModifyTool
{
    class ItemPowerCore
    {
        ///<summary>
        ///能量核心最大能量
        ///</summary>
        public static ConfigEntry<long> PowerCoreMaxEnergy;
        ///<summary>
        ///能量枢纽最大功率
        ///</summary>
        public static ConfigEntry<long> exchangeEnergyPerTick;
        ///<summary>
        ///蓄电器最大能量
        ///</summary>
        public static ConfigEntry<long> PowerAccumulatorMaxEnergy;
        ///<summary>
        ///蓄电器输出功率
        ///</summary>
        public static ConfigEntry<long> PowerAccumulatorEnergyPerTick;


       
        public static void AddItemPowerCore()
        {
            Sprite iconPowerCore;
            Sprite iconPowerCoreFull;

            iconPowerCore = LDB.items.Select(2206).iconSprite;//获取电瓶的图标
            iconPowerCoreFull = LDB.items.Select(2207).iconSprite;//获取电瓶的图标


            //满的
            ItemProto ItemProtoPowerCore00 = LDB.items.Select(2207);
            ItemProto ItemProtoPowerCore10 = ItemProtoPowerCore00.Copy<ItemProto>();
            ItemProtoPowerCore10.ID = ItemsProto.ID_ITEM_POWERCORE10;
            ItemProtoPowerCore10.Name = "能量核心(满)";
            ItemProtoPowerCore10.name = "能量核心(满)".Translate();
            ItemProtoPowerCore10.Description = "能量核心描述";
            ItemProtoPowerCore10.description = "能量核心描述".Translate();
            ItemProtoPowerCore10.BuildIndex = ItemsProto.INDEX_BUILD_POWERCORE10;
            ItemProtoPowerCore10.GridIndex = ItemsProto.INDEX_GRID_POWERCORE10;
            ItemProtoPowerCore10.makes = new List<RecipeProto>();
            ItemProtoPowerCore10.prefabDesc = ItemProtoPowerCore00.prefabDesc.Copy<PrefabDesc>();
            ItemProtoPowerCore10.prefabDesc.modelIndex = ItemProtoPowerCore10.ModelIndex;
            ItemProtoPowerCore10.prefabDesc.maxAcuEnergy = ItemPowerCore.PowerCoreMaxEnergy.Value;
            ItemProtoPowerCore10.HeatValue = ItemPowerCore.PowerCoreMaxEnergy.Value;
            ItemProtoPowerCore10.prefabDesc.outputEnergyPerTick = 0;
            ItemProtoPowerCore10.prefabDesc.inputEnergyPerTick = 0;
            ItemProtoPowerCore10.CanBuild = false;
            Traverse.Create((object)ItemProtoPowerCore10).Field("_iconSprite").SetValue((object)iconPowerCoreFull);
            LDBTool.PostAddProto(ProtoType.Item, (Proto)ItemProtoPowerCore10);
        }

    }
}

