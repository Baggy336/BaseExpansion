using Assets.Domain.Resources.Enums;
using UnityEngine;

namespace Assets.Core.Resources
{
    [CreateAssetMenu(fileName = "NewResourceNode", menuName = "Resource")]
    public class ResourceNode : ScriptableObject
    {
        [SerializeField]
        public ResourceTypes ResourceType;

        [SerializeField]
        public int BaseYield = 12000;
    }
}

