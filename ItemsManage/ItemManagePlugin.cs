using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using CommonAPI;
using CommonAPI.Systems;
using HarmonyLib;


namespace ItemsManage
{

    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInProcess(GAME_PROCESS)]
    [BepInDependency(CommonAPIPlugin.GUID)]
    [CommonAPISubmoduleDependency(nameof(ProtoRegistry), nameof(CustomDescSystem))]

    public class ItemManagePlugin : BaseUnityPlugin
    {
        public const string GUID = "kumor.plugin.ItemManage";
        public const string NAME = "DSP_Kumor_ItemManage";
        public const string VERSION = "1.0.3.0";
        public const string GAME_VERSION = "0.10.28.20856";
        public const string GAME_PROCESS = "DSPGAME.exe";
        public static ManualLogSource logger;
        private static Harmony harmony;
        
        ///<summary>
        ///模拟帧数量
        ///</summary>
        public static ConfigEntry<int> 模拟帧数量;
        ///<summary>
        ///不模拟本地星球
        ///</summary>
        public static ConfigEntry<bool> 不模拟本地星球;

        private void Awake()
        {
            this.LoadConfig();
            ItemManagePlugin.logger = this.Logger;
            ItemManagePlugin.harmony = new Harmony(GUID);
            ItemManagePlugin.harmony.PatchAll(typeof(ItemTankStorage));
            ItemManagePlugin.harmony.PatchAll(typeof(ItemManageTest));

            ItemCollector.AddItem();
            ItemManagePlugin.harmony.PatchAll(typeof(ItemCollector));
           
        }

        private void Start()
        {

        }

        private void Update()
        {
            ItemCollector.SetFrame();
        }

        private void OnDestroy()
        {

        }

        private void LoadConfig()
        {
            ItemManagePlugin.模拟帧数量 = this.Config.Bind<int>(
                "模拟帧兼容",
                "模拟帧数量",
                1,
                new ConfigDescription(
                    "1 = 正常，5 = 5倍速运行",
                    new AcceptableValueRange<int>(1, 10),
                    new object[0]));
            ItemManagePlugin.不模拟本地星球 = this.Config.Bind<bool>(
                "模拟帧兼容",
                "不模拟本地星球",
                false,
                new ConfigDescription(
                    "是否加速本地星球",
                    null,
                    new object[0]));

            
        }














    }







}



