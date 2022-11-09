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
    }
}