using Assets.Domain.Building.Economy;
using Assets.Domain.Interfaces;
using Domain.Globals;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Controller.Selection
{
    public class SelectionController : MonoBehaviour
    {
        [SerializeField]
        public List<MonoBehaviour> SelectableMonobehaviors = new List<MonoBehaviour>();

        public List<ISelectable> SelectableObjects = new List<ISelectable>();

        public List<ISelectable> SelectedObjects { get; set; } = new List<ISelectable>();

        public void Awake()
        {
            foreach (MonoBehaviour behavior in SelectableMonobehaviors)
            {
                SelectableObjects.Add(behavior as ISelectable);
            }
        }

        public void HandleSelectable(ISelectable hitObject)
        {
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                DeselectAll();
            }
            if (!ObjectExistsInSelection(hitObject))
            {
                SelectedObjects.Add(hitObject);
            }
        }

        public void SelectObjectsInBounds(Rect bounds)
        {
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                DeselectAll();
            }

            foreach (ISelectable selectable in SelectableObjects)
            {
                if (ObjectIsWithinSelectionBounds(selectable, bounds))
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

        public void RemoveSelectableObject(ISelectable selectable)
        {
            SelectableObjects.Remove(selectable);
            if (SelectedObjects.Contains(selectable))
            {
                SelectedObjects.Remove(selectable);
            }
        }

        public void AddSelectableToList(ISelectable selectable)
        {
            if (!SelectableObjects.Contains(selectable))
            {
                SelectableObjects.Add(selectable);
            }
        }

        public bool CheckObjectExistsInSelectable<T>(T type)
        {
            if (type is ISelectable selectable)
            {
                return SelectableObjects.Contains(selectable);
            }
            return false;
        }

        public ResourceDepot FindNearestResourceDepot(Vector3 workerPosition)
        {
            (ResourceDepot, float) closestDepot = (null, float.MaxValue);
            foreach (ISelectable selectable in SelectableObjects)
            {
                if (selectable is ResourceDepot depot)
                {
                    float distanceToWorker = Vector3.Distance(workerPosition, depot.transform.position);
                    if (distanceToWorker < closestDepot.Item2)
                    {
                        closestDepot = (depot, distanceToWorker);
                    }
                }
            }
            return closestDepot.Item1;
        }
    }
}
