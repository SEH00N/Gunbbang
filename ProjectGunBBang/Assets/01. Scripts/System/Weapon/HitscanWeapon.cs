using GB.Entities.Components;
using Unity.Netcode;
using UnityEngine;

namespace GB.Weapons
{
    public class HitscanWeapon : Weapon
    {
        [SerializeField] Transform muzzle = null;
        private Transform cameraTransform = null;

        public override void Init(NetworkObject owner)
        {
            base.Init(owner);
            cameraTransform = Camera.main.transform;
        }

        protected override void OnWeaponActived()
        {
            bool isHit = Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, float.MaxValue, WeaponData.targetLayer);
            Debug.DrawLine(cameraTransform.position, isHit ? hit.point : cameraTransform.position + cameraTransform.forward * float.MaxValue, Color.red, 1f);
            if(isHit == false)
                return;

            if(hit.transform.TryGetComponent<EntityHealth>(out EntityHealth health) == false)
                return;

            health.InflictDamage(owner, WeaponData.damage, hit.point, hit.normal);
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(cameraTransform == null)
                return;

            if (Camera.current.transform != cameraTransform)
                return;

            Gizmos.color = Color.red;
            bool isHit = Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, float.MaxValue, WeaponData.targetLayer);
            if(isHit)
            {
                if(hit.transform.TryGetComponent<EntityHealth>(out EntityHealth health))
                    Gizmos.color = Color.green;

                Gizmos.DrawLine(cameraTransform.position, hit.point);
                Gizmos.DrawWireSphere(hit.point, 0.5f);
            }
            else
                Gizmos.DrawLine(cameraTransform.position, cameraTransform.position + cameraTransform.forward * float.MaxValue);
        }
        #endif
    }
}
