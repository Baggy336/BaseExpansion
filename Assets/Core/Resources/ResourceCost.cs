using Assets.Domain.Resources.Enums;
using System;
using UnityEngine;

namespace Assets.Core.Resources
{
    [Serializable]
    public class ResourceCost
    {
        [SerializeField]
        public ResourceTypes ResourceType;

        [SerializeField]
        public int Cost;
    }
}
