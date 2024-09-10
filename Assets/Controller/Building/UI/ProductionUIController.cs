using UnityEngine;
using UnityEngine.UI;

namespace Assets.Controller.Building.UI
{
    public class ProductionUIController : MonoBehaviour
    {
        [SerializeField]
        public Image ProductionProgressBar;

        [SerializeField]
        public Image ProductionProgressBarBackground;

        private float InitialProductionTime { get; set; }

        private void Update()
        {
            Vector3 cameraForward = Camera.main.transform.forward;
            ProductionProgressBarBackground.transform.rotation = Quaternion.LookRotation(cameraForward);
        }

        public void UpdateProgressBar(float productionTime)
        {
            float productionProgress = Mathf.Clamp01(1 - (productionTime / InitialProductionTime));
            ProductionProgressBar.fillAmount = productionProgress;
        }

        public void SetProgressBarActive(float productionTime)
        {
            if(ProductionProgressBarBackground.gameObject.activeSelf == false)
            {
                InitialProductionTime = productionTime;
                ProductionProgressBarBackground.gameObject.SetActive(true);
            }
        }
        
        public void SetProgressBarInactive()
        {
            if (ProductionProgressBarBackground.gameObject.activeSelf == true)
            {
                ProductionProgressBarBackground.gameObject.SetActive(false);
            }
        }
    }
}
