using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using BepInEx.Logging;
using System;
using xiaoye97;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

namespace ItemsModifyTool
{

    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInProcess(GAME_PROCESS)]
    
    class ItemsModifyToolPlugin : BaseUnityPlugin
    {
        public const string GUID = "kumor.plugin.ItemsModifyTool";
        public const string NAME = "DSP_Kumor_ItemsModifyTool";
        public const string VERSION = "1.0.1.0";
        public const string GAME_VERSION = "0.8.22.9331";
        public const string GAME_PROCESS = "DSPGAME.exe";
        public static ManualLogSource logger;
        private static Harmony harmony;


        private void Awake()
        {
            ItemsModifyToolPlugin.logger = this.Logger;

            ItemsModifyToolPlugin.harmony = new Harmony(GUID);



            //this.ConfigItemPowerCore();
            
            //ItemsModifyToolPlugin.harmony.PatchAll(typeof(ItemPowerCore));
            //LDBTool.PreAddDataAction += new Action(ItemPowerCore.AddItemPowerCore);
            //LDBTool.PreAddDataAction += new Action(this.AddLanguage);
        }

        private void Start()
        {

        }
            
        private void Update()
        {
            
        }

       


        private void AddLanguage()
        {
            StringProto stringproto1 = new StringProto();
            StringProto stringproto2 = new StringProto();
            StringProto stringproto3 = new StringProto();

            stringproto1.ID = 10547;
            stringproto1.Name = "能量核心";
            stringproto1.name = "能量核心";
            stringproto1.ZHCN = "能量核心";
            stringproto1.ENUS = "";
            stringproto1.FRFR = "";
            stringproto2.ID = 10548;
            stringproto2.Name = "能量核心描述";
            stringproto2.name = "能量核心描述";
            stringproto2.ZHCN = "把电瓶做大就没人能偷走！";
            stringproto2.ENUS = "";
            stringproto2.FRFR = "";
            stringproto3.ID = 10549;
            stringproto3.Name = "能量核心(满)";
            stringproto3.name = "能量核心(满)";
            stringproto3.ZHCN = "能量核心(满)";
            stringproto3.ENUS = "";
            stringproto3.FRFR = "";


            LDBTool.PreAddProto(ProtoType.String, (Proto)stringproto1);
            LDBTool.PreAddProto(ProtoType.String, (Proto)stringproto2);
            LDBTool.PreAddProto(ProtoType.String, (Proto)stringproto3);


            StringProto PenroseBallName = new StringProto();
            StringProto PenroseBallDesc = new StringProto();
            PenroseBallName.ID = 10012;
            PenroseBallName.Name = "人造黑洞";
            PenroseBallName.name = "人造黑洞";
            PenroseBallName.ZHCN = "人造黑洞";
            PenroseBallName.ENUS = "Artificial black holes";
            PenroseBallName.FRFR = "Artificial black holes";

            PenroseBallDesc.ID = 10013;
            PenroseBallDesc.Name = "人造黑洞描述";
            PenroseBallDesc.name = "人造黑洞描述";
            PenroseBallDesc.ZHCN = "建立于人造黑洞上的彭罗斯球体，利用黑洞能层加速电磁波，使得电磁波强度呈指数倍增长，是通过超辐射散射收集能量的装置，不过小心，它可能会爆炸！(注意时刻向黑洞内投送物质维持其稳定，好在它不挑食)";
            PenroseBallDesc.ENUS = "The Penrose sphere, built on a man-made black hole, uses the black hole's ergosphere to accelerate electromagnetic waves, making them exponentially stronger, a device that collects energy through hyperradiation scattering, but watch out, it could explode! (Pay attention to throwing matter into the black hole at all times to keep it stable, but it's not a picky eater.).";
            PenroseBallDesc.FRFR = "The Penrose sphere, built on a man-made black hole, uses the black hole's ergosphere to accelerate electromagnetic waves, making them exponentially stronger, a device that collects energy through hyperradiation scattering, but watch out, it could explode! (Pay attention to throwing matter into the black hole at all times to keep it stable, but it's not a picky eater.).";

            LDBTool.PreAddProto(ProtoType.String, PenroseBallName);
            LDBTool.PreAddProto(ProtoType.String, PenroseBallDesc);







        }

    }


    
}
