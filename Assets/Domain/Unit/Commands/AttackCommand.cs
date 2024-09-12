using Assets.Domain.Interfaces;

public class AttackCommand : ICommand
{
    private readonly ICombatAble _combatReady;

    private readonly IAttackable _attackable;

    public AttackCommand(ICombatAble combatReady, IAttackable attackable)
    {
        _combatReady = combatReady;
        _attackable = attackable;
    }

    public void Execute()
    {
        _combatReady.SetAttackTarget(_attackable);
    }
}
