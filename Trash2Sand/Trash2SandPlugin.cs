using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;


namespace Trash2Sand

{
    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInProcess(GAME_PROCESS)]

    class Trash2SandPlugin : BaseUnityPlugin
    {
        public const string GUID = "kumor.plugin.Trash2Sand";
        public const string NAME = "DSP_Kumor_Trash2Sand";
        public const string VERSION = "1.0.4.0";
        public const string GAME_VERSION = "0.9.27.15033";
        public const string GAME_PROCESS = "DSPGAME.exe";
        public static ManualLogSource logger;

        public static ConfigEntry<int> Trash2SandMult;
        public static ConfigEntry<bool> FreeloaderMode;
        private void Start()
        {
            Trash2SandPlugin.logger = this.Logger;
            new Harmony(GUID).PatchAll(typeof(Trash2SandPatch));
            Trash2SandPlugin.Trash2SandMult = this.Config.Bind<int>("Trash2Sand", "Trash2SandMult", 1, new ConfigDescription("自定义沙土兑换倍率", (AcceptableValueBase)new AcceptableValueRange<int>(1, 100), new object[0]));
            Trash2SandPlugin.FreeloaderMode = this.Config.Bind<bool>("Trash2Sand", "FreeloaderMode", false, new ConfigDescription("是否开启白嫖模式，该模式下可以将任何在地上的垃圾转化为沙土，比如抽海水抽氢气。关闭后只允许转化石头，硅头，金伯利矿，分形硅矿。默认关闭。", (AcceptableValueBase)null, new object[0]));

        }
    }
}

