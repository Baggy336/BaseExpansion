using Assets.Core.Resources;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Core
{
    [CreateAssetMenu(fileName = "CostAmounts", menuName = "ScriptableObjects/CostAmount")]
    public class ConstructionCost : ScriptableObject
    {
        [SerializeField]
        public List<ResourceCost> CostToConstruct;

        [SerializeField]
        public GameObject ConstructedObject;

        [SerializeField]
        public float ConstructionTime;

        [SerializeField]
        public KeyCode Hotkey;
    }
}
