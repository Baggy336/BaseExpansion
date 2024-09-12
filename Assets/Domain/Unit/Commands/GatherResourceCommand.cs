using Assets.Domain.Interfaces;
using Assets.Domain.Resources;

namespace Assets.Domain.Unit.Commands
{
    public class GatherResourceCommand : ICommand
    {
        private readonly ResourceNodeRuntime _resource;

        private readonly IResourceCollector _resourceCollector;

        public GatherResourceCommand(ResourceNodeRuntime resource, IResourceCollector resourceCollector)
        {
            _resource = resource;
            _resourceCollector = resourceCollector;
        }

        public void Execute()
        {
            _resourceCollector.SetTargetResource(_resource);
        }
    }
}
