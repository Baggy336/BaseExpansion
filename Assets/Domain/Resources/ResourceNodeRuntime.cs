using Assets.Core.Resources;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Domain.Resources
{
    public class ResourceNodeRuntime : MonoBehaviour
    {
        [SerializeField]
        public ResourceNode ResourceNodeBase;

        [SerializeField]
        public List<Transform> ResourceHarvestPoints = new List<Transform>();

        public int AvailableResources;

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

        public Vector3 GetAvailableHarvestPoint()
        {
            int randomPoint = Random.Range(0, ResourceHarvestPoints.Count);
            return ResourceHarvestPoints[randomPoint].position;
        }
    }
}
