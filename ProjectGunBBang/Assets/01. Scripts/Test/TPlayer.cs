using Cinemachine;
using GB.Entities;
using GB.Entities.Components;
using GB.Players;
using GB.Weapons;
using Unity.Netcode;
using UnityEngine;

namespace GB.Tests
{
    public class TPlayer : NetworkBehaviour
    {
        [SerializeField] WeaponDataSO[] weapons = null;
        [SerializeField] CinemachineVirtualCamera playerVCam = null;
        private EntityController controller;

        private void Awake()
        {
            controller = GetComponent<EntityController>();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsOwner)
                playerVCam.Priority = 10;            
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
            
            if(Input.GetKeyDown(KeyCode.Alpha1))
                controller.GetEntityComponent<PlayerWeaponHandler>().ChangeWeapon(0);

            if(Input.GetKeyDown(KeyCode.Mouse0))
                controller.GetEntityComponent<PlayerWeaponHandler>().ActiveWeapon();

        }
    }
}
