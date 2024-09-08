using Assets.Core.Building;

namespace Assets.Domain.Building
{
    public class BuildingRuntimeStats : RuntimeStats
    { 
        public BuildingRuntimeStats(BuildingBaseStats baseStats)
        {
            Health = baseStats.Health;
            Armor = baseStats.Armor;
            Name = baseStats.Name;
        }
    }
}
