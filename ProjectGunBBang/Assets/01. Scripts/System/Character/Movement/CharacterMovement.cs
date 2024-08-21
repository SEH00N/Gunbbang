using UnityEngine;

namespace GB.Characters
{
    public class CharacterMovement : CharacterComponent
    {
        private UnityEngine.CharacterController characterController = null;

        [Tooltip("Movement")]
        [SerializeField] float maxSpeed = 10f;
        [SerializeField] float acceleration = 20f;
        private float currentSpeed = 0f;

        [Tooltip("Gravity")]
        [SerializeField] bool useGravity = true;
        public bool IsGround = false;

        [Space(15f)]
        [SerializeField] float gravityScale = 1f;
        [SerializeField, Range(0f, 50f)] float gravityScaleLimit = 30f;
        private float gravityAccel = -9.81f;

        [Space(15f)]
        [SerializeField] float groundDistance = 0.5f;
        [SerializeField] Transform footTransform = null;
        [SerializeField] LayerMask groundLayer = 1 << 6;
        
        private Vector2 moveDirection = Vector3.zero;
        private float verticalVelocity = 0f;

        public override void Init(CharacterController controller)
        {
            base.Init(controller);

            characterController = GetComponent<UnityEngine.CharacterController>();
        }

        private void FixedUpdate()
        {
            if(useGravity)
                CalculateGravity();

            bool shouldMove = moveDirection.sqrMagnitude > 0;
            Vector2 planeVelocity = Vector3.zero;

            if(shouldMove)
            {
                currentSpeed += acceleration * Time.fixedDeltaTime;
                currentSpeed = Mathf.Min(currentSpeed, maxSpeed);

                planeVelocity = currentSpeed * moveDirection;
            }

            Vector3 velocity = new Vector3(planeVelocity.x, verticalVelocity, planeVelocity.y);
            characterController.Move(velocity * Time.fixedDeltaTime);
        }

        private void CalculateGravity()
        {
            IsGround = Physics.Raycast(footTransform.position, Vector3.down, groundDistance, groundLayer);
            if(IsGround && verticalVelocity < gravityAccel)
            {
                verticalVelocity = gravityAccel;
            }
            else
            {
                verticalVelocity += gravityAccel * gravityScale * Time.fixedDeltaTime;
                verticalVelocity = Mathf.Clamp(verticalVelocity, -gravityScaleLimit, gravityScaleLimit);
            }
        }

        public void SetDirection(Vector2 direction)
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

        public void SetVerticalVelocity(float velocity)
        {
            verticalVelocity = velocity;
        }
    }
}
