using BepInEx;
using BepInEx.Logging;
using HarmonyLib;


namespace ShowMe

{
    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInProcess(GAME_PROCESS)]
    class ShowMePlugin : BaseUnityPlugin
    {
        public const string GUID = "kumor.plugin.ShowMe";
        public const string NAME = "DSP_Kumor_ShowMe";
        public const string VERSION = "1.0.1.0";
        public const string GAME_VERSION = "0.9.27.15033";
        public const string GAME_PROCESS = "DSPGAME.exe";
        public static ManualLogSource logger;


        private void Awake()
        {
            ShowMePlugin.logger = this.Logger;
            new Harmony(GUID).PatchAll(typeof(ShowMePatch));


        }
        private void Start()
        {

        }
        private void Update()
        {

        }

        private void OnDestroy()
        {

        }
    }

}
