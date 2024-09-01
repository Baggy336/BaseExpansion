using Assets.Controller.Resources.Events;
using Assets.Domain.Resources;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Controller.Resources
{
    public class ResourceController : MonoBehaviour
    {
        public static ResourceController Instance { get; private set; }

        [SerializeField]
        private List<ResourceNodeRuntime> ResourceNodesRuntimeStats;

        private HarvestResourceEvent OnResourceHarvest { get; set; }

        public void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Start()
        {
            SetUpHarvestEvent();
        }

        private void SetUpHarvestEvent()
        {
            if (OnResourceHarvest == null)
            {
                OnResourceHarvest = new HarvestResourceEvent();
            }
            OnResourceHarvest.AddListener(HarvestFromResourceNode);
        }

        public void HarvestFromResourceNode(ResourceNodeRuntime node, int amount)
        {
            node.Harvest(amount);
            if(node.IsDepleted())
            {
                DestroyResourceNode(node);
            }
        }

        private void DestroyResourceNode(ResourceNodeRuntime node)
        {
            Destroy(node.gameObject);
            ResourceNodesRuntimeStats.Remove(node);
        }
    }
}
