using Assets.Core.Building;
using Assets.Domain.Building.Economy;
using Assets.Domain.Interfaces;
using Assets.Domain.Unit.PlayerUnits;
using System;
using UnityEngine;

namespace Assets.Controller.Player.Events
{
    public class PlayerEvents
    {
        public event Action<ISelectable, PlayerController> OnSelectableCreated;
        public event Func<IResourceCollector, Vector3, PlayerController, ResourceDepot> ResourceCollectorNeedsDepot;
        public event Action<Worker, PlayerController, BuildingConstructionCost> OnBuildingPlacementStarted;

        public void InvokeSelectableCreated(ISelectable selectable, PlayerController player)
        {
            OnSelectableCreated?.Invoke(selectable, player);
        }
        
        public ResourceDepot InvokeResourceCollectorNeedsDepot(IResourceCollector worker, Vector3 workerPosition, PlayerController player)
        {
            return ResourceCollectorNeedsDepot?.Invoke(worker, workerPosition, player);
        }

        public void InvokeBuildingPlacement(Worker worker, PlayerController player, BuildingConstructionCost building)
        {
            OnBuildingPlacementStarted?.Invoke(worker, player, building);
        }
    }
}
