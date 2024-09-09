using Assets.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Controller.Unit.UI
{
    public class HealthUIController : MonoBehaviour
    {
        [SerializeField]
        public Image HealthBarBackground;

        [SerializeField]
        public Image HealthBarImage;

        [SerializeField]
        public BaseStats BaseStats;

        private int MaxHealth { get; set; }

        private void Awake()
        {
            MaxHealth = BaseStats.Health;
            HealthBarImage.color = Color.Lerp(Color.red, Color.green, MaxHealth);
        }

        private void Update()
        {
            Vector3 cameraForward = Camera.main.transform.forward;
            HealthBarBackground.transform.rotation = Quaternion.LookRotation(cameraForward);
        }

        public void UpdateHealthBar(int currentHealth)
        {
            float healthPercentage = (float)currentHealth / MaxHealth;
            HealthBarImage.fillAmount = healthPercentage;
            HealthBarImage.color = Color.Lerp(Color.red, Color.green, healthPercentage);
        }
    }
}
