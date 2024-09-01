using Assets.Core.Resources;
using UnityEngine;

namespace Assets.Domain.Resources
{
    public class ResourceNodeRuntime : MonoBehaviour
    {
        [SerializeField]
        public ResourceNode ResourceNodeBase;

        public int AvailableResources { get; set; }

        public void Start()
        {
            InitializeStats();
        }

        private void InitializeStats()
        {
            AvailableResources = ResourceNodeBase.BaseYield;
        }

        public void Harvest(int amount)
        {
            AvailableResources -= amount;
        }

        public bool IsDepleted()
        {
            return AvailableResources <= 0;
        }
    }
}
