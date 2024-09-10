using Assets.Core.Camera;
using Assets.Domain.Camera;
using UnityEngine;

namespace Assets.Controller.GameView
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private CameraSettings CameraValues;

        private CameraRuntimeSettings CameraRuntime { get; set; }

        private void Awake()
        {
            CameraRuntime = new CameraRuntimeSettings(CameraValues);
        }

        private void Update()
        {
            CheckMouseEdgePan();
            CheckMouseZoom();
        }

        private void CheckMouseEdgePan()
        {
            Vector3 pos = transform.position;
            bool shouldPan = false;

            float panSpeed = CameraRuntime.EdgePanSpeed;

            if (Input.mousePosition.y >= Screen.height - CameraRuntime.EdgePanBorder)
            {
                float distance = Screen.height - Input.mousePosition.y;
                panSpeed = Mathf.Lerp(CameraRuntime.EdgePanSpeed, 0, distance / CameraRuntime.EdgePanBorder);
                pos.z += panSpeed * Time.deltaTime;
                shouldPan = true;
            }
            if (Input.mousePosition.y <= CameraRuntime.EdgePanBorder)
            {
                float distance = Input.mousePosition.y;
                panSpeed = Mathf.Lerp(CameraRuntime.EdgePanSpeed, 0, distance / CameraRuntime.EdgePanBorder);
                pos.z -= panSpeed * Time.deltaTime;
                shouldPan = true;
            }
            if (Input.mousePosition.x >= Screen.width - CameraRuntime.EdgePanBorder)
            {
                float distance = Screen.width - Input.mousePosition.x;
                panSpeed = Mathf.Lerp(CameraRuntime.EdgePanSpeed, 0, distance / CameraRuntime.EdgePanBorder);
                pos.x += panSpeed * Time.deltaTime;
                shouldPan = true;
            }
            if (Input.mousePosition.x <= CameraRuntime.EdgePanBorder)
            {
                float distance = Input.mousePosition.x;
                panSpeed = Mathf.Lerp(CameraRuntime.EdgePanSpeed, 0, distance / CameraRuntime.EdgePanBorder);
                pos.x -= panSpeed * Time.deltaTime;
                shouldPan = true;
            }

            if (shouldPan)
            {
                pos.x = Mathf.Clamp(pos.x, -CameraRuntime.EdgePanLimit.x, CameraRuntime.EdgePanLimit.x);
                pos.z = Mathf.Clamp(pos.z, -CameraRuntime.EdgePanLimit.y, CameraRuntime.EdgePanLimit.y);

                PanCamera(pos);
            }
        }

        private void CheckMouseZoom()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (Mathf.Abs(scroll) > Mathf.Epsilon)
            {
                Vector3 pos = transform.position;

                pos.y -= scroll * CameraRuntime.ZoomSpeed * 100f * Time.deltaTime;
                pos.y = Mathf.Clamp(pos.y, CameraRuntime.ZoomMiny, CameraRuntime.ZoomMaxy);

                ZoomCamera(pos.y);
            }
        }

        private void PanCamera(Vector3 destination)
        {
            transform.position = Vector3.Lerp(transform.position, destination, CameraRuntime.EdgePanSpeed * Time.deltaTime);
        }

        private void ZoomCamera(float zoom)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, zoom, CameraRuntime.ZoomSpeed * Time.deltaTime), transform.position.z);
        }
    }
}

