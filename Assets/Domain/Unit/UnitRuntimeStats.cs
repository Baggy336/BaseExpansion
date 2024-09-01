namespace Assets.Domain.Unit
{
    public class UnitRuntimeStats
    {
        public int Health { get; set; }
        public int Armor { get; set; }
        public float MovementSpeed { get; set; }
        public int AttackDamage { get; set; }
        public float AttackRange { get; set; }
        public float AttackInterval { get; set; }
        public float RotationSpeed { get; set; }

        public UnitRuntimeStats(UnitBaseStats baseStats)
        {
            Health = baseStats.Health;
            Armor = baseStats.Armor;
            MovementSpeed = baseStats.MovementSpeed;
            AttackDamage = baseStats.AttackDamage;
            AttackRange = baseStats.AttackRange;
            RotationSpeed = baseStats.RotationSpeed;
            AttackInterval = baseStats.AttackInterval;
        }
    }
}
