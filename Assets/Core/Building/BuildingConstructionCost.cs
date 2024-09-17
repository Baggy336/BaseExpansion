using UnityEngine;

namespace Assets.Core.Building
{
    [CreateAssetMenu(fileName = "BuildingCostAmounts", menuName = "ScriptableObjects/BuildingCostAmount")]
    public class BuildingConstructionCost : ConstructionCost
    {
        [SerializeField]
        public GameObject ConstructionPreview;
    }
}
