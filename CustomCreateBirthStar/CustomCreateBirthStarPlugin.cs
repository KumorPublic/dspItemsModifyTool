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
        public const string VERSION = "1.3.1.0";
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
        public static ConfigEntry<bool> Debug;
        ///<summary>
        ///资源倍率
        ///</summary>
        public static ConfigEntry<float> resourceCoef;
        ///<summary>
        ///守卫强度
        ///</summary>
        public static ConfigEntry<int> DarkFogLv;
        ///<summary>
        ///初始星系id
        ///</summary>
        public static int BirthStarId = -1;
        ///<summary>
        ///初始行星id
        ///</summary>
        public static int BirthPlanetId = -1;
        ///<summary>
        ///是否出生在圣地星系群
        ///</summary>
        public static ConfigEntry<bool> BirthStarAtOStar;
        ///<summary>
        ///是否出生在黑洞星系群
        ///</summary>
        public static ConfigEntry<bool> BirthStarAtSuperMassBlackHole;
        ///<summary>
        ///更多星球
        ///</summary>
        public static bool MorePlanet = false;


        private void Awake()
        {
            string 星区参数 = "星区参数(不兼容旧存档|重启生效)";
            CustomCreateBirthStarPlugin.StarTypePatch = this.Config.Bind<bool>(星区参数, "圣地星系群", true, new ConfigDescription("是否在母星系附近生成黑洞，中子星，O星，蓝巨星(圣地星系群，一个远古文明供奉的圣地，拥有丰富的资源，但它的主人却已不在，在遗留的数据里发现了*雾，纳*，天灾...)", (AcceptableValueBase)null, new object[0])).Value;
            CustomCreateBirthStarPlugin.NewSuperMassBlackHole = this.Config.Bind<bool>(星区参数, "超重黑洞星系群", true, new ConfigDescription("是否在星系边缘生成一个超重黑洞及其星系群，请存档并带够燃料再前往探索，祝你好运", (AcceptableValueBase)null, new object[0])).Value;
            CustomCreateBirthStarPlugin.MorePlanet = this.Config.Bind<bool>(星区参数, "更多的行星", false, new ConfigDescription("是否在往模组生成的星系中添加更多行星", (AcceptableValueBase)null, new object[0])).Value;


            string 出生点参数 = "设置母星系位置(不兼容旧存档|重启生效)";
            CustomCreateBirthStarPlugin.BirthStarAtOStar = this.Config.Bind<bool>(
                出生点参数,
                "将圣地星系群设为母星·二选一",
                false,
                new ConfigDescription(
                    "开局建议选跳过，运气不好可能需要重开",
                    null,
                    new object[0]));
            CustomCreateBirthStarPlugin.BirthStarAtSuperMassBlackHole = this.Config.Bind<bool>(
                出生点参数,
                "将黑洞星系群设为母星·二选一",
                false,
                new ConfigDescription(
                    "开局建议选跳过，运气不好可能需要重开(当更多的行星开启时，保证出生在一颗宜居固态星球上)",
                    null,
                    new object[0]));

            string 资源参数 = "资源参数(不兼容旧存档|生成种子前生效)";
            CustomCreateBirthStarPlugin.resourceCoef = this.Config.Bind<float>(
                资源参数,
                "遗产倍率",
                1000,
                new ConfigDescription(
                    "遗产的资源倍率，此倍率 * 星区资源倍率不要超过10000，否则资源数量会异常",
                    new AcceptableValueRange<float>(1, 100000), 
                    new object[0]));
            CustomCreateBirthStarPlugin.DarkFogLv = this.Config.Bind<int>(
                资源参数,
                "防守力度",
                8,
                new ConfigDescription(
                    "遗产的防守力度，1 - 8对应弱 - 强",
                    new AcceptableValueRange<int>(1, 8), 
                    new object[0]));
            
            string 科技参数 = "科技参数(不兼容旧存档|重启生效)";
            CustomCreateBirthStarPlugin.Relics = this.Config.Bind<bool>(科技参数, "遗落科技", true, new ConfigDescription("是否开启遗落科技(会增加几个额外建筑，科技，如果不喜欢可关闭)，在特定行星挖掘零散矿石有概率获得稀有物品", (AcceptableValueBase)null, new object[0])).Value;
            CustomCreateBirthStarPlugin.NanoTech = this.Config.Bind<bool>(科技参数, "黑雾科技", true, new ConfigDescription("是否开启黑雾科技", (AcceptableValueBase)null, new object[0])).Value;
            

            CustomCreateBirthStarPlugin.Debug = this.Config.Bind<bool>(
                "Debug(离我远点)",
                "Debug(不要点)",
                false,
                new ConfigDescription(
                    "开发人员调试模式",
                    (AcceptableValueBase)null, 
                    new object[0]));

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