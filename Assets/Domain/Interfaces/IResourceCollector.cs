using Assets.Domain.Building.Economy;
using Assets.Domain.Resources;

namespace Assets.Domain.Interfaces
{
    public interface IResourceCollector
    {
        public void SetTargetResource(ResourceNodeRuntime resourceNode);
        public void SetDepositPoint(ResourceDepot resourceDepot);
    }
}
