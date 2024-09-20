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
            LocationToCreateBuilding = GetPlacementLocation(position);
            ProductionTimer = building.ConstructionTime;
        }

        private Vector3 GetPlacementLocation(Vector3 position)
        {
            Renderer buildingRenderer = BuildingToConstruct.GetComponent<Renderer>();
            position.y = (buildingRenderer.bounds.size.y / 2) + .25f;
            return position;
        }

        public void SetProductionUI()
        {
            CurrentBuildingProductionUI = CurrentBuildingInstance.GetComponent<ProductionUIController>();
        }
    }
}
