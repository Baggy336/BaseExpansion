using UnityEngine;

[CreateAssetMenu(fileName = "NewUnitBaseStats", menuName = "Unit")]
public class UnitBaseStats : ScriptableObject
{
    public string UnitName;

    public int Health;

    public int Armor;

    public float MovementSpeed;
}
