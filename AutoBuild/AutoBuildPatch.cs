using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;


namespace AutoBuild
{
    class AutoBuildPatch
    {
        static Text modText;

        //从步行进入飞行
        public static void Fly()
        {
            if (AutoBuildPlugin.WalkBuild.Value)
            {
                return;
            }

            if (AutoBuildPlugin.player.controller.mecha.thrusterLevel > 0 && AutoBuildPlugin.player.movementState == EMovementState.Walk)
            {
                AutoBuildPlugin.player.movementState = EMovementState.Fly;
                AutoBuildPlugin.player.controller.actionWalk.jumpCoolTime = 0.3f;
                AutoBuildPlugin.player.controller.actionWalk.jumpedTime = 0.0f;
                AutoBuildPlugin.player.controller.actionWalk.flyUpChance = 0.0f;
                AutoBuildPlugin.player.controller.actionWalk.SwitchToFly();
            }
        }


        //从飞行进入步行
        public static void Walk()
        {
            if (AutoBuildPlugin.player.movementState == EMovementState.Fly)
            {
                AutoBuildPlugin.player.movementState = EMovementState.Walk;
                AutoBuildPlugin.player.controller.movementStateInFrame = EMovementState.Walk;
                AutoBuildPlugin.player.controller.softLandingTime = 1.2f;
            }

        }

        //退出自动建造
        public static void ExitAutoBuild(string extraTip = null)
        {
            AutoBuildPlugin.AutoBuildFlag = false;
            AutoBuildPlugin.PowerFlag = 0;

            string text = "自动建造模式结束";
            if (extraTip != null)
            {
                text = text + "-" + extraTip;
            }
            UIRealtimeTip.Popup(text);
            if (AutoBuildPatch.modText != null && AutoBuildPatch.modText.IsActive())
            {
                AutoBuildPatch.modText.gameObject.SetActive(false);
                AutoBuildPatch.modText.text = string.Empty;
            }
            AutoBuildPlugin.logger.LogInfo("ExitAutoBuild => " + text);
            return;
        }


        [HarmonyPostfix]
        [HarmonyPatch(typeof(UIGeneralTips), "_OnUpdate")]
        public static void UIGeneralTips_OnUpdate(UIGeneralTips __instance)
        {
            if (!AutoBuildPlugin.AutoBuild.Value || !AutoBuildPlugin.AutoBuildFlag)
            {
                return;
            }
            Text text = Traverse.Create(__instance).Field("modeText").GetValue<Text>();
            AutoBuildPatch.modText = text;
            AutoBuildPatch.modText.rectTransform.anchoredPosition = new Vector2(0.0f, 160f);
            AutoBuildPatch.modText.gameObject.SetActive(true);
            if (AutoBuildPlugin.PowerFlag == 1)
            {
                AutoBuildPatch.modText.text = "正在寻找充电站...";
            }
            else if (AutoBuildPlugin.PowerFlag == 2)
            {
                AutoBuildPatch.modText.text = "正在充电...";
            }
            else if (AutoBuildPlugin.PowerFlag == 0)
            {
                AutoBuildPatch.modText.text = "自动建造模式";
            }
        }


        //获取自动建造所需的player,actionBuild对象
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerController), "Init")]
        public static void PlayerControllerInit(PlayerController __instance)
        {
            AutoBuildPlugin.player = __instance.player;
            AutoBuildPlugin.actionBuild = __instance.actionBuild;
            AutoBuildPlugin.logger.LogInfo("PlayerController Init OK");
        }


        //返回true：前往充电，返回false：无法充电
        private static bool PowerCharger(int mode)
        {
            bool needOrder = false;
            Vector3 PowerNode3D = new Vector3(0.0f, 0.0f, 0.0f);
            float min = 999f;
            Vector3 d3D;
            foreach (PowerNodeComponent PowerNode in GameMain.localPlanet.factory.powerSystem.nodePool)
            {


                if (PowerNode.id != 0 && PowerNode.isCharger)
                {
                    if (AutoBuildPlugin.player.planetData.factory.entityPool[PowerNode.entityId].protoId == 2202)
                    {
                        if (PowerNode3D.magnitude == 0.0f)
                        {
                            needOrder = true;
                            PowerNode3D = PowerNode.powerPoint;
                            d3D = PowerNode3D - AutoBuildPlugin.player.position;
                            min = d3D.magnitude;
                        }
                        else
                        {
                            float temp = min;
                            d3D = PowerNode.powerPoint - AutoBuildPlugin.player.position;
                            if (temp > d3D.magnitude)
                            {
                                needOrder = true;
                                PowerNode3D = PowerNode.powerPoint;
                                d3D = PowerNode3D - AutoBuildPlugin.player.position;
                                min = d3D.magnitude;
                            }
                        }
                    }

                }
            }
            if (needOrder)
            {
                if (mode == 0)
                {
                    AutoBuildPlugin.PowerFlag = 1;
                    AutoBuildPlugin.logger.LogInfo("PowerNode Dt: " + min.ToString());
                }
                else if (mode == 1)
                {
                    AutoBuildPatch.Walk();
                    if (min < AutoBuildPlugin.PowerChargerDt.Value)
                    {
                        AutoBuildPlugin.PowerFlag = 2;
                    }
                    AutoBuildPlugin.logger.LogInfo("PowerNode Dt: " + min.ToString());
                }
                AutoBuildPlugin.player.Order(new OrderNode() { target = PowerNode3D, type = EOrderType.Move }, false);
                return true;
            }
            else
            {
                return false;
            }
        }



        //自动建造
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerController), "GetInput")]
        public static void PlayerControllerGetInput()
        {
            //判断功能是否开启
            if (!AutoBuildPlugin.AutoBuild.Value || !AutoBuildPlugin.AutoBuildFlag)
            {
                return;
            }
            //判断玩家是否主动移动
            if (((bool)VFInput._moveForward) || (bool)VFInput._moveBackward || (bool)VFInput._moveLeft || (bool)VFInput._moveRight || VFInput.alt)
            {
                AutoBuildPatch.ExitAutoBuild("玩家移动");
                return;
            }
            //判断玩家是否飞行,且不在充电
            if ((AutoBuildPlugin.player.movementState != EMovementState.Fly && AutoBuildPlugin.PowerFlag == 0 && !AutoBuildPlugin.WalkBuild.Value) || AutoBuildPlugin.player.movementState == EMovementState.Sail)
            {
                AutoBuildPatch.ExitAutoBuild("退出飞行");
                return;
            }
            //判断玩家是否进入建造模式
            if (UIGame.viewMode == EViewMode.Build)
            {
                AutoBuildPatch.ExitAutoBuild("进入手动建造模式");
                return;
            }


            if (GameMain.localPlanet != null && AutoBuildPlugin.actionBuild != null && AutoBuildPlugin.actionBuild.clickTool != null && AutoBuildPlugin.actionBuild.clickTool.factory != null && AutoBuildPlugin.actionBuild.clickTool.factory.prebuildPool != null)
            {

                if (AutoBuildPlugin.player.currentOrder != null && !AutoBuildPlugin.player.currentOrder.targetReached)
                {
                    if ((double)(AutoBuildPlugin.player.position - AutoBuildPlugin.player.currentOrder.target).magnitude < 20.0)
                    {
                        AutoBuildPlugin.player.AchieveOrder();
                        //AutoBuildPlugin.logger.LogInfo("AchieveOrder...");
                    }
                    return;
                }


                if (AutoBuildPlugin.AutoPowerCharger.Value)
                {
                    double coreEnergyPer = AutoBuildPlugin.player.mecha.coreEnergy / AutoBuildPlugin.player.mecha.coreEnergyCap;
                    if (AutoBuildPlugin.PowerFlag > 0)
                    {
                        //正在充电
                        if (coreEnergyPer <= 0.99d)
                        {
                            if (AutoBuildPlugin.PowerFlag == 1)//还没到达
                            {
                                AutoBuildPatch.PowerCharger(1);
                                AutoBuildPlugin.logger.LogInfo("PowerCharger(1)...");
                            }
                            else if (AutoBuildPlugin.PowerFlag == 2)//到达充电站
                            {
                                //AutoBuildPlugin.logger.LogInfo("PowerCharger(2)...");
                            }
                            return;
                        }
                        else if (coreEnergyPer > 0.99d)//充好了
                        {
                            AutoBuildPlugin.PowerFlag = 0;
                            AutoBuildPatch.Fly();
                            AutoBuildPlugin.logger.LogInfo("PowerCharger(exit)...");
                        }

                    }
                    else
                    {
                        //当能量小于30%，前去充电
                        if (coreEnergyPer < (double)AutoBuildPlugin.PowerPer.Value)
                        {
                            if (AutoBuildPatch.PowerCharger(0))
                            {
                                AutoBuildPlugin.logger.LogInfo("PowerCharger(0)...");
                            }
                            else
                            {
                                AutoBuildPatch.ExitAutoBuild("能量过低");
                            }
                            return;
                        }
                    }

                }







                bool needOrder = false;
                Vector3 build3D = new Vector3(0.0f, 0.0f, 0.0f);
                float min = 999f;
                Vector3 d3D;
                foreach (PrebuildData prebuildData in AutoBuildPlugin.actionBuild.clickTool.factory.prebuildPool)
                {
                    if (prebuildData.id != 0 && (prebuildData.itemRequired == 0 || prebuildData.itemRequired <= AutoBuildPlugin.player.package.GetItemCount((int)prebuildData.protoId)))
                    {
                        if (build3D.magnitude == 0.0f)
                        {
                            needOrder = true;
                            build3D = prebuildData.pos;
                            d3D = build3D - AutoBuildPlugin.player.position;
                            min = d3D.magnitude;
                        }
                        else
                        {
                            float temp = min;
                            d3D = prebuildData.pos - AutoBuildPlugin.player.position;
                            if (temp > d3D.magnitude)
                            {
                                needOrder = true;
                                build3D = prebuildData.pos;
                                d3D = build3D - AutoBuildPlugin.player.position;
                                min = d3D.magnitude;
                            }
                        }
                    }
                }




                //AutoBuildPlugin.logger.LogInfo("player.Order   " + flag.ToString());
                if (needOrder)
                {
                    AutoBuildPlugin.player.Order(new OrderNode() { target = build3D, type = EOrderType.Move }, false);
                }

            }
        }





















    }
}
