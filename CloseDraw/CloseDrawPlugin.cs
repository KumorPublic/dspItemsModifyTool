using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;


namespace CloseDraw
{

    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInProcess(GAME_PROCESS)]

    class CloseDrawPlugin : BaseUnityPlugin
    {
        public const string GUID = "kumor.plugin.CloseDraw";
        public const string NAME = "DSP_Kumor_CloseDraw";
        public const string VERSION = "1.0.0.0";
        public const string GAME_VERSION = "0.9.27.15033";
        public const string GAME_PROCESS = "DSPGAME.exe";
        public static ManualLogSource logger;
        ///<summary>
        ///是否渲染飞机
        ///</summary>
        public static ConfigEntry<bool> drawShipDrone;
        ///<summary>
        ///是否渲染戴森壳
        ///</summary>
        public static ConfigEntry<bool> drawDysonShell;
        ///<summary>
        ///是否渲染戴森云
        ///</summary>
        public static ConfigEntry<bool> drawDysonSwarm;
        ///<summary>
        ///是否渲染电网
        ///</summary>
        public static ConfigEntry<bool> drawPowerNet;
        ///<summary>
        ///是否渲染研究站
        ///</summary>
        public static ConfigEntry<bool> drawLabRenderer;
        ///<summary>
        ///是否渲染建筑
        ///</summary>
        public static ConfigEntry<bool> DrawInstanced;
        ///<summary>
        ///是否渲染戴森球框架
        ///</summary>
        public static ConfigEntry<bool> DysonFrame;



        private void Start()
        {
            CloseDrawPlugin.logger = this.Logger;
            new Harmony("kumor.plugin.ItemsModifyTool").PatchAll(typeof(CloseDrawPatch));

            CloseDrawPlugin.drawShipDrone = this.Config.Bind<bool>("通用", "飞机", true, new ConfigDescription("是否渲染飞机", (AcceptableValueBase)null, new object[0]));
            CloseDrawPlugin.DrawInstanced = this.Config.Bind<bool>("通用", "建筑", true, new ConfigDescription("是否渲染建筑", (AcceptableValueBase)null, new object[0]));
            CloseDrawPlugin.drawPowerNet = this.Config.Bind<bool>("通用", "电网", true, new ConfigDescription("是否渲染电网", (AcceptableValueBase)null, new object[0]));
            CloseDrawPlugin.drawLabRenderer = this.Config.Bind<bool>("通用", "研究站", true, new ConfigDescription("是否渲染研究站", (AcceptableValueBase)null, new object[0]));
            CloseDrawPlugin.drawDysonShell = this.Config.Bind<bool>("戴森球", "戴森球", true, new ConfigDescription("是否渲染戴森球", (AcceptableValueBase)null, new object[0]));
            CloseDrawPlugin.drawDysonSwarm = this.Config.Bind<bool>("戴森球", "戴森云", true, new ConfigDescription("是否渲染戴森云", (AcceptableValueBase)null, new object[0]));
            CloseDrawPlugin.DysonFrame = this.Config.Bind<bool>("戴森球", "框架", true, new ConfigDescription("是否渲染戴森球框架", (AcceptableValueBase)null, new object[0]));

        }
    }
}
