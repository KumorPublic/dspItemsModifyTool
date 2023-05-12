using BepInEx;
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
        public const string VERSION = "1.0.2.0";
        public const string GAME_VERSION = "0.9.27.15033";
        public const string GAME_PROCESS = "DSPGAME.exe";
        public static ManualLogSource logger;
        private static Harmony harmony;


        private void Awake()
        {
            this.LoadConfig();
            ItemManagePlugin.logger = this.Logger;
            ItemManagePlugin.harmony = new Harmony(GUID);
            ItemManagePlugin.harmony.PatchAll(typeof(ItemTankStorage));
            ItemManagePlugin.harmony.PatchAll(typeof(ItemStationSolt));
            ItemManagePlugin.harmony.PatchAll(typeof(ItemManageTest));
            //ItemManagePlugin.harmony.PatchAll(typeof(ItemWarpStorage));

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


        }














    }







}



