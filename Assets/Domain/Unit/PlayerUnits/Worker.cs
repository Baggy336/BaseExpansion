using Assets.Controller.Resources;
using Assets.Core.Unit;
using Assets.Core.Unit.Worker;
using Assets.Domain.Building.Economy;
using Assets.Domain.Globals.Enums;
using Assets.Domain.Resources;
using UnityEngine;

namespace Assets.Domain.Unit.PlayerUnits
{
    public class Worker : UnitBase
    {
        [SerializeField]
        public LayerMask ResourceLayer;

        [SerializeField]
        public LayerMask DepotLayer;

        [SerializeField]
        public WorkerBaseStats WorkerStats;

        private ResourceNodeRuntime TargetResourceNode { get; set; }

        private WorkerResourceBank WorkerStoredResources { get; set; }

        private ResourceDepot SelectedResourceDepot { get; set; }

        private WorkerStates WorkerState { get; set; }

        private float TimeSinceLastHarvest { get; set; }

        public override void Awake()
        {
            WorkerStoredResources = new WorkerResourceBank();
            UnitStats = WorkerStats;
            WorkerState = WorkerStates.None;
            base.Awake();
        }

        public override void Update()
        {
            switch (WorkerState)
            {
                case WorkerStates.None:
                    break;
                case WorkerStates.Harvesting:
                    HandleResourceGathering();
                    break;
                case WorkerStates.Depositing:
                    HandleResourceDeposit();
                    break;
            }
            base.Update();
        }

        private void HandleResourceGathering()
        {
            TimeSinceLastHarvest += Time.deltaTime;
            if (TargetResourceNode != null && WithinHarvestDistance() && WithinHarvestInterval(TimeSinceLastHarvest))
            {
                HarvestResource();
                CheckCarryCapacity();
            }
        }

        private bool WithinHarvestDistance()
        {
            return Vector3.Distance(TargetResourceNode.transform.position, transform.position) <= WorkerStats.HarvestDistance;
        }

        private bool WithinHarvestInterval(float timeSinceHarvest)
        {
            return timeSinceHarvest > WorkerStats.HarvestInterval;
        }

        private void HarvestResource()
        {
            ResourceController.Instance.OnResourceHarvest.Invoke(TargetResourceNode, WorkerStats.HarvestYield);
            WorkerStoredResources.ResourceStoredAmount += WorkerStats.HarvestYield;
            TimeSinceLastHarvest = 0f;
        }

        private void CheckCarryCapacity()
        {
            if (WorkerStoredResources.ResourceStoredAmount == WorkerStats.CarryCapacity)
            {
                WorkerState = WorkerStates.Depositing;
                MoveToDepot();
            }
        }

        private void HandleResourceDeposit()
        {
            if (SelectedResourceDepot != null && WithinDepositDistance())
            {
                DepositResourceBank();
                if (TargetResourceNode != null)
                {
                    WorkerState = WorkerStates.Harvesting;
                    MoveToResource();
                }
            }
        }

        private bool WithinDepositDistance()
        {
            return Vector3.Distance(SelectedResourceDepot.transform.position, transform.position) <= WorkerStats.HarvestDistance;
        }

        private void DepositResourceBank()
        {
            SelectedResourceDepot.DepositResource(WorkerStoredResources.ResourceType, WorkerStoredResources.ResourceStoredAmount);
            WorkerStoredResources.ResourceStoredAmount = 0;
        }

        public override void MoveToLocation(Vector3 location)
        {
            if (CheckResourceNodeHit(location))
            {
                WorkerState = WorkerStates.Harvesting;
                MoveToResource();
            }
            else if (CheckResourceDepotHit(location))
            {
                WorkerState = WorkerStates.Depositing;
                MoveToDepot();
            }
            else
            {
                WorkerState = WorkerStates.None;
                base.MoveToLocation(location);
            }
        }

        private bool CheckResourceDepotHit(Vector3 location)
        {
            bool depotHit = false;
            Collider[] hitColliders = Physics.OverlapSphere(location, 1f);
            foreach (Collider hitCollider in hitColliders)
            {
                if (((1 << hitCollider.gameObject.layer) & DepotLayer) != 0)
                {
                    depotHit = true;
                    HandleSelectedDepot(hitCollider);
                    return true;
                }
            }

            if (!depotHit)
            {
                TargetResourceNode = null;
            }
            return false;
        }

        private void HandleSelectedDepot(Collider hitCollider)
        {
            ResourceDepot resourceDepot = hitCollider.gameObject.GetComponent<ResourceDepot>();

            if (resourceDepot != null)
            {
                SelectedResourceDepot = resourceDepot;
            }
        }

        private bool CheckResourceNodeHit(Vector3 location)
        {
            bool resourceNodeHit = false;
            Collider[] hitColliders = Physics.OverlapSphere(location, 1f);
            foreach (Collider hitCollider in hitColliders)
            {
                if (((1 << hitCollider.gameObject.layer) & ResourceLayer) != 0)
                {
                    resourceNodeHit = true;
                    HandleSelectedResource(hitCollider);
                    return true;
                }
            }

            if (!resourceNodeHit)
            {
                TargetResourceNode = null;
            }
            return false;
        }

        private void HandleSelectedResource(Collider hitCollider)
        {
            ResourceNodeRuntime resourceNode = hitCollider.gameObject.GetComponent<ResourceNodeRuntime>();

            if (resourceNode != null)
            {
                if (TargetResourceNode != null)
                {
                    if (resourceNode.ResourceNodeBase.ResourceType != TargetResourceNode.ResourceNodeBase.ResourceType)
                    {
                        WorkerStoredResources.ResourceStoredAmount = 0;
                    }
                }
                WorkerStoredResources.ResourceType = resourceNode.ResourceNodeBase.ResourceType;
                TargetResourceNode = resourceNode;
            }
        }

        private void MoveToResource()
        {
            base.MoveToLocation(TargetResourceNode.GetAvailableHarvestPoint());
        }

        private void MoveToDepot()
        {
            if (SelectedResourceDepot == null)
            {
                UpdateNearestDepot();
            }

            if (SelectedResourceDepot != null)
            {
                base.MoveToLocation(SelectedResourceDepot.DepositLocation.position);
            }
        }

        private void UpdateNearestDepot()
        {
            ResourceDepot[] availableDepots = FindObjectsOfType<ResourceDepot>();
            float nearestDistance = float.MaxValue;

            foreach (ResourceDepot depot in availableDepots)
            {
                float distanceToDepot = Vector3.Distance(depot.transform.position, transform.position);

                if (distanceToDepot < nearestDistance)
                {
                    nearestDistance = distanceToDepot;
                    SelectedResourceDepot = depot;
                }
            }
        }
    }
}
