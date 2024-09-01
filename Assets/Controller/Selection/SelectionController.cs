using Assets.Domain.Interfaces;
using Domain.Globals;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Controller.Selection
{
    public class SelectionController : MonoBehaviour
    {
        [SerializeField]
        public List<LayerMask> SelectableLayers;

        [SerializeField]
        public LayerMask GroundLayer;

        [SerializeField]
        public List<MonoBehaviour> SelectableObjects;

        public List<ISelectable> SelectedObjects { get; set; } = new List<ISelectable>();

        private LayerMask combinedLayerMasks { get; set; }

        private bool draggingSelection { get; set; }

        private Vector3 dragStartPosition { get; set; }

        private Rect dragSelectionRect { get; set; }

        public void Awake()
        {
            combinedLayerMasks = CombineLayerMasks();
        }

        private LayerMask CombineLayerMasks()
        {
            LayerMask combinedLayerMask = 0;
            foreach (LayerMask layerMask in SelectableLayers)
            {
                combinedLayerMask |= layerMask;
            }
            return combinedLayerMask;
        }

        private void OnGUI()
        {
            if (draggingSelection)
            {
                dragSelectionRect = GroupSelect.GetScreenBox(dragStartPosition, Input.mousePosition);
                GroupSelect.DrawSelectionBox(dragSelectionRect, new Color(0f, 100f, 0f, .25f));
                GroupSelect.DrawBoxBorder(dragSelectionRect, 3, Color.green);
            }
        }

        public void Update()
        {
            HandleSelectionInput();
        }

        private void HandleSelectionInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                dragStartPosition = Input.mousePosition;

                if (HitSomethingInteractable(out ISelectable hitObject))
                {
                    SelectObject(hitObject);
                }
                else
                {
                    draggingSelection = true;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (draggingSelection)
                {
                    SelectUnitsInDragArea();
                }
                dragStartPosition = Vector3.zero;
                draggingSelection = false;
            }
        }

        private bool HitSomethingInteractable(out ISelectable hitObject)
        {
            Ray ray = Camera.main.ScreenPointToRay(dragStartPosition);
            RaycastHit hit;

            Debug.DrawLine(ray.origin, ray.direction);

            if (Physics.Raycast(ray, out hit, 100, combinedLayerMasks))
            {
                ISelectable selectable = hit.collider.GetComponent<ISelectable>(); // TODO: Change this
                if (LayerIsInteractable(hit.collider.gameObject) && selectable != null)
                {
                    hitObject = selectable;
                    return true;
                }
            }
            hitObject = null;
            return false;
        }

        private bool LayerIsInteractable(GameObject obj)
        {
            return (combinedLayerMasks == (combinedLayerMasks | (1 << obj.layer)));
        }

        private void SelectObject(ISelectable hitObject)
        {
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                DeselectAll();
            }
            if(!ObjectExistsInSelection(hitObject))
            {
                SelectedObjects.Add(hitObject);
            }
        }

        private void SelectUnitsInDragArea()
        {
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                DeselectAll();
            }

            foreach (MonoBehaviour obj in SelectableObjects)
            {
                ISelectable selectable = obj as ISelectable;
                if (ObjectIsWithinSelectionBounds(selectable, dragSelectionRect) && LayerIsInteractable(obj.gameObject))
                {
                    if (!ObjectExistsInSelection(selectable))
                    {
                        SelectedObjects.Add(selectable);
                    }
                }
            }
        }

        private bool ObjectIsWithinSelectionBounds(ISelectable obj, Rect selectionRect)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint((obj as MonoBehaviour).transform.position); // TODO: Change this
            return selectionRect.Contains(screenPosition);
        }

        private bool ObjectExistsInSelection(ISelectable selectedObject)
        {
            return SelectedObjects.Contains(selectedObject);
        }

        private void DeselectAll()
        {
            SelectedObjects.Clear();
        }
    }
}
