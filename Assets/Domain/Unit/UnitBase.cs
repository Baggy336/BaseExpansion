using Assets.Controller.Unit;
using Assets.Domain.Interfaces;
using Assets.Domain.Unit;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour, IMoveable, ISelectable
{
    [SerializeField]
    public MovementController UnitMovementHandler;

    public UnitBaseStats UnitStats { get; set; }
 
    private UnitRuntimeStats UnitRuntimeStats;

    public virtual void Awake()
    {
        UnitRuntimeStats = new UnitRuntimeStats(UnitStats);
    }

    public virtual void MoveToLocation(Vector3 location)
    {
        UnitMovementHandler.SetMovementSpeed(UnitRuntimeStats.MovementSpeed);
        UnitMovementHandler.SetRotationSpeed(UnitRuntimeStats.RotationSpeed);
        UnitMovementHandler.HandleMovement(location);
    }
}
