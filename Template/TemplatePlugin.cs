using BepInEx;
using BepInEx.Logging;
using HarmonyLib;


namespace Template
{

    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInProcess(GAME_PROCESS)]
    class TemplatePlugin : BaseUnityPlugin
    {
        public const string GUID = "kumor.plugin.TemplatePlugin";
        public const string NAME = "DSP_Kumor_TemplatePlugin";
        public const string VERSION = "1.0.0.0";
        public const string GAME_VERSION = "0.9.27.14659";
        public const string GAME_PROCESS = "DSPGAME.exe";
        public static ManualLogSource logger;

        private void Awake()
        {
            TemplatePlugin.logger = this.Logger;
            new Harmony(GUID).PatchAll();
        }
        private void Start()
        {

        }
        private void OnDestroy()
        {

        }
    }


}
