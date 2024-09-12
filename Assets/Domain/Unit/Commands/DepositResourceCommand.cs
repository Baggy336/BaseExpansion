using Assets.Domain.Building.Economy;
using Assets.Domain.Interfaces;

namespace Assets.Domain.Unit.Commands
{
    public class DepositResourceCommand : ICommand
    {
        private readonly IResourceCollector _resourceCollector;

        private readonly ResourceDepot _resourceDepot;

        public DepositResourceCommand(ResourceDepot resourceDepot, IResourceCollector resourceCollector)
        {
            _resourceDepot = resourceDepot;
            _resourceCollector = resourceCollector;
        }

        public void Execute()
        {
            _resourceCollector.SetDepositPoint(_resourceDepot);
        }
    }
}
