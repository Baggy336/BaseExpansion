using Assets.Controller.Building.UI;
using Assets.Core;
using Assets.Domain.Interfaces;
using Assets.Domain.Player;
using Assets.Domain.Unit;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Domain.Building.Economy
{
    public class ProductionBuildingBase : BuildingBase, IConstruction
    {
        [SerializeField]
        public List<ConstructionCost> BuildableObjects;

        [SerializeField]
        public PlayerBankRuntime PlayerBank;

        [SerializeField]
        public ProductionUIController ProductionUI;

        [SerializeField]
        public Transform ConstructionLocation;

        [SerializeField]
        public PlayerController OwnerPlayer;

        [SerializeField]
        public int ConstructionQueueMaxLength = 4;

        [SerializeField]
        private UnitFactory UnitFactory;

        private Queue<ConstructionCost> QueuedConstruction { get; set; } = new Queue<ConstructionCost>();

        private float ConstructionTimer { get; set; } = 0f;

        public virtual void Update()
        {
            HandleConstructionQueue();
        }

        private void HandleConstructionQueue()
        {
            if (QueuedConstruction.Count > 0)
            {
                ProductionUI.SetProgressBarActive(QueuedConstruction.Peek().ConstructionTime);
                HandleConstructionTimer();
                if (ConstructionTimer <= 0f)
                {
                    BuildObjectFromQueue();
                }
            }
            else
            {
                ProductionUI.SetProgressBarInactive();
            }
        }

        private void HandleConstructionTimer()
        {
            ConstructionTimer -= Time.deltaTime;
            ProductionUI.UpdateProgressBar(ConstructionTimer);
        }

        private void BuildObjectFromQueue()
        {
            ConstructionCost task = QueuedConstruction.Dequeue();
            UnitFactory.CreateUnit(task.ConstructedObject, ConstructionLocation.position, OwnerPlayer);

            if (QueuedConstruction.Count > 0)
            {
                ConstructionTimer = QueuedConstruction.Peek().ConstructionTime;
            }
        }

        private void AddObjectToConstructionQueue(ConstructionCost constructionTask)
        {
            QueuedConstruction.Enqueue(constructionTask);
            if (QueuedConstruction.Count == 1)
            {
                ConstructionTimer = QueuedConstruction.Peek().ConstructionTime;
            }
        }

        public void CheckConstructionHotkey(KeyCode keyCode)
        {
            foreach (ConstructionCost construction in BuildableObjects)
            {
                if (construction.Hotkey == keyCode)
                {
                    TryToConstructObject(construction);
                }
            }
        }

        private void TryToConstructObject(ConstructionCost construction)
        {
            if(QueuedConstruction.Count < ConstructionQueueMaxLength)
            {
                if (CheckPlayerHasResources(construction))
                {
                    AddObjectToConstructionQueue(construction);
                }
            }
        }

        private bool CheckPlayerHasResources(ConstructionCost construction)
        {
            return PlayerBank.TryWithdrawResource(construction.CostToConstruct);
        }
    }
}
