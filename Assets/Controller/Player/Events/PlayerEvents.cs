using Assets.Domain.Building.Economy;
using Assets.Domain.Interfaces;
using System;
using UnityEngine;

namespace Assets.Controller.Player.Events
{
    public class PlayerEvents
    {
        public event Action<ISelectable, PlayerController> OnSelectableCreated;
        public event Func<IResourceCollector, Vector3, PlayerController, ResourceDepot> ResourceCollectorNeedsDepot; 

        public void InvokeSelectableCreated(ISelectable selectable, PlayerController player)
        {
            OnSelectableCreated?.Invoke(selectable, player);
        }
        
        public ResourceDepot InvokeResourceCollectorNeedsDepot(IResourceCollector worker, Vector3 workerPosition, PlayerController player)
        {
            return ResourceCollectorNeedsDepot?.Invoke(worker, workerPosition, player);
        }
    }
}
