using UnityEngine;

namespace Assets.Core.Unit
{
    [CreateAssetMenu(fileName = "NewWorkerBaseStats", menuName = "WorkerStats")]
    public class WorkerBaseStats : UnitBaseStats
    {
        public int CarryCapacity;

        public int HarvestYield;

        public float HarvestInterval;

        public float HarvestDistance;
    }
}
