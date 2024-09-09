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
        public SelectionController SelectionHandler;
        
        [SerializeField]
        public HealthUIController HealthUIHandler;

        public BuildingBaseStats BuildingStats { get; set; }

        private BuildingRuntimeStats BuildingRuntimeStats { get; set; }

        public HealthController HealthHandler;

        public virtual void Awake()
        {
            HealthHandler = new HealthController();
            BuildingRuntimeStats = new BuildingRuntimeStats(BuildingStats);
        }

        public virtual void TakeDamage(int amount)
        {
            HealthHandler.TakeFromHealthPool(BuildingRuntimeStats, amount);
            HealthUIHandler.UpdateHealthBar(BuildingRuntimeStats.Health);
            if (BuildingRuntimeStats.Health <= 0)
            {
                SelectionHandler.RemoveSelectableObject(this);
                Destroy(gameObject);
            }
        }
    }
}
