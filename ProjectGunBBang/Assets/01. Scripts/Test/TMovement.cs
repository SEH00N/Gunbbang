using GB.Characters;
using UnityEngine;
using CharacterController = GB.Characters.CharacterController;

namespace GB.Tests
{
    public class TMovement : MonoBehaviour
    {
        private CharacterController controller;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            
            controller.GetCharacterComponent<CharacterMovement>().SetDirection(new Vector3(x, 0, y));
        }
    }
}
