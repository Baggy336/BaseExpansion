using Assets.Core.Camera;
using UnityEngine;

namespace Assets.Domain.Camera
{
    public class CameraRuntimeSettings
    {
        public float EdgePanSpeed { get; set; }
        public float EdgePanBorder { get; set; }
        public Vector2 EdgePanLimit { get; set; }
        public float ZoomSpeed { get; set; }
        public float ZoomMiny { get; set; }
        public float ZoomMaxy { get; set; }

        public CameraRuntimeSettings(CameraSettings camSettings)
        {
            EdgePanSpeed = camSettings.EdgePanSpeed;
            EdgePanBorder = camSettings.EdgePanBorder;
            EdgePanLimit = camSettings.EdgePanLimit;
            ZoomSpeed = camSettings.ZoomSpeed;
            ZoomMiny = camSettings.ZoomMiny;
            ZoomMaxy = camSettings.ZoomMaxy;
        }
    }
}
