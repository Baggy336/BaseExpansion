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
        public List<GameObject> SelectableObjects;

        public List<GameObject> SelectedObjects { get; set; } = new List<GameObject>();

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

                if (HitSomethingInteractable(out GameObject hitObject))
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

        private bool HitSomethingInteractable(out GameObject hitObject)
        {
            Ray ray = Camera.main.ScreenPointToRay(dragStartPosition);
            RaycastHit hit;

            Debug.DrawLine(ray.origin, ray.direction);

            if (Physics.Raycast(ray, out hit, 100, combinedLayerMasks))
            {
                if (LayerIsInteractable(hit.collider.gameObject) && ObjectIsSelectable(hit.collider.gameObject))
                {
                    hitObject = hit.collider.gameObject;
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

        private bool ObjectIsSelectable(GameObject hitObject)
        {
            return SelectableObjects.Contains(hitObject);
        }

        private void SelectObject(GameObject hitObject)
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

            foreach (GameObject obj in SelectableObjects)
            {
                if (ObjectIsWithinSelectionBounds(obj, dragSelectionRect) && LayerIsInteractable(obj))
                {
                    if (!SelectedObjects.Contains(obj))
                    {
                        SelectedObjects.Add(obj);
                    }
                }
            }
        }
        private bool ObjectIsWithinSelectionBounds(GameObject obj, Rect selectionRect)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(obj.transform.position);
            return selectionRect.Contains(screenPosition);
        }

        private bool ObjectExistsInSelection(GameObject selectedObject)
        {
            return SelectedObjects.Contains(selectedObject);
        }

        private void DeselectAll()
        {
            SelectedObjects.Clear();
        }
    }
}
