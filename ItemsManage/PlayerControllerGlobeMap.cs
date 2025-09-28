using HarmonyLib;
using NGPT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;



namespace ItemsManage
{
    public class PlayerControllerGlobeMap
    {
        private static RaycastHit hitInfo = new RaycastHit();
        private static bool hit = false;
        private static Vector3 dragBeginMousePosition;

        // 每个 GameTick，检查玩家是否正在使用移动按钮，todo：找到禁用移动的任何东西并改变它，而不是改变 GameTick，这样会更简单
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerController), "GameTick")]
        private static void GlobeMapMove(PlayerController __instance, long time)
        {
            // 仅在星球地图打开时才处理
            if (!UIRoot.instance.uiGame.globemap.active) return;

            var mousePos = Input.mousePosition;

            if (VFInput._rtsMove.onDown)
            {
                dragBeginMousePosition = mousePos;
                hit = Physics.Raycast(
                    Camera.main.ScreenPointToRay(mousePos),
                    out hitInfo,
                    800f,
                    8720,
                    QueryTriggerInteraction.Collide
                );
            }
            else if (VFInput._rtsMove.onUp)
            {
                // 鼠标几乎没移动，判定为点击而不是拖拽
                float sqrDist = (dragBeginMousePosition - mousePos).sqrMagnitude;
                if (sqrDist < 800f && hit)
                {
                    var player = GameMain.data.mainPlayer;
                    player.Order(OrderNode.MoveTo(hitInfo.point), VFInput._multiOrdering);
                    RTSTargetGizmo.Create(hitInfo.point);
                    hit = false;
                }
            }
        }

    }
}
