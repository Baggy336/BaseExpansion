using UnityEngine;

namespace Assets.Core.Camera
{
    [CreateAssetMenu(fileName = "CameraSettings", menuName = "ScriptableObjects/CameraSettings", order = 1)]
    public class CameraSettings : ScriptableObject
    {
        public float EdgePanSpeed;
        public float EdgePanBorder;
        public Vector2 EdgePanLimit;
        public float ZoomSpeed;
        public float ZoomMiny;
        public float ZoomMaxy;
    }
}
