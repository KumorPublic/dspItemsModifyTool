using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;


namespace AutoBuild
{
    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInProcess(GAME_PROCESS)]
    public class AutoBuildPlugin : BaseUnityPlugin
    {
        public const string GUID = "kumor.plugin.AutoBuild";
        public const string NAME = "DSP_Kumor_AutoBuild";
        public const string VERSION = "1.0.4.0";
        public const string GAME_VERSION = "0.10.27.15033";
        public const string GAME_PROCESS = "DSPGAME.exe";
        public static ManualLogSource logger;
        public static Player player;
        public static PlayerAction_Build actionBuild;
        ///<summary>
        ///是否开启自动建造
        ///</summary>
        public static ConfigEntry<bool> AutoBuild;
        ///<summary>
        ///自动建造内部标识
        ///</summary>
        public static bool AutoBuildFlag = false;
        ///<summary>
        ///充电内部标识
        ///</summary>
        public static int PowerFlag = 0;
        ///<summary>
        ///自动充电百分比
        ///</summary>
        public static ConfigEntry<float> PowerPer;
        ///<summary>
        ///是否开启自动充电
        ///</summary>
        public static ConfigEntry<bool> AutoPowerCharger;
        ///<summary>
        ///充电站充电距离
        ///</summary>
        public static ConfigEntry<float> PowerChargerDt;

        ///<summary>
        ///开启关闭步行建造模式
        ///</summary>
        public static ConfigEntry<bool> WalkBuild;


        private void Awake()
        {
            AutoBuildPlugin.logger = this.Logger;
            new Harmony(GUID).PatchAll(typeof(AutoBuildPatch));

            AutoBuildPlugin.AutoBuild = this.Config.Bind<bool>("自动建造", "是否开启自动建造", true, new ConfigDescription("是否开启自动建造", (AcceptableValueBase)null, new object[0]));
            AutoBuildPlugin.AutoPowerCharger = this.Config.Bind<bool>("自动建造", "是否开启自动充电", true, new ConfigDescription("是否开启自动充电", (AcceptableValueBase)null, new object[0]));
            AutoBuildPlugin.PowerPer = this.Config.Bind<float>("自动建造", "充电下限百分比", 0.3f, new ConfigDescription("充电下限百分比", (AcceptableValueBase)new AcceptableValueRange<float>(0.1f, 0.99f), new object[0]));
            AutoBuildPlugin.PowerChargerDt = this.Config.Bind<float>("自动建造", "充电站充电距离", 5.5f, new ConfigDescription("充电站充电距离", (AcceptableValueBase)new AcceptableValueRange<float>(3f, 30f), new object[0]));
            AutoBuildPlugin.WalkBuild = this.Config.Bind<bool>("自动建造", "步行建造模式", false, new ConfigDescription("开启关闭步行建造模式", (AcceptableValueBase)null, new object[0]));

        }
        private void Start()
        {

        }
        private void Update()
        {

            if (!AutoBuildPlugin.AutoBuild.Value || AutoBuildPlugin.player == null)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                AutoBuildPlugin.AutoBuildFlag = !AutoBuildPlugin.AutoBuildFlag;
                if (!AutoBuildPlugin.AutoBuildFlag)
                {
                    AutoBuildPatch.ExitAutoBuild();
                }
                else
                {
                    if (!AutoBuildPlugin.WalkBuild.Value)
                    {
                        AutoBuildPatch.Fly();
                    }

                }
                AutoBuildPlugin.logger.LogInfo("AutoBuildFlag: " + AutoBuildPlugin.AutoBuildFlag.ToString() + "  KeyCode.Z");
            }
        }


        private void OnDestroy()
        {

        }




    }




}
