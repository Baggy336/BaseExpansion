using UnityEngine;

namespace Assets.Controller.Unit
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField]
        public LayerMask GroundLayer;

        [SerializeField]
        public Collider ObjectCollider;

        private Vector3 MovementDestination { get; set; }

        private Quaternion RotationDestination { get; set; }

        private float MovementSpeed { get; set; }

        private float RotationSpeed { get; set; }

        private void Update()
        {
            MoveTo(MovementDestination);
            RotateTo(RotationDestination);
        }

        public void SetMovementSpeed(float movementSpeed)
        {
            MovementSpeed = movementSpeed;
        }

        public void SetRotationSpeed(float rotationSpeed)
        {
            RotationSpeed = rotationSpeed;
        }

        public void MoveTo(Vector3 destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, MovementSpeed * Time.deltaTime);
        }

        private void RotateTo(Quaternion destination)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, destination, RotationSpeed * Time.deltaTime);
        }

        private void StopMovement()
        {
            MovementDestination = transform.position;
        }

        public void HandleMovement(Vector3 destination)
        {
            SetMovementDestination(destination);
            SetRotationDestination(destination);
        }

        private void SetMovementDestination(Vector3 destination)
        {
            destination = GetAdjustedForGroundDestination(destination);

            MovementDestination = destination;
        }

        private Vector3 GetAdjustedForGroundDestination(Vector3 destination)
        {
            float objectHeight = ObjectCollider.bounds.size.y;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
            {
                if (((1 << hit.collider.gameObject.layer) & GroundLayer) != 0)
                {
                    float groundHeight = hit.point.y;
                    destination = new Vector3(destination.x, groundHeight + objectHeight / 2, destination.z);
                }
            }

            return destination;
        }

        private void SetRotationDestination(Vector3 destination)
        {
            Quaternion rotateDirection = GetRotationDirection(destination);
            RotationDestination = rotateDirection;
        }

        private Quaternion GetRotationDirection(Vector3 destination)
        {
            Quaternion rotateDirection = transform.rotation;
            Vector3 movementDirection = (destination - transform.position).normalized;

            if (movementDirection != Vector3.zero)
            {
                float yRotation = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg;
                rotateDirection = Quaternion.Euler(0, yRotation, 0);
            }

            return rotateDirection;
        }
    }
}
