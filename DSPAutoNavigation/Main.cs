﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using BepInEx.Logging;


namespace AutoNavigate
{
    [BepInPlugin(__GUID__, __NAME__, "1.02")]
    public class AutoNavigate : BaseUnityPlugin
    {
        public const string __NAME__ = "StellarAutoNavigation";
        public const string __GUID__ = "0x.plugins.dsp." + __NAME__;

        public static ManualLogSource logger;

        static public AutoNavigate self;
        private Player player = null;

        public static AutoStellarNavigation autoNav;

        public static bool isHistoryNav = false;
        public static ConfigEntry<double> minAutoNavEnergy;

        //public static double focusDistance;
        public static VectorLF3 focusDistance;
        public static long tipsCount;
        
        void Start()
        {
            AutoNavigate.logger = this.Logger;

            autoNav = new AutoStellarNavigation(GetNavConfig());

            self = this;
            new Harmony(__GUID__).PatchAll();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.K) && player != null)
            {
                autoNav.ToggleNav();
            }
        }

        private AutoStellarNavigation.NavigationConfig GetNavConfig()
        {
            AutoStellarNavigation.NavigationConfig navConfig = new AutoStellarNavigation.NavigationConfig();

            minAutoNavEnergy = Config.Bind<double>("AutoStellarNavigation", "minAutoNavEnergy", 50000000.0, "开启自动导航最低能量(最低50m)");
            navConfig.speedUpEnergylimit = Config.Bind<double>("AutoStellarNavigation", "SpeedUpEnergylimit", 50000000.0, "开启加速最低能量(默认50m)");
            navConfig.wrapEnergylimit = Config.Bind<double>("AutoStellarNavigation", "WrapEnergylimit", 800000000, "开启曲率最低能量(默认800m)");
            navConfig.enableLocalWrap = Config.Bind<bool>("AutoStellarNavigation", "EnableLocalWrap", true, "是否开启本地行星曲率飞行");
            navConfig.localWrapMinDistance = Config.Bind<double>("AutoStellarNavigation", "LocalWrapMinDistance", 100000.0, "本地行星曲率飞行最短距离");

            if (minAutoNavEnergy.Value < 50000000.0)
                minAutoNavEnergy.Value = 50000000.0;

            return navConfig;
        }

        class SafeMod
        {
            public static void ResetMod()
            {
                isHistoryNav = false;
                autoNav.Reset();
                autoNav.target.Reset();               
                self.player = null;
            }

            [HarmonyPatch(typeof(GameMain), "OnDestroy")]
            public class SafeDestroy
            {
                private static void Prefix()
                {
                    ResetMod();
                }
            }

            [HarmonyPatch(typeof(GameMain), "Pause")]
            public class SafePause
            {
                public static void Prefix()
                {
                    autoNav.pause();
                }
            }

            [HarmonyPatch(typeof(GameMain), "Resume")]
            public class SafeResume
            {
                public static void Prefix()
                {
                    autoNav.resume();
                }
            }
        }


        [HarmonyPatch(typeof(PlayerController), "Init")]
        private class PlayerControllerInit
        {
            private static void Postfix(PlayerController __instance)
            {
                self.player = __instance.player;
            }
        }

        private static string Tips()
        {
            if (AutoNavigate.tipsCount > 200)
            {
                return ("缺德地图持续为您导航");
            }
            else
            {
                AutoNavigate.tipsCount++;
                double Distance = (AutoNavigate.focusDistance - GameMain.mainPlayer.uPosition).magnitude;
                float ly = (float)(Distance / (double)(60 * 40000));
                return ("准备出发，全程" + ly.ToString("f3") + "光年\n缺德地图持续为您导航");
            }

            

        }

/// --------------------------
/// AutoStellarNavigation
/// --------------------------
        [HarmonyPatch(typeof(UIGeneralTips), "_OnUpdate")]
        private class NavigateTips
        {
            static Vector2 anchoredPosition = new Vector2(0.0f, 260.0f);

            public static void Postfix(UIGeneralTips __instance)
            {
                Text modeText = Traverse.Create((object)__instance).Field("modeText").GetValue<Text>();
                if (autoNav.enable)
                {
                    modeText.gameObject.SetActive(true);
                    modeText.rectTransform.anchoredPosition = anchoredPosition;

                    if (autoNav.IsCurNavPlanet())
                    {
                        modeText.text = AutoNavigate.Tips();
                    }
                        


                    else if (autoNav.IsCurNavStar())
                    {
                        modeText.text = AutoNavigate.Tips();
                    }
                        

                    autoNav.modeText = modeText;
                }

            }
        }

        [HarmonyPatch(typeof(VFInput), "_sailSpeedUp", MethodType.Getter)]
        private class SailSpeedUp
        {
            private static void Postfix(ref bool __result)
            {
                if (autoNav.enable && autoNav.sailSpeedUp)
                {
                    __result = true;
                }                 
            }
        }

        /// <summary>
        /// Sail Mode
        /// </summary>
        [HarmonyPatch(typeof(PlayerMove_Sail), "GameTick")]
        private class SailMode_AutoNavigate
        {
            private static VectorLF3 ofwdRayUDir;

            private static void Prefix(PlayerMove_Sail __instance)
            {
                if (autoNav.enable && (__instance.player.sailing || __instance.player.warping))
                {
                    ++__instance.controller.input0.y;
                    ofwdRayUDir = __instance.controller.fwdRayUDir;

                    if (autoNav.IsCurNavStar())
                    {
                       autoNav.StarNavigation(__instance);
                       
                    }
                    else if (autoNav.IsCurNavPlanet() )
                    {
                        autoNav.PlanetNavigation(__instance);
                    }
                }
            }

            private static void Postfix(PlayerMove_Sail __instance)
            {
                if (autoNav.enable && (GameMain.localPlanet != null || autoNav.target.IsVaild() ))
                {
                    autoNav.HandlePlayerInput();
                }
                __instance.controller.fwdRayUDir = ofwdRayUDir;
                autoNav.sailSpeedUp = false;
            }
        }

        /// <summary>
        /// Fly Mode
        /// </summary>
        [HarmonyPatch(typeof(PlayerMove_Fly), "GameTick")]
        private class FlyMode_TrySwtichToSail
        {
            static float sailMinAltitude = 49.0f;

            private static void Prefix(PlayerMove_Fly __instance)
            {
                if (autoNav.enable)
                {
                    if (__instance.player.movementState != EMovementState.Fly)
                        return;

                    if (autoNav.DetermineArrive())
                    {
                        ModDebug.Log("FlyModeArrive");
                        autoNav.Arrive();

                    }
                    else if (
                        __instance.mecha.thrusterLevel < 2)
                    {
                        autoNav.Arrive("驱动引擎等级过低");
                    }
                    else if (__instance.player.mecha.coreEnergy < minAutoNavEnergy.Value)
                    {
                        autoNav.Arrive("机甲能量过低");
                    }
                    else
                    {
                        ++__instance.controller.input1.y;

                        if (__instance.currentAltitude > sailMinAltitude)
                        {
                            AutoStellarNavigation.Fly.TrySwtichToSail(__instance);
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Walk Mode
        /// </summary>
        [HarmonyPatch(typeof(PlayerMove_Walk), "UpdateJump")]
        private class WalkMode_TrySwticToFly
        {
            private static void Postfix(PlayerMove_Walk __instance, ref bool __result)
            {

                if (autoNav.enable && autoNav.target.IsVaild())
                {
                    if (autoNav.DetermineArrive())
                    {
                        ModDebug.Log("WalkModeArrive");
                        autoNav.Arrive();
                    }
                    else if (
                        __instance.mecha.thrusterLevel < 1)
                    {
                        autoNav.Arrive("驱动引擎等级过低");
                    }
                    else if (__instance.player.mecha.coreEnergy < minAutoNavEnergy.Value)
                    {
                        autoNav.Arrive("机甲能量过低");
                    }
                    else
                    {
                        AutoStellarNavigation.Walk.TrySwitchToFly(__instance);
                        __result = true;
                        return;

                    }

                    __result = false;
                }

            }

        }


/// --------------------------
/// Starmap Indicator
/// --------------------------
///
        [HarmonyPatch(typeof(UIStarmap), "OnCursorFunction3Click")]
        private class OnSetIndicatorAstro
        {
            private static void Prefix(UIStarmap __instance)
            {

                PlayerNavigation navigation = GameMain.mainPlayer.navigation;
                if (__instance.focusPlanet != null &&
                    navigation._indicatorAstroId != __instance.focusPlanet.planet.id)
                {
                    //AutoNavigate.focusDistance = (__instance.focusPlanet.planet.uPosition - GameMain.mainPlayer.uPosition).magnitude;
                    AutoNavigate.focusDistance = __instance.focusPlanet.planet.uPosition;
                    AutoNavigate.tipsCount = 0;
                    autoNav.target.SetTarget(__instance.focusPlanet.planet);
                    
                    AutoNavigate.logger.LogInfo("planetid:" + __instance.focusPlanet.planet.id.ToString());
                }
                else if (__instance.focusStar != null && navigation._indicatorAstroId != __instance.focusStar.star.id * 100)
                {
                    //AutoNavigate.focusDistance = (__instance.focusStar.star.uPosition - GameMain.mainPlayer.uPosition).magnitude;
                    AutoNavigate.focusDistance = __instance.focusStar.star.uPosition;
                    AutoNavigate.tipsCount = 0;
                    autoNav.target.SetTarget(__instance.focusStar.star);
                    AutoNavigate.logger.LogInfo("starid:" + __instance.focusStar.star.id.ToString()+ "   focusDistance:"+ AutoNavigate.focusDistance.sqrMagnitude.ToString());
                }

                
            }
        }

        


    }
}