namespace Assets.Domain.Interfaces
{
    public interface ICombatAble : ISelectable 
    {
        void SetAttackTarget(IAttackable target);
    }
}
