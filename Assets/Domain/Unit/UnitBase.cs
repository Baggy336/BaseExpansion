using Assets.Domain.Interfaces;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour, IMoveable, ISelectable
{
    [SerializeField]
    public UnitBaseStats UnitStats;

    public void MoveToLocation(Vector3 location)
    {

    }
}
