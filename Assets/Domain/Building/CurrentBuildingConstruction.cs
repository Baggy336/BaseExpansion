using Assets.Controller.Building.UI;
using Assets.Core.Building;
using UnityEngine;

namespace Assets.Domain.Building
{
    public class CurrentBuildingConstruction
    {
        public GameObject CurrentBuildingInstance { get; set; }

        public GameObject BuildingToConstruct { get; set; }

        public ProductionUIController CurrentBuildingProductionUI { get; set; }

        public Vector3 LocationToCreateBuilding { get; set; }

        public float ProductionTimer { get; set; } = 0f;

        public CurrentBuildingConstruction(Vector3 position, BuildingConstructionCost building)
        {
            CurrentBuildingInstance = building.ConstructionPreview;
            BuildingToConstruct = building.ConstructedObject;
            CurrentBuildingProductionUI = CurrentBuildingInstance.GetComponent<ProductionUIController>();
            LocationToCreateBuilding = position;
        }
    }
}
