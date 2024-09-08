using Assets.Domain;
using UnityEngine;

namespace Assets.Controller.Unit
{
    public class HealthController
    {
        public void TakeFromHealthPool(RuntimeStats runtimeStats, int amount)
        {
            runtimeStats.Health -= amount;
        }

        public void AddToHealthPool(RuntimeStats runtimeStats, int amount)
        {
            runtimeStats.Health += amount;
        }
    }
}
