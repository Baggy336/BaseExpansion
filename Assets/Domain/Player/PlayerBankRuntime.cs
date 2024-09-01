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

        private List<ResourceBankRuntime> BankedResourcesRuntime { get; set; }

        private void Start()
        {
            InitializeRuntimeBank();
        }

        private void InitializeRuntimeBank()
        {
            BankedResourcesRuntime = new List<ResourceBankRuntime>();

            foreach(ResourceBank resource in PlayerResourceBank.BankedResources)
            {
                BankedResourcesRuntime.Add(new ResourceBankRuntime(resource.ResourceType, resource.ResourceStoredAmount));
            }
        }

        public void WithdrawResource(ResourceTypes resourceType, int amount)
        {
            ResourceBankRuntime resourceBank = BankedResourcesRuntime.Where(x => x.ResourceType == resourceType).FirstOrDefault();

            if(resourceBank != null)
            {
                if(resourceBank.HasEnoughResources(amount))
                {
                    resourceBank.WithrawAmount(amount);
                }
            }
        }
    }
}
