using Assets.Core.Building.Economy;
using Assets.Domain.Player;
using Assets.Domain.Resources.Enums;
using UnityEngine;

namespace Assets.Domain.Building.Economy
{
    public class ResourceDepot : BuildingBase
    {
        [SerializeField]
        public ResourceDepotBaseStats ResourceDepotStats;

        [SerializeField]
        public Transform DepositLocation;

        [SerializeField]
        public PlayerBankRuntime PlayerBank;

        public override void Awake()
        {
            BuildingStats = ResourceDepotStats;
            base.Awake();
        }

        public void DepositResource(ResourceTypes resourceType, int amount)
        {
            PlayerBank.DepositResource(resourceType, amount);
        }
    }
}
