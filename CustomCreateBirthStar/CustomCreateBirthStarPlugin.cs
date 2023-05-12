using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using CommonAPI;
using CommonAPI.Systems;


namespace CustomCreateBirthStar

{
    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInProcess(GAME_PROCESS)]
    [BepInDependency(CommonAPIPlugin.GUID)]
    [CommonAPISubmoduleDependency(nameof(ProtoRegistry), nameof(CustomDescSystem))]
    class CustomCreateBirthStarPlugin : BaseUnityPlugin
    {
        public const string GUID = "kumor.plugin.CustomCreateBirthStar";
        public const string NAME = "DSP_Kumor_CustomCreateBirthStar";
        public const string VERSION = "1.2.0.0";
        public const string GAME_VERSION = "0.9.27.15033";
        public const string GAME_PROCESS = "DSPGAME.exe";
        public static ManualLogSource logger;
        ///<summary>
        ///是否在母星系附近生成黑洞，中子星，O星，蓝巨星
        ///</summary>
        public static bool StarTypePatch = true;
        ///<summary>
        ///是否在星系边缘生成一个超重黑洞
        ///</summary>
        public static bool NewSuperMassBlackHole = true;
        ///<summary>
        ///是否开启遗迹挖掘
        ///</summary>
        //public static bool Relics = true;

        private void Awake()
        {

            CustomCreateBirthStarPlugin.StarTypePatch = this.Config.Bind<bool>("CustomCreateBirthStar", "圣地星系群", true, new ConfigDescription("是否在母星系附近生成黑洞，中子星，O星，蓝巨星(圣地星系群，一个远古文明供奉的圣地，拥有丰富的资源，但它的主人却已不在，在遗留的数据里发现了*雾，纳*，天灾...)", (AcceptableValueBase)null, new object[0])).Value;
            CustomCreateBirthStarPlugin.NewSuperMassBlackHole = this.Config.Bind<bool>("SuperMassBlackHole", "超重黑洞星系群", true, new ConfigDescription("是否在星系边缘生成一个超重黑洞及其星系群，请存档并带够燃料再前往探索，祝你好运", (AcceptableValueBase)null, new object[0])).Value;
            //CustomCreateBirthStarPlugin.Relics = this.Config.Bind<bool>("Relics", "遗迹挖掘", true, new ConfigDescription("是否开启遗迹挖掘(需要ItemsManage模组)，在特定行星挖掘零散矿石有概率获得稀有物品", (AcceptableValueBase)null, new object[0])).Value;

            CustomCreateBirthStarPlugin.logger = this.Logger;
            Harmony harmony = new Harmony(GUID);
            Harmony.CreateAndPatchAll(typeof(CustomCreateBirthStarPatch), harmony.Id);
            Harmony.CreateAndPatchAll(typeof(GalaxyStarCountPatch), harmony.Id);

            ItemManage.AddString();
            ItemManage.AddTech0();
            ItemManage.AddTech1();

            ItemPlantPowerNet.AddItem();
            ItemPowerCore.AddItem();
            ItemPowerCoreFull.AddItem();
            ItemPowerExchange.AddItem();

            Harmony.CreateAndPatchAll(typeof(ItemPlantPowerNet), harmony.Id);
            Harmony.CreateAndPatchAll(typeof(ItemPowerCore), harmony.Id);
            Harmony.CreateAndPatchAll(typeof(ItemPowerCoreFull), harmony.Id);
            Harmony.CreateAndPatchAll(typeof(ItemPowerExchange), harmony.Id);

            Util.Log("初始化完毕");

        }
    }
}