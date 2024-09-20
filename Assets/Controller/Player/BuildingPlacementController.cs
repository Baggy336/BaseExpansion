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
        private Renderer renderer { get; set; }
        private Worker selectedWorker { get; set; }

        private void Update()
        {
            if (PlacingBuilding)
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
            renderer = buildingPreviewInstance.GetComponent<Renderer>();
            
        }

        public void ConfirmBuildingPlacement(Vector3 position)
        {
            selectedWorker.ConfirmConstruction(position, currentBuilding);
            CancelBuildingPlacement();
        }

        private void HandleBuildingPreview()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.WorldToScreenPoint(buildingPreviewInstance.transform.position).z;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            worldPosition.y = (renderer.bounds.size.y / 2) + .25f;
            buildingPreviewInstance.transform.position = worldPosition;
        }

        public void CancelBuildingPlacement()
        {
            PlacingBuilding = false;
            Destroy(buildingPreviewInstance);
        }
    }
}
