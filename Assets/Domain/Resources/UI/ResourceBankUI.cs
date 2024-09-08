using Assets.Domain.Resources.Enums;
using TMPro;
using UnityEngine;

namespace Assets.Domain.Resources.UI
{
    public class ResourceBankUI : MonoBehaviour
    {
        public ResourceTypes ResourceType;

        public TextMeshProUGUI ResourceAmountText;

        public TextMeshProUGUI ResourceLabelText;

        public void UpdateResourceAmount(int amount)
        {
            ResourceAmountText.text = amount.ToString();
        }

        public void SetResourceLabel(string label)
        {
            ResourceLabelText.text = label;
        }
    }
}
