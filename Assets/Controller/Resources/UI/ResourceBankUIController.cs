using Assets.Domain.Resources.Enums;
using Assets.Domain.Resources.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Controller.Resources.UI
{
    public class ResourceBankUIController : MonoBehaviour
    {
        [SerializeField]
        private List<ResourceBankUI> ResourceUIElements;

        public void UpdateResourceUI(ResourceTypes resourceType, int amount)
        {
            ResourceBankUI resourceBank = ResourceUIElements.Where(x => x.ResourceType == resourceType).First();

            resourceBank.UpdateResourceAmount(amount);
        }
    }
}
