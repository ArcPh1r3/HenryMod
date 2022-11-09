using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HenryMod.Modules.Components.Bundled {
    public class ReRigger : MonoBehaviour {

        [SerializeField]
        private ReRigGroup[] overrides;

        void LateUpdate() {
            for (int i = 0; i < overrides.Length; i++) {
                overrides[i].UpdatePosition();
            }
        }

        [System.Serializable]
        public class ReRigGroup {
            public Transform transform;
            public Vector3 localPositionOverride;

            public void UpdatePosition() {
                transform.localPosition = localPositionOverride;
            }
        }


        [ContextMenu("Set Local Positions from Transform")]
        void AutomaticallySetLocalPositions() {

            for (int i = 0; i < overrides.Length; i++) {
                if (!overrides[i].transform)
                    continue;

                overrides[i].localPositionOverride = overrides[i].transform.localPosition;
            }
        }


        [ContextMenu("Update Bone Positions")]
        void UpdateBonePositions() {

            for (int i = 0; i < overrides.Length; i++) {
                overrides[i].UpdatePosition();
            }
        }
    }
}