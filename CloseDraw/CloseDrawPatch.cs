using HarmonyLib;
using UnityEngine;

namespace CloseDraw
{
    [HarmonyPatch]
    class CloseDrawPatch
    {
        //不渲染飞机飞船
        [HarmonyPrefix]
        [HarmonyPatch(typeof(LogisticDroneRenderer), "Draw")]
        public static bool DrawDronePrefix() => CloseDrawPlugin.drawShipDrone.Value;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(LogisticShipRenderer), "Draw")]
        public static bool DrawShipPrefix() => CloseDrawPlugin.drawShipDrone.Value;
        //不渲染戴森壳
        [HarmonyPrefix]
        [HarmonyPatch(typeof(DysonSphere), "DrawModel")]
        public static bool DrawDysonShellPrefix() => CloseDrawPlugin.drawDysonShell.Value;
        //不渲染戴森云
        [HarmonyPrefix]
        [HarmonyPatch(typeof(DysonSphere), "DrawPost")]
        public static bool DrawDysonSwarmPrefix(DysonSphere __instance)
        {
            if (CloseDrawPlugin.drawDysonSwarm.Value)
                return true;
            if (GameCamera.sceneIndex == 2 || DysonSphere.renderPlace == ERenderPlace.Starmap && UIStarmap.isChangingToMilkyWay)
                return false;
            Shader.SetGlobalInt("_Global_DS_RenderPlace", (int)DysonSphere.renderPlace);
            Shader.SetGlobalInt("_Global_DS_EditorMaskS", __instance.inEditorRenderMaskS);
            Shader.SetGlobalInt("_Global_DS_GameMaskS", __instance.inGameRenderMaskS);
            Shader.SetGlobalInt("_Global_DS_HideFarSide", UIDysonEditor.hideFarSide ? 1 : 0);
            __instance.swarm.DrawPost(DysonSphere.renderPlace);
            return false;
        }
        //不渲染电网
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PowerSystemRenderer), "DrawDisks")]
        public static bool DrawDisksPrefix() => CloseDrawPlugin.drawPowerNet.Value;

        //不渲染研究站
        [HarmonyPrefix]
        [HarmonyPatch(typeof(LabRenderer), "Render")]
        public static bool LabRendererPrefix(LabRenderer __instance) => __instance.modelId != 70 || CloseDrawPlugin.drawLabRenderer.Value;

        //不渲染建筑
        [HarmonyPrefix]
        [HarmonyPatch(typeof(FactoryModel), "DrawInstancedBatches")]
        public static bool FactoryModelDrawInstancedBatches(LabRenderer __instance) => CloseDrawPlugin.DrawInstanced.Value;

        /**停止旋转

        [HarmonyPrefix]
        [HarmonyPatch(typeof(DysonSphereLayer), "GameTick")]
        public static bool DysonSphereLayerGameTickPrefix(DysonSphereLayer __instance, long gameTick)
        {
            //__instance.currentAngle += __instance.orbitAngularSpeed * 0.01666667f;
            //if ((double)__instance.currentAngle > 360.0)
            //    __instance.currentAngle -= 360f;
            //if ((double)__instance.currentAngle < 0.0)
            //    __instance.currentAngle += 360f;
            //__instance.currentRotation = __instance.orbitRotation * Quaternion.Euler(0.0f, -__instance.currentAngle, 0.0f);
            //__instance.nextRotation = __instance.orbitRotation * Quaternion.Euler(0.0f, (float)(-(double)__instance.currentAngle - (double)__instance.orbitAngularSpeed * 0.0166666675359011), 0.0f);
            DysonSwarm swarm = __instance.dysonSphere.swarm;
            int num = (int)(gameTick % 120L);
            for (int index = 1; index < __instance.nodeCursor; ++index)
            {
                DysonNode dysonNode = __instance.nodePool[index];
                if (dysonNode != null && dysonNode.id == index && dysonNode.id % 120 == num && dysonNode.sp == dysonNode.spMax)
                    dysonNode.OrderConstructCp(gameTick, swarm);
            }


            return false;
        }
        **/


        /** 戴森球层级
        [HarmonyPrefix]
        [HarmonyPatch(typeof(DysonSphereSegmentRenderer), "DrawModels")]
        public static bool DrawModels(DysonSphereSegmentRenderer __instance,ERenderPlace place, int mask, ref int ____counter, ref Vector4[] ___layerRotations, ref Material[] ___instMats,ref ComputeBuffer ___argBuffer,ref uint[] ___argArr)
        {
            Vector3 pos1 = Vector3.zero;
            Vector4 vector4 = new Vector4(0.0f, 0.0f, 0.0f, 1f);
            Vector3 pos2 = Vector3.zero;
            if (__instance.starData != null && __instance.gameData != null)
            {
                PlanetData localPlanet = __instance.gameData.localPlanet;
                Player mainPlayer = __instance.gameData.mainPlayer;
                pos1 = localPlanet == null ? (Vector3)(__instance.starData.uPosition - mainPlayer.uPosition) : (Vector3)Maths.QInvRotateLF(localPlanet.runtimeRotation, __instance.starData.uPosition - localPlanet.uPosition);
                if (DysonSphere.renderPlace == ERenderPlace.Starmap)
                    pos2 = (Vector3)((__instance.starData.uPosition - UIStarmap.viewTargetStatic) * 0.00025);
                if (localPlanet != null)
                    vector4 = new Vector4(localPlanet.runtimeRotation.x, localPlanet.runtimeRotation.y, localPlanet.runtimeRotation.z, localPlanet.runtimeRotation.w);
            }
            for (int index = 0; index < DysonSphereSegmentRenderer.totalProtoCount; ++index)
            {
                if (__instance.batches[index] != null)
                    ___argArr[index * 5 + 1] = (uint)__instance.batches[index].cursor;
            }
            ___argBuffer.SetData(___argArr);
            for (uint index = 1; index <= 10U; ++index)
            {
                //=======================================
                if(!CloseDrawPlugin.DysonRotations[index-1].Value)
                {
                    continue;
                }
                //=======================================
                DysonSphereLayer dysonSphereLayer = __instance.dysonSphere.layersIdBased[(int)index];
                if (dysonSphereLayer != null)
                {
                    ___layerRotations[(int)index].x = dysonSphereLayer.currentRotation.x;
                    ___layerRotations[(int)index].y = dysonSphereLayer.currentRotation.y;
                    ___layerRotations[(int)index].z = dysonSphereLayer.currentRotation.z;
                    ___layerRotations[(int)index].w = dysonSphereLayer.currentRotation.w;
                }
                else
                {
                    ___layerRotations[(int)index].x = 0.0f;
                    ___layerRotations[(int)index].y = 0.0f;
                    ___layerRotations[(int)index].z = 0.0f;
                    ___layerRotations[(int)index].w = 1f;
                }
            }
            
            int layer = 16;
            switch (DysonSphere.renderPlace)
            {
                case ERenderPlace.Starmap:
                    Camera screenCamera1 = UIRoot.instance.uiGame.starmap.screenCamera;
                    layer = 20;
                    break;
                case ERenderPlace.Dysonmap:
                    Camera screenCamera2 = UIRoot.instance.uiGame.dysonmap.screenCamera;
                    layer = 21;
                    break;
            }

            if (CloseDrawPlugin.DysonFrame.Value)
            {
                for (int index = 0; index < DysonSphereSegmentRenderer.totalProtoCount; ++index)
                {
                    if (__instance.batches[index] != null && __instance.batches[index].cursor > 0)
                    {
                        __instance.batches[index].SyncBufferData();
                        ___instMats[index].SetBuffer("_InstBuffer", __instance.batches[index].buffer);
                        ___instMats[index].SetVectorArray("_LayerRotations", ___layerRotations);
                        ___instMats[index].SetVector("_SunPosition", (Vector4)pos1);
                        ___instMats[index].SetVector("_SunPosition_Map", (Vector4)pos2);
                        ___instMats[index].SetVector("_LocalRot", vector4);
                        ___instMats[index].SetColor("_SunColor", __instance.dysonSphere.sunColor);
                        ___instMats[index].SetColor("_DysonEmission", __instance.dysonSphere.emissionColor);
                        Graphics.DrawMeshInstancedIndirect(DysonSphereSegmentRenderer.protoMeshes[index], 0, ___instMats[index], new Bounds(Vector3.zero, new Vector3(300000f, 300000f, 300000f)), ___argBuffer, index * 5 * 4, (MaterialPropertyBlock)null, ShadowCastingMode.Off, false, layer);
                        if (PerformanceMonitor.GpuProfilerOn)
                        {
                            int cursor = __instance.batches[index].cursor;
                            int vertexCount = DysonSphereSegmentRenderer.protoMeshes[index].vertexCount;
                            PerformanceMonitor.RecordGpuWork(index < DysonSphereSegmentRenderer.nodeProtoCount ? EGpuWorkEntry.DysonNode : EGpuWorkEntry.DysonFrame, cursor, cursor * vertexCount);
                        }
                    }
                }
            }

            
            Shader.SetGlobalVector("_Global_DS_SunPosition", (Vector4)pos1);
            Shader.SetGlobalVector("_Global_DS_SunPosition_Map", (Vector4)pos2);
            for (uint index1 = 1; index1 <= 10U; ++index1)
            {
                //=======================================
                if (!CloseDrawPlugin.DysonLayer[index1-1].Value)
                {
                    continue;
                }
                //=======================================

                DysonSphereLayer dysonSphereLayer = __instance.dysonSphere.layersIdBased[(int)index1];
                if (dysonSphereLayer != null)
                {
                    int num = ____counter % 10 == 0 ? ____counter / 10 % dysonSphereLayer.shellCursor : 0;
                    for (int index2 = 1; index2 < dysonSphereLayer.shellCursor; ++index2)
                    {
                        DysonShell dysonShell = dysonSphereLayer.shellPool[index2];
                        if (dysonShell != null && dysonShell.id == index2)
                        {
                            if (index2 == num)
                                dysonShell.SyncCellBuffer();
                            dysonShell.material.SetFloat("_State", (float)dysonShell.state);
                            if (DysonSphere.renderPlace == ERenderPlace.Universe)
                                Graphics.DrawMesh(dysonShell.mesh, Matrix4x4.TRS(pos1, new Quaternion(vector4.x, vector4.y, vector4.z, -vector4.w) * dysonSphereLayer.currentRotation, Vector3.one), dysonShell.material, layer, (Camera)null, 0, (MaterialPropertyBlock)null, false, false);
                            else
                                Graphics.DrawMesh(dysonShell.mesh, Matrix4x4.TRS(pos2, dysonSphereLayer.currentRotation, Vector3.one), dysonShell.material, layer, (Camera)null, 0, (MaterialPropertyBlock)null, false, false);
                            if (PerformanceMonitor.GpuProfilerOn)
                                PerformanceMonitor.RecordGpuWork(EGpuWorkEntry.DysonShell, 1, dysonShell.mesh.vertexCount);
                        }
                    }
                }
            }
            ++____counter;



            return false;


        }
        **/























    }
}
