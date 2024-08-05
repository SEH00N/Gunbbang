using UnityEngine;

namespace GB.Characters
{
    public class CharacterMovement : CharacterComponent
    {
        private UnityEngine.CharacterController characterController = null;

        public override void Init(CharacterController controller)
        {
            base.Init(controller);

            characterController = GetComponent<UnityEngine.CharacterController>();
        }
    }
}
