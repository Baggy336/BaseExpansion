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

        private float ObjectRadius { get; set; }

        private float AttackTargetRadius { get; set; }

        private void Awake()
        {
            ObjectRadius = GetComponent<Collider>().bounds.extents.x;
        }

        public void Update()
        {
            TimeSinceLastAttack += Time.deltaTime;

            if (AttackTarget != null && WithinAttackDistance() && WithinAttackInterval())
            {
                DealDamageToTarget();
            }
        }

        public void SetAttackTarget(IAttackable attackable)
        {
            MonoBehaviour attackableObject = (attackable as MonoBehaviour);
            AttackTarget = attackable;
            AttackTargetLocation = attackableObject.transform;
            AttackTargetRadius = attackableObject.GetComponent<Collider>().bounds.extents.x;
        }

        public void ClearAttackTarget()
        {
            AttackTarget = null;
            AttackTargetLocation = null;
            AttackTargetRadius = 0f;
        }

        public Vector3 GetAttackPosition()
        {
            if (AttackTargetLocation != null)
            {
                Vector3 directionToTarget = (AttackTargetLocation.position - transform.position).normalized;
                float combinedRadii = ObjectRadius + AttackTargetRadius;
                float desiredDistance = UnitStats.AttackRange + combinedRadii;
                float distanceToTarget = Vector3.Distance(transform.position, AttackTargetLocation.position);

                Debug.Log($"ObjectRadius: {ObjectRadius}, AttackTargetRadius: {AttackTargetRadius}, AttackRange: {UnitStats.AttackRange}");
                Debug.Log($"Distance to Target: {distanceToTarget}, Desired Distance: {desiredDistance}");

                if (distanceToTarget > desiredDistance)
                {
                    return transform.position + directionToTarget * (distanceToTarget - desiredDistance);
                }
                else
                {
                    return transform.position;
                }
            }
            return transform.position;
        }

        public bool WithinAttackDistance()
        {
            if (AttackTargetLocation != null)
            {
                float distanceBetweenCenters = Vector3.Distance(AttackTargetLocation.position, transform.position);
                float combinedRadii = ObjectRadius + AttackTargetRadius;
                float attackDistance = UnitStats.AttackRange + combinedRadii;

                return distanceBetweenCenters <= attackDistance;
            }
            return false;
        }

        public bool WithinAttackInterval()
        {
            return TimeSinceLastAttack >= UnitStats.AttackInterval;
        }

        public void DealDamageToTarget()
        {
            if (AttackTarget != null)
            {
                AttackTarget.TakeDamage(UnitStats.AttackDamage);
                TimeSinceLastAttack = 0f;
            }
        }
    }
}
