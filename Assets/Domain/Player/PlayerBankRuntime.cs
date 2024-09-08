using Assets.Controller.Resources.UI;
using Assets.Core.Player;
using Assets.Core.Resources;
using Assets.Domain.Resources;
using Assets.Domain.Resources.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Domain.Player
{
    public class PlayerBankRuntime : MonoBehaviour
    {
        [SerializeField]
        public PlayerBank PlayerResourceBank;

        [SerializeField]
        public ResourceBankUIController ResourceBankUI;

        private List<ResourceBankRuntime> BankedResourcesRuntime { get; set; }

        private void Start()
        {
            InitializeRuntimeBank();
            InitializeUI();
        }

        private void InitializeRuntimeBank()
        {
            BankedResourcesRuntime = new List<ResourceBankRuntime>();

            foreach (ResourceBank resource in PlayerResourceBank.BankedResources)
            {
                BankedResourcesRuntime.Add(new ResourceBankRuntime(resource.ResourceType, resource.ResourceStoredAmount));
            }
        }

        private void InitializeUI()
        {
            foreach (ResourceBankRuntime resourceBank in BankedResourcesRuntime)
            {
                ResourceBankUI.UpdateResourceUI(resourceBank.ResourceType, resourceBank.ResourceStoredAmount);
            }
        }

        public void WithdrawResource(ResourceTypes resourceType, int amount)
        {
            ResourceBankRuntime resourceBank = BankedResourcesRuntime.Where(x => x.ResourceType == resourceType).FirstOrDefault();

            if (resourceBank != null)
            {
                if (resourceBank.HasEnoughResources(amount))
                {
                    resourceBank.WithrawAmount(amount);
                    UpdateUIAmount(resourceBank);
                }
            }
        }

        public void DepositResource(ResourceTypes resourceType, int amount)
        {
            ResourceBankRuntime resourceBank = BankedResourcesRuntime.Where(x => x.ResourceType == resourceType).FirstOrDefault();

            if (resourceBank != null)
            {
                resourceBank.DepositAmount(amount);
                UpdateUIAmount(resourceBank);
            }
        }

        private void UpdateUIAmount(ResourceBankRuntime resourceBank)
        {
            ResourceBankUI.UpdateResourceUI(resourceBank.ResourceType, resourceBank.ResourceStoredAmount);
        }
    }
}
