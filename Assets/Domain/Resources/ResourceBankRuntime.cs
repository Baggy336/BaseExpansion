using Assets.Domain.Resources.Enums;

namespace Assets.Domain.Resources
{
    public class ResourceBankRuntime
    {
        public ResourceTypes ResourceType;

        public int ResourceStoredAmount;

        public ResourceBankRuntime(ResourceTypes resourceType, int resourceStoredAmount)
        {
            ResourceType = resourceType;
            ResourceStoredAmount = resourceStoredAmount;
        }

        public bool HasEnoughResources(int amountToTake)
        {
            return ResourceStoredAmount - amountToTake > 0;
        }

        public void WithrawAmount(int amountToTake)
        {
            ResourceStoredAmount -= amountToTake;
        }
        
        public void DepositAmount(int amountToTake)
        {
            ResourceStoredAmount += amountToTake;
        }
    }
}
