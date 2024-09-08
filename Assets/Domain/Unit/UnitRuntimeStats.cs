namespace Assets.Domain.Unit
{
    public class UnitRuntimeStats : RuntimeStats
    {
        public float MovementSpeed { get; set; }
        public int AttackDamage { get; set; }
        public float AttackRange { get; set; }
        public float AttackInterval { get; set; }
        public float RotationSpeed { get; set; }

        public UnitRuntimeStats(UnitBaseStats baseStats)
        {
            Health = baseStats.Health;
            Armor = baseStats.Armor;
            Name = baseStats.Name;
            MovementSpeed = baseStats.MovementSpeed;
            AttackDamage = baseStats.AttackDamage;
            AttackRange = baseStats.AttackRange;
            RotationSpeed = baseStats.RotationSpeed;
            AttackInterval = baseStats.AttackInterval;
        }
    }
}
