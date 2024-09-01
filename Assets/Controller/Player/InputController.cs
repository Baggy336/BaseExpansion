using Assets.Domain.Globals.Enums;
using System;
using UnityEngine;

namespace Assets.Controller.Player
{
    public class InputController : MonoBehaviour
    {
        [SerializeField]
        public LayerMask GroundLayer;

        [SerializeField]
        public MouseButton MovementButton = MouseButton.Right;

        public Action<Vector3> IssuedMovementCommand { get; set; }

        public void Update()
        {
            CheckMovementInput();
        }

        private void CheckMovementInput()
        {
            if(Input.GetMouseButtonDown((int)MovementButton))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, GroundLayer))
                {
                    Vector3 destination = hit.point;
                    IssueMovementCommand(destination);
                }
            }
        }

        private void IssueMovementCommand(Vector3 destination)
        {
            IssuedMovementCommand?.Invoke(destination);
        }
    }
}
