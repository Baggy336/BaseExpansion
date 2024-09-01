using Assets.Domain.Resources.Enums;
using UnityEngine;

namespace Assets.Core.Resources
{
    [CreateAssetMenu(fileName = "NewResourceBank", menuName = "ResourceBank")]
    public class ResourceBank : ScriptableObject
    {
        [SerializeField]
        public ResourceTypes ResourceType;

        [SerializeField]
        public int ResourceStoredAmount;
    }
}
