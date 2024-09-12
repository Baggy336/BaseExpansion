using Assets.Controller;
using Assets.Controller.Resources;
using Assets.Core.Unit;
using Assets.Core.Unit.Worker;
using Assets.Domain.Building.Economy;
using Assets.Domain.Globals.Enums;
using Assets.Domain.Interfaces;
using Assets.Domain.Resources;
using UnityEngine;

namespace Assets.Domain.Unit.PlayerUnits
{
    public class Worker : UnitBase, IResourceCollector
    {
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
            if (TargetResourceNode != null && WithinHarvestDistance() && WithinHarvestInterval(TimeSinceLastHarvest) && WorkerHasStorageCapacity())
            {
                HarvestResource();
            }
            CheckCarryCapacity();
        }

        private bool WithinHarvestDistance()
        {
            return Vector3.Distance(TargetResourceNode.transform.position, transform.position) <= WorkerStats.HarvestDistance;
        }

        private bool WithinHarvestInterval(float timeSinceHarvest)
        {
            return timeSinceHarvest > WorkerStats.HarvestInterval;
        }

        private bool WorkerHasStorageCapacity()
        {
            return WorkerStoredResources.ResourceStoredAmount < WorkerStats.CarryCapacity;
        }

        private void HarvestResource()
        {
            if(WorkerStoredResources.ResourceType != TargetResourceNode.ResourceNodeBase.ResourceType)
            {
                WorkerStoredResources.ResourceType = TargetResourceNode.ResourceNodeBase.ResourceType;
                WorkerStoredResources.ResourceStoredAmount = 0;
            }
            ResourceController.Instance.OnResourceHarvest.Invoke(TargetResourceNode, WorkerStats.HarvestYield);
            WorkerStoredResources.ResourceStoredAmount += WorkerStats.HarvestYield;
            TimeSinceLastHarvest = 0f;
        }

        private void CheckCarryCapacity()
        {
            if (WorkerStoredResources.ResourceStoredAmount >= WorkerStats.CarryCapacity
             || WorkerStoredResources.ResourceStoredAmount + WorkerStats.HarvestYield > WorkerStats.CarryCapacity)
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
            WorkerState = WorkerStates.None;
            base.MoveToLocation(location);
        }

        private void MoveToResource()
        {
            base.MoveToLocation(TargetResourceNode.GetAvailableHarvestPoint());
        }

        private void MoveToDepot()
        {
            if (SelectedResourceDepot == null)
            {
                SelectedResourceDepot = GameController.Instance.GetPlayerEventSystem(OwnerPlayer).InvokeResourceCollectorNeedsDepot(this, transform.position, OwnerPlayer);
            }
            base.MoveToLocation(SelectedResourceDepot.DepositLocation.position);
        }

        public void SetTargetResource(ResourceNodeRuntime resourceNode)
        {
            TargetResourceNode = resourceNode;
            WorkerState = WorkerStates.Harvesting;
            MoveToResource();
        }

        public void SetDepositPoint(ResourceDepot resourceDepot)
        {
            SelectedResourceDepot = resourceDepot;
            WorkerState = WorkerStates.Depositing;
            MoveToDepot();
        }
    }
}
