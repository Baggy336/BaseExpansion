using Assets.Controller.Selection;
using Assets.Controller.Unit;
using Assets.Controller.Unit.UI;
using Assets.Domain.Globals.Enums;
using Assets.Domain.Interfaces;
using Assets.Domain.Unit;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour, IMoveable, ISelectable, IAttackable
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

    private PlayerController OwnerPlayer { get; set; }

    public SelectionController SelectionHandler { get; set; }

    private UnitStates UnitState;

    public virtual void Awake()
    {
        UnitState = UnitStates.Idle;
        UnitRuntimeStats = new UnitRuntimeStats(UnitStats);
        HealthHandler = new HealthController();
        AttackHandler.UnitStats = new UnitRuntimeStats(UnitStats);
    }

    public virtual void Initialize(PlayerController player)
    {
        OwnerPlayer = player;
        SelectionHandler = player.GetComponent<SelectionController>();
        SelectionHandler.AddSelectableToList(this);
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
        if (HitSomethingAttackable(location, out IAttackable attackableTarget))
        {
            SetAttackTarget(attackableTarget);
            UnitState = UnitStates.Attacking;
        }
        else
        {
            if(AttackHandler.AttackTarget != null)
            {
                AttackHandler.ClearAttackTarget();
            }
            UnitState = UnitStates.Moving;
        }

        SetMovementValues(location);
    }

    private void SetMovementValues(Vector3 location)
    {
        UnitMovementHandler.SetMovementSpeed(UnitRuntimeStats.MovementSpeed);
        UnitMovementHandler.SetRotationSpeed(UnitRuntimeStats.RotationSpeed);
        UnitMovementHandler.HandleMovement(location);
    }

    private bool HitSomethingAttackable(Vector3 location, out IAttackable attackableTarget)
    {
        Collider[] hitColliders = Physics.OverlapSphere(location, 1f);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != null && hitCollider.gameObject != this.gameObject)
            {
                IAttackable attackable = hitCollider.gameObject.GetComponent<IAttackable>();
                ISelectable selectable = hitCollider.gameObject.GetComponent<ISelectable>();

                if (selectable != null)
                {
                    if (SelectionHandler.SelectableObjects.Contains(selectable as MonoBehaviour))
                    {
                        attackableTarget = null;
                        return false;
                    }
                }

                if (attackable != null)
                {
                    attackableTarget = attackable;
                    return true;
                }
            }
        }
        attackableTarget = null;
        return false;
    }

    private void SetAttackTarget(IAttackable attackableTarget)
    {
        AttackHandler.SetAttackTarget(attackableTarget);
    }

    public virtual void TakeDamage(int amount)
    {
        HealthHandler.TakeFromHealthPool(UnitRuntimeStats, amount);
        HealthUIHandler.UpdateHealthBar(UnitRuntimeStats.Health);
        if (UnitRuntimeStats.Health <= 0)
        {
            SelectionHandler.RemoveSelectableObject(this);
            Destroy(gameObject);
        }
    }

    public virtual void Heal(int amount)
    {
        HealthHandler.AddToHealthPool(UnitRuntimeStats, amount);
        HealthUIHandler.UpdateHealthBar(UnitRuntimeStats.Health);
    }
}
