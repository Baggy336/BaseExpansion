using Assets.Domain.Resources;
using UnityEngine.Events;

namespace Assets.Controller.Resources.Events
{
    public class HarvestResourceEvent : UnityEvent<ResourceNodeRuntime, int> { }
}
