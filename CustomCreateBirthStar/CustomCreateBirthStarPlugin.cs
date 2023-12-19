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
        public const string NAME = "夺取回我们的遗产";
        public const string VERSION = "1.3.0.0";
        public const string GAME_VERSION = "0.10.28.20856";
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
        public static bool Relics = false;
        ///<summary>
        ///是否开启黑雾科技
        ///</summary>
        public static bool NanoTech = false;
        ///<summary>
        ///是否开启调试
        ///</summary>
        public static bool Debug = false;
        private void Awake()
        {
            CustomCreateBirthStarPlugin.StarTypePatch = this.Config.Bind<bool>("星区参数", "圣地星系群", true, new ConfigDescription("是否在母星系附近生成黑洞，中子星，O星，蓝巨星(圣地星系群，一个远古文明供奉的圣地，拥有丰富的资源，但它的主人却已不在，在遗留的数据里发现了*雾，纳*，天灾...)", (AcceptableValueBase)null, new object[0])).Value;
            CustomCreateBirthStarPlugin.NewSuperMassBlackHole = this.Config.Bind<bool>("星区参数", "超重黑洞星系群", true, new ConfigDescription("是否在星系边缘生成一个超重黑洞及其星系群，请存档并带够燃料再前往探索，祝你好运", (AcceptableValueBase)null, new object[0])).Value;
            CustomCreateBirthStarPlugin.Relics = this.Config.Bind<bool>("科技参数", "遗落科技", true, new ConfigDescription("是否开启遗落科技(会增加几个额外建筑，科技，如果不喜欢可关闭)，在特定行星挖掘零散矿石有概率获得稀有物品", (AcceptableValueBase)null, new object[0])).Value;
            CustomCreateBirthStarPlugin.NanoTech = this.Config.Bind<bool>("科技参数", "黑雾科技", true, new ConfigDescription("是否开启黑雾科技", (AcceptableValueBase)null, new object[0])).Value;
            
            CustomCreateBirthStarPlugin.Debug = this.Config.Bind<bool>("Debug", "Debug", false, new ConfigDescription("调试模式", (AcceptableValueBase)null, new object[0])).Value;

            CustomCreateBirthStarPlugin.logger = this.Logger;
            Harmony harmony = new Harmony(GUID);
            Harmony.CreateAndPatchAll(typeof(CustomCreateBirthStarPatch), harmony.Id);
            Harmony.CreateAndPatchAll(typeof(GalaxyStarCountPatch), harmony.Id);


            if (CustomCreateBirthStarPlugin.Relics)
            {
                ItemManage.AddString();
                ItemManage.AddTech0();
                ItemManage.AddTech1();

                ItemPowerCore.AddItem();
                ItemPowerCoreFull.AddItem();
                ItemPowerExchange.AddItem();
                ItemPowerExchangeMK2.AddItem();

                Harmony.CreateAndPatchAll(typeof(ItemPowerCore), harmony.Id);
                Harmony.CreateAndPatchAll(typeof(ItemPowerCoreFull), harmony.Id);
                Harmony.CreateAndPatchAll(typeof(ItemPowerExchange), harmony.Id);
                Harmony.CreateAndPatchAll(typeof(ItemPowerExchangeMK2), harmony.Id);
                
            }
            if (CustomCreateBirthStarPlugin.NanoTech)
            {
                ItemStationSolt.Create();
                Harmony.CreateAndPatchAll(typeof(ItemStationSolt), harmony.Id);
            }




            Util.Log("初始化完毕");

        }




    }
}