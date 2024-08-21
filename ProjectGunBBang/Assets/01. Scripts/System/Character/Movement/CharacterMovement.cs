using UnityEngine;

namespace GB.Characters
{
    public class CharacterMovement : CharacterComponent
    {
        private UnityEngine.CharacterController characterController = null;

        [SerializeField] float maxSpeed = 10f;
        [SerializeField] float acceleration = 20f;
        private float currentSpeed = 0f;
        
        private Vector3 moveDirection = Vector3.zero;
        public Vector3 Velocity { get; private set; } = Vector3.zero;

        public override void Init(CharacterController controller)
        {
            base.Init(controller);

            characterController = GetComponent<UnityEngine.CharacterController>();
        }

        private void FixedUpdate()
        {
            bool shouldMove = moveDirection.sqrMagnitude > 0;
            if(shouldMove == false)
                return;

            currentSpeed += acceleration * Time.fixedDeltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);

            Velocity = currentSpeed * moveDirection;
            characterController.Move(Velocity * Time.fixedDeltaTime);
        }

        public void SetDirection(Vector3 direction)
        {
            direction = direction.normalized;
            
            if(direction.sqrMagnitude < 0.1f)
            {
                currentSpeed = 0f;
            }
            else
            {
                float theta = Mathf.Acos(Vector3.Dot(moveDirection, direction)) * Mathf.Rad2Deg;
                bool directionReversed = theta > 90f;
                if(directionReversed)
                    currentSpeed -= currentSpeed * (2 / 3);
            }

            moveDirection = direction;
        }

        public void AddDirection(Vector3 direction)
        {
            SetDirection(moveDirection + direction);
        }
    }
}
