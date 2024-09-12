using Assets.Domain.Globals.Enums;
using Domain.Globals;
using System;
using UnityEngine;

namespace Assets.Controller.Player
{
    public class InputController : MonoBehaviour
    {
        [SerializeField]
        public LayerMask GroundLayer;

        [SerializeField]
        public MouseButton RightClick = MouseButton.Right;

        [SerializeField]
        public MouseButton LeftClick = MouseButton.Left;

        [SerializeField]
        public float DragThreshold = 10f;

        public Action<Vector3> IssuedRightClick { get; set; }

        public Action<Vector3> IssuedLeftClick { get; set; }

        public Action<KeyCode> IssuedKeycodeCommand { get; set; }

        public Action<Rect> IssuedDragSelection { get; set; }

        private bool DraggingSelection { get; set; }

        private Vector3 DragStartPosition { get; set; }

        private Rect DragSelectionRect { get; set; }

        public void Update()
        {
            CheckLeftClick();
            CheckRightClick();
            HandleDragSelection();
            CheckKeyInput();
        }

        private void OnGUI()
        {
            if (DraggingSelection)
            {
                DragSelectionRect = GroupSelect.GetScreenBox(DragStartPosition, Input.mousePosition);
                GroupSelect.DrawSelectionBox(DragSelectionRect, new Color(0f, 100f, 0f, .25f));
                GroupSelect.DrawBoxBorder(DragSelectionRect, 3, Color.green);
            }
        }

        private void CheckLeftClick()
        {
            if (Input.GetMouseButtonUp((int)LeftClick))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                {
                    Vector3 destination = hit.point;
                    if (!DraggingSelection)
                    {
                        IssueLeftClick(destination);
                    }
                }
            }
        }

        private void IssueLeftClick(Vector3 destination) => IssuedLeftClick?.Invoke(destination);

        private void CheckRightClick()
        {
            if (Input.GetMouseButtonDown((int)RightClick))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                {
                    Vector3 destination = hit.point;
                    IssueRightClick(destination);
                }
            }
        }

        private void IssueRightClick(Vector3 destination) => IssuedRightClick?.Invoke(destination);

        private void CheckKeyInput()
        {
            if (Input.anyKeyDown)
            {
                string inputString = Input.inputString;
                if (!string.IsNullOrEmpty(inputString))
                {
                    KeyCode pressedKey = (KeyCode)Enum.Parse(typeof(KeyCode), inputString.ToUpper());
                    IssueKeycodeCommand(pressedKey);
                }
            }
        }

        private void IssueKeycodeCommand(KeyCode key) => IssuedKeycodeCommand?.Invoke(key);

        private void HandleDragSelection()
        {
            if (Input.GetMouseButtonDown((int)LeftClick))
            {
                DragStartPosition = Input.mousePosition;
            }

            if (Input.GetMouseButton((int)LeftClick))
            {
                if (Vector3.Distance(DragStartPosition, Input.mousePosition) > DragThreshold)
                {
                    DraggingSelection = true;
                }
            }

            if (Input.GetMouseButtonUp((int)LeftClick))
            {
                if (DraggingSelection)
                {
                    IssueDragSelect(DragSelectionRect);
                }
                DragStartPosition = Vector3.zero;
                DraggingSelection = false;
            }

            if (DraggingSelection)
            {
                DragSelectionRect = GroupSelect.GetScreenBox(DragStartPosition, Input.mousePosition);
            }
        }

         private void IssueDragSelect(Rect bounds) => IssuedDragSelection?.Invoke(bounds);
    }
}
