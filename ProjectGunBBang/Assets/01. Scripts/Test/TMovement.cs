using GB.Characters;
using Unity.Netcode;
using UnityEngine;
using CharacterController = GB.Characters.CharacterController;

namespace GB.Tests
{
    public class TMovement : NetworkBehaviour
    {
        private CharacterController controller;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if(IsOwner == false)
                return;

            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            controller.GetCharacterComponent<CharacterMovement>().SetDirection(new Vector2(x, y));

            float xDelta = Input.GetAxisRaw("Mouse X");
            float yDelta = Input.GetAxisRaw("Mouse Y");
            controller.GetCharacterComponent<CharacterRotator>().RotateByDelta(new Vector2(xDelta, yDelta));

            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(controller.GetCharacterComponent<CharacterMovement>().IsGround)
                    controller.GetCharacterComponent<CharacterMovement>().SetVerticalVelocity(10f);
            }
        }
    }
}
