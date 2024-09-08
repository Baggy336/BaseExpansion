using Assets.Domain.Interfaces;
using Assets.Domain.Unit;
using UnityEngine;

namespace Assets.Controller.Unit
{
    public class AttackController : MonoBehaviour
    {
        public IAttackable AttackTarget { get; set; }

        private Transform AttackTargetLocation { get; set; }

        public UnitRuntimeStats UnitStats { get; set; }

        private float TimeSinceLastAttack { get; set; }

        public AttackController(UnitRuntimeStats unitStats)
        {
            UnitStats = unitStats;
        }

        public void Update()
        {
            TimeSinceLastAttack += Time.deltaTime;
        }

        public void SetAttackTarget(IAttackable attackable)
        {
            AttackTarget = attackable;
            AttackTargetLocation = (attackable as MonoBehaviour).transform;
        }

        public void ClearAttackTarget()
        {
            AttackTarget = null;
            AttackTargetLocation = null;
        }

        public Vector3 GetAttackPosition()
        {
            if(AttackTargetLocation != null)
            {
                Vector3 directionToTarget = (AttackTargetLocation.position - transform.position).normalized;
                return AttackTargetLocation.position - directionToTarget * UnitStats.AttackRange;
            }
            return transform.position;
        }

        public bool WithinAttackDistance()
        {
            if (AttackTargetLocation != null)
            {
                return Vector3.Distance(AttackTargetLocation.position, transform.position) <= UnitStats.AttackRange;
            }
            return false;
        }

        public bool WithinAttackInterval()
        {
            return TimeSinceLastAttack >= UnitStats.AttackInterval;
        }

        public void DealDamageToTarget()
        {
            AttackTarget.TakeDamage(UnitStats.AttackDamage);
            TimeSinceLastAttack = 0f;
        }
    }
}
