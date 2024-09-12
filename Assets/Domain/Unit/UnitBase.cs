using Assets.Controller.Unit;
using Assets.Controller.Unit.UI;
using Assets.Domain.Globals.Enums;
using Assets.Domain.Interfaces;
using Assets.Domain.Unit;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour, IMoveable, ICombatAble, IAttackable
{
    [SerializeField]
    public MovementController UnitMovementHandler;

    [SerializeField]
    private AttackController AttackHandler;

    [SerializeField]
    private HealthUIController HealthUIHandler;

    public HealthController HealthHandler;
    public UnitBaseStats UnitStats { get; set; }

    private UnitRuntimeStats UnitRuntimeStats;

    public PlayerController OwnerPlayer { get; set; }

    private UnitStates UnitState;

    public virtual void Awake()
    {
        UnitState = UnitStates.Idle;
        UnitRuntimeStats = new UnitRuntimeStats(UnitStats);
        HealthHandler = new HealthController();
        AttackHandler.UnitStats = new UnitRuntimeStats(UnitStats);
    }

    public virtual void Update()
    {
        switch(UnitState)
        {
            case UnitStates.Idle:
                break;
            case UnitStates.Moving:
                MoveUnit();
                break;
            case UnitStates.Attacking:
                HandleAttacking();
                break;
        }
    }

    private void MoveUnit()
    {
        UnitMovementHandler.MoveTo();
        UnitMovementHandler.RotateTo();
    }

    private void HandleAttacking()
    {
        if(AttackHandler.AttackTarget != null)
        {
            MoveToAttack();
            if (WithinAttackDistance() && WithinAttackInterval())
            {
                AttackTarget();
            }
        }
        else
        {
            AttackHandler.ClearAttackTarget();
            UnitState = UnitStates.Moving;
        }
    }

    private void MoveToAttack()
    {
        Vector3 attackPosition = AttackHandler.GetAttackPosition();
        SetMovementValues(attackPosition);
        MoveUnit();
    }

    private bool WithinAttackDistance()
    {
        return AttackHandler.WithinAttackDistance();
    }

    private bool WithinAttackInterval()
    {
        return AttackHandler.WithinAttackInterval();
    }

    private void AttackTarget()
    {
        AttackHandler.DealDamageToTarget();
    }

    public virtual void MoveToLocation(Vector3 location)
    {
        if(AttackHandler.AttackTarget != null)
        {
            AttackHandler.ClearAttackTarget();
        }
        UnitState = UnitStates.Moving;

        SetMovementValues(location);
    }

    private void SetMovementValues(Vector3 location)
    {
        UnitMovementHandler.SetMovementSpeed(UnitRuntimeStats.MovementSpeed);
        UnitMovementHandler.SetRotationSpeed(UnitRuntimeStats.RotationSpeed);
        UnitMovementHandler.HandleMovement(location);
    }

    public virtual void TakeDamage(int amount)
    {
        HealthHandler.TakeFromHealthPool(UnitRuntimeStats, amount);
        HealthUIHandler.UpdateHealthBar(UnitRuntimeStats.Health);
        if (UnitRuntimeStats.Health <= 0)
        {
            //SelectionHandler.RemoveSelectableObject(this); TODO: Deregister
            Destroy(gameObject);
        }
    }

    public virtual void Heal(int amount)
    {
        HealthHandler.AddToHealthPool(UnitRuntimeStats, amount);
        HealthUIHandler.UpdateHealthBar(UnitRuntimeStats.Health);
    }

    public void SetAttackTarget(IAttackable target)
    {
        AttackHandler.SetAttackTarget(target);
    }

    public void SetPlayerReference(PlayerController player)
    {
        OwnerPlayer = player;
    }
}
