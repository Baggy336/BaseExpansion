using Assets.Controller.Selection;
using Assets.Controller.Unit;
using Assets.Controller.Unit.UI;
using Assets.Core.Building;
using Assets.Domain.Interfaces;
using UnityEngine;

namespace Assets.Domain.Building
{
    public class BuildingBase : MonoBehaviour, ISelectable, IAttackable
    {
        [SerializeField]
        public HealthUIController HealthUIHandler;

        public BuildingBaseStats BuildingStats { get; set; }

        private BuildingRuntimeStats BuildingRuntimeStats { get; set; }

        public PlayerController OwnerPlayer { get; set; }

        [SerializeField]
        public PlayerController _ownerPlayer;

        public HealthController HealthHandler;

        public virtual void Awake()
        {
            SetPlayerReference(_ownerPlayer);
            HealthHandler = new HealthController();
            BuildingRuntimeStats = new BuildingRuntimeStats(BuildingStats);
        }

        public virtual void TakeDamage(int amount)
        {
            HealthHandler.TakeFromHealthPool(BuildingRuntimeStats, amount);
            HealthUIHandler.UpdateHealthBar(BuildingRuntimeStats.Health);
            if (BuildingRuntimeStats.Health <= 0)
            {
                Destroy(gameObject);
            }
        }

        public void SetPlayerReference(PlayerController player)
        {
            OwnerPlayer = player;
        }
    }
}
