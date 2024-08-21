using UnityEngine;

namespace GB.Characters
{
    public class CharacterRotator : CharacterComponent
    {
        [SerializeField] Transform yawTransform = null; // y rotation
        [SerializeField] Transform pitchTransform = null; // x rotation

        [Space(15f)]
        [SerializeField] float pitchMin = -50;
        [SerializeField] float pitchMax = 50;
        private float currentPitch = 0f;

        public override void Init(CharacterController controller)
        {
            base.Init(controller);

            if(IsOwner == false)
                return;

            pitchTransform.localRotation = Quaternion.AngleAxis(currentPitch, Vector3.right);
        }

        public void RotateByDelta(Vector2 delta)
        {
            if(yawTransform != null)
            {
                float angle = yawTransform.eulerAngles.y + delta.x;
                yawTransform.localRotation = Quaternion.AngleAxis(angle, Vector3.up);
            }

            if(pitchTransform != null)
            {
                currentPitch -= delta.y;
                currentPitch = Mathf.Clamp(currentPitch, pitchMin, pitchMax);
                pitchTransform.localRotation = Quaternion.AngleAxis(currentPitch, Vector3.right);
            }
        }

        public void RotateToDirection(Vector3 direction)
        {
            Vector3 yaw = direction;
            yaw.y = 0;
            yawTransform.localRotation = Quaternion.LookRotation(yaw, Vector3.up);

            Vector3 pitch = direction;
            pitch.x = 0;
            pitch.z = 0;
            pitchTransform.localRotation = Quaternion.LookRotation(pitch);
        }
    }
}
