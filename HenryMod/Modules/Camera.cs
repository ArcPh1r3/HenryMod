using RoR2;
using System;
using UnityEngine;
using static RoR2.CameraTargetParams;

namespace HenryMod.Modules {

    internal static class Camera {

        public enum ParamsType {
            NONE,
            ZOOM_IN,
            EMOTE,
            BAZOOKA,
        }

        private static CharacterCameraParamsData bazookaCameraParams = CreateParamsData(new Vector3(1.1f, -0.8f, -7.5f));

        private static CameraParamsOverrideHandle camOverrideHandle;
                                                                                                 //presto
        private static CharacterCameraParamsData CreateParamsData (Vector3 position, float vert = 1.37f) {

            return new CharacterCameraParamsData() {
                maxPitch = 70,
                minPitch = -70,
                pivotVerticalOffset = vert,
                idealLocalCameraPos = position,
                wallCushion = 0.1f,
            };

        }

        public static void ActivateCameraParamsOverride (CameraTargetParams cameraTargetParams, ParamsType paramsType) {

            CharacterCameraParamsData cameraParameters;

            switch (paramsType) {
                default:
                case ParamsType.NONE:
                    DeactivateCameraParamsOverrides(cameraTargetParams);
                    return;
                //case ParamsType.ZOOM_IN:
                //    break;
                //case ParamsType.EMOTE:
                //    break;
                case ParamsType.BAZOOKA:
                    cameraParameters = bazookaCameraParams;
                    break;
            }

            CameraParamsOverrideRequest request = new CameraParamsOverrideRequest {
                cameraParamsData = cameraParameters,
                priority = 0,
            };
            camOverrideHandle = cameraTargetParams.AddParamsOverride(request, 0.6f);
        }

        public static void DeactivateCameraParamsOverrides(CameraTargetParams cameraTargetParams) {

            for (int i = cameraTargetParams.cameraParamsOverrides.Count - 1; i >= 0; i--) {

                camOverrideHandle.target = cameraTargetParams.cameraParamsOverrides[i];

                cameraTargetParams.RemoveParamsOverride(camOverrideHandle, 0.3f);
            }
        }

        //internal static CharacterCameraParams defaultCameraParams;
        //internal static CharacterCameraParams zoomInCameraParams;
        //internal static CharacterCameraParams emoteCameraParams;

        //[Obsolete("new camera system doesn't use cameraparams directly")]
        //private static CharacterCameraParams NewCameraParams(string name, float pitch, float pivotVerticalOffset, Vector3 standardPosition) {
        //    return NewCameraParams(name, pitch, pivotVerticalOffset, standardPosition, 0.1f);
        //}
        //[Obsolete("new camera system doesn't use cameraparams directly")]
        //private static CharacterCameraParams NewCameraParams(string name, float pitch, float pivotVerticalOffset, Vector3 standardPosition, float wallCushion) {
        //    CharacterCameraParams newParams = ScriptableObject.CreateInstance<CharacterCameraParams>();

        //    newParams.data.maxPitch = pitch;
        //    newParams.data.minPitch = -pitch;
        //    newParams.data.pivotVerticalOffset = pivotVerticalOffset;
        //    newParams.data.idealLocalCameraPos = standardPosition;
        //    newParams.data.wallCushion = wallCushion;

        //    return newParams;
        //}
    }
}