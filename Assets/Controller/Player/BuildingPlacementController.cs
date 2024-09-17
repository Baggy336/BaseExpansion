using Assets.Core.Building;
using Assets.Domain.Unit.PlayerUnits;
using UnityEngine;

namespace Assets.Controller.Player
{
    public class BuildingPlacementController : MonoBehaviour
    {
        public bool PlacingBuilding { get; set; }
        private GameObject buildingPreviewPrefab { get; set; }
        private GameObject buildingPreviewInstance { get; set; }
        private BuildingConstructionCost currentBuilding { get; set; }
        private Worker selectedWorker { get; set; }

        private void Update()
        {
            if(PlacingBuilding)
            {
                HandleBuildingPreview();
            }
        }

        public void StartBuildingPlacement(Worker workerInvoked, BuildingConstructionCost building)
        {
            buildingPreviewPrefab = building.ConstructionPreview;
            currentBuilding = building;
            selectedWorker = workerInvoked;
            PlacingBuilding = true;
            buildingPreviewInstance = Instantiate(buildingPreviewPrefab, transform);
        }

        public void ConfirmBuildingPlacement(Vector3 position)
        {
            CancelBuildingPlacement();
            selectedWorker.ConfirmConstruction(position, currentBuilding);
        }

        private void HandleBuildingPreview()
        {
            buildingPreviewInstance.transform.position = Input.mousePosition;
        }

        public void CancelBuildingPlacement()
        {
            PlacingBuilding = false;
            Destroy(buildingPreviewInstance);
        }
    }
}
