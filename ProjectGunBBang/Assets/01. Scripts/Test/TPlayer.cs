using Cinemachine;
using GB.Entities;
using GB.Entities.Components;
using GB.Players;
using GB.Weapons;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace GB.Tests
{
    public class TPlayer : NetworkBehaviour
    {
        [SerializeField] WeaponDataSO[] weapons = null;
        [SerializeField] Camera overlayCamera = null;
        [SerializeField] CinemachineVirtualCamera playerVCam = null;
        private EntityController controller;

        private void Awake()
        {
            controller = GetComponent<EntityController>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsOwner)
            {
                playerVCam.Priority = 10;
                Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(overlayCamera);
            }
        }

        private void Update()
        {
            if(IsOwner == false)
                return;

            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            controller.GetEntityComponent<EntityMovement>().SetDirection(new Vector2(x, y));

            float xDelta = Input.GetAxisRaw("Mouse X");
            float yDelta = Input.GetAxisRaw("Mouse Y");
            controller.GetEntityComponent<EntityRotator>().RotateByDelta(new Vector2(xDelta, yDelta));

            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(controller.GetEntityComponent<EntityMovement>().IsGround)
                    controller.GetEntityComponent<EntityMovement>().SetVerticalVelocity(10f);
            }

            if(Input.GetKeyDown(KeyCode.Alpha0))
                controller.GetEntityComponent<PlayerWeaponHandler>().SetWeaponTable(weapons);
            
            for(int i = (int)KeyCode.Alpha1; i < (int)KeyCode.Alpha9; ++i)
            {
                if(Input.GetKeyDown((KeyCode)i))
                    controller.GetEntityComponent<PlayerWeaponHandler>().ChangeWeapon(i - (int)KeyCode.Alpha1);
            }

            if(Input.GetKeyDown(KeyCode.Mouse0))
                controller.GetEntityComponent<PlayerWeaponHandler>().ActiveWeapon();

        }
    }
}
