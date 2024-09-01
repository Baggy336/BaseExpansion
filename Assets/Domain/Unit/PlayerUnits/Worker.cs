using Assets.Controller.Resources;
using Assets.Core.Unit;
using Assets.Core.Unit.Worker;
using Assets.Domain.Resources;
using UnityEngine;

namespace Assets.Domain.Unit.PlayerUnits
{
    public class Worker : UnitBase
    {
        [SerializeField]
        public LayerMask ResourceLayer;

        [SerializeField]
        public WorkerBaseStats WorkerStats;

        private ResourceNodeRuntime TargetResourceNode { get; set; }

        private WorkerResourceBank WorkerStoredResources { get; set; }

        public override void Awake()
        {
            WorkerStoredResources = new WorkerResourceBank();
            UnitStats = WorkerStats;
            base.Awake();
        }

        public override void MoveToLocation(Vector3 location)
        {
            CheckResourceNodeHit(location);

            if (TargetResourceNode != null)
            {
                MoveToResource();
            }
            else
            {
                base.MoveToLocation(location);
            }
        }

        private void CheckResourceNodeHit(Vector3 location)
        {
            bool resourceNodeHit = false;
            Collider[] hitColliders = Physics.OverlapSphere(location, 1f);
            foreach (Collider hitCollider in hitColliders)
            {
                if (((1 << hitCollider.gameObject.layer) & ResourceLayer) != 0)
                {
                    resourceNodeHit = true;
                    HandleSelectedResource(hitCollider);
                }
            }

            if (!resourceNodeHit)
            {
                TargetResourceNode = null;
            }
        }

        private void HandleSelectedResource(Collider hitCollider)
        {
            ResourceNodeRuntime resourceNode = hitCollider.gameObject.GetComponent<ResourceNodeRuntime>();

            if (resourceNode != null)
            {
                WorkerStoredResources.ResourceType = resourceNode.ResourceNodeBase.ResourceType;
                TargetResourceNode = resourceNode;
            }
        }

        private void MoveToResource()
        {
            base.MoveToLocation(TargetResourceNode.GetAvailableHarvestPoint());

            ResourceController.Instance.OnResourceHarvest.Invoke(TargetResourceNode, 10);
        }
    }
}
