using Assets.Core.Events;
using Assets.Domain.Building.Economy;
using Assets.Domain.Interfaces;
using Assets.Domain.Resources;
using System.Collections.Generic;
using UnityEngine;
using Assets.Domain.Globals.Enums;
using Assets.Controller.Selection;
using Assets.Domain.Unit.Commands;
using Assets.Domain.Building.Commands;
using System.Linq;
using System;

namespace Assets.Controller.Player
{
    public class CommandController : MonoBehaviour
    {
        [SerializeField]
        public float OverlapRadius = 1f;

        [SerializeField]
        public SelectionController SelectionManager;

        [SerializeField] 
        private BuildingPlacementController BuildingPlacementManager;

        [SerializeField]
        public LayerMask GroundLayer;

        private List<ITypeInputAction> InteractionHandlers { get; set; }

        private List<LayerActions> LayerHandlers { get; set; }

        private List<IKeyAction> KeyHandlers { get; set; }


        public void Awake()
        {
            InteractionHandlers = new List<ITypeInputAction>()
            {
                new TypeInputAction<ISelectable>(MouseButton.Left, HandleSelectable),
                new TypeInputAction<IAttackable>(MouseButton.Right, HandleAttackable),
                new TypeInputAction<ResourceNodeRuntime>(MouseButton.Right, HandleResourceNode),
                new TypeInputAction<ResourceDepot>(MouseButton.Right, HandleResourceDepot)
            };

            LayerHandlers = new List<LayerActions>()
            {
                new LayerActions(GroundLayer, HandleMovement)
            };

            KeyHandlers = new List<IKeyAction>()
            {
                new KeyActions<IConstruction>(HandleConstructionHotkey)
            };
        }

        public void ProcessRightClickHit(Vector3 location)
        {
            if (TryGetHitObject(location, out GameObject hitObject))
            {
                foreach (LayerActions action in LayerHandlers)
                {
                    if (action.InteractionLayer == GroundLayer)
                    {
                        action.MethodToInvoke.Invoke(location);
                    }
                }
                foreach (ITypeInputAction action in InteractionHandlers)
                {
                    Type componentType = action.GetType().GetGenericArguments()[0];
                    if (hitObject.TryGetComponent(componentType, out Component component) && action.ClickType == MouseButton.Right)
                    {
                        action.Invoke(hitObject, component);
                    }
                }
            }
        }

        public void ProcessLeftClickHit(Vector3 location)
        {
            if (TryGetHitObject(location, out GameObject hitObject))
            {
                foreach (ITypeInputAction action in InteractionHandlers)
                {
                    Type componentType = action.GetType().GetGenericArguments()[0];
                    if (hitObject.TryGetComponent(componentType, out Component component) && action.ClickType == MouseButton.Left)
                    {
                        action.Invoke(hitObject, component);
                        return;
                    }
                }
            }
            else if(BuildingPlacementManager.PlacingBuilding)
            {
                SetBuildingPlacement(location);
            }
        }

        private bool TryGetHitObject(Vector3 location, out GameObject hitObject)
        {
            Collider[] hitColliders = Physics.OverlapSphere(location, OverlapRadius);
            foreach (Collider collider in hitColliders)
            {
                if (collider.gameObject != null)
                {
                    hitObject = collider.gameObject;
                    return true;
                }
            }
            hitObject = null;
            return false;
        }

        private void HandleSelectable(GameObject hitObject, Component selectableComponent)
        {
            if (selectableComponent is ISelectable selectable)
            {
                SelectionManager.HandleSelectable(selectable);
            }
        }

        private void SetBuildingPlacement(Vector3 loaction)
        {
            BuildingPlacementManager.ConfirmBuildingPlacement(loaction);
        }

        private void HandleAttackable(GameObject hitObject, Component attackableComponent)
        {
            if (attackableComponent is IAttackable attackable)
            {
                if (SelectionManager.CheckObjectExistsInSelectable(attackable) == false)
                {
                    foreach (ISelectable selected in SelectionManager.SelectedObjects)
                    {
                        if (selected is ICombatAble combat)
                        {
                            ICommand attackCommand = new AttackCommand(combat, attackable);
                            attackCommand.Execute();
                        }
                    }
                }
            }
        }

        private void HandleResourceNode(GameObject hitObject, Component resourceNodeComponent)
        {
            if (resourceNodeComponent is ResourceNodeRuntime resourceNode)
            {
                foreach (ISelectable selected in SelectionManager.SelectedObjects)
                {
                    if (selected is IResourceCollector collector)
                    {
                        ICommand gatherCommand = new GatherResourceCommand(resourceNode, collector);
                        gatherCommand.Execute();
                    }
                }
            }
        }

        private void HandleResourceDepot(GameObject hitObject, Component resourceDepotComponent)
        {
            if (resourceDepotComponent is ResourceDepot depot)
            {
                foreach (ISelectable selected in SelectionManager.SelectedObjects)
                {
                    if (selected is IResourceCollector collector)
                    {
                        ICommand depositCommand = new DepositResourceCommand(depot, collector);
                        depositCommand.Execute();
                    }
                }
            }
        }

        private void HandleMovement(Vector3 hitLocation)
        {
            foreach (ISelectable selected in SelectionManager.SelectedObjects)
            {
                if (selected is IMoveable moveable)
                {
                    ICommand moveCommand = new MoveCommand(hitLocation, moveable);
                    moveCommand.Execute();
                }
            }
        }

        public void ProcessKeyStroke(KeyCode key)
        {
            foreach (IKeyAction action in KeyHandlers)
            {
                Type actionType = action.GetType().GetGenericArguments()[0];

                List<ISelectable> selectedObjectsToRespondToKey = SelectionManager.SelectableObjects.Where(x => actionType.IsAssignableFrom(x.GetType())).ToList();

                foreach (ISelectable selectedObject in selectedObjectsToRespondToKey)
                {
                    action.Invoke(key, selectedObject);
                    return;
                }
            }
        }

        private void HandleConstructionHotkey(KeyCode key, ISelectable selectedObject)
        {
            if (selectedObject is IConstruction construction)
            {
                ICommand constructionCommand = new ConstructionCommand(construction, key);
                constructionCommand.Execute();
            }
        }

        public void ProcessDragSelection(Rect bounds)
        {
            SelectionManager.SelectObjectsInBounds(bounds);
        }
    }
}
