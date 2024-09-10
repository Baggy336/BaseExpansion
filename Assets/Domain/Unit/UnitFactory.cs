using UnityEngine;

namespace Assets.Domain.Unit
{
    public class UnitFactory : MonoBehaviour
    {
        public UnitBase CreateUnit(GameObject unitPrefab, Vector3 position, PlayerController player)
        {
            GameObject unitObject = Instantiate(unitPrefab, position, Quaternion.identity);
            UnitBase unit = unitObject.GetComponent<UnitBase>();
            unit.Initialize(player);
            return unit;
        }
    }
}
