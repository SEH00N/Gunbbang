using System;
using Unity.Netcode;
using UnityEngine;
using NetworkEvent = GB.NetworkEvents.NetworkEvent;

namespace GB.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] WeaponDataSO weaponData = null;
        public WeaponDataSO WeaponData => weaponData;

        [SerializeField] NetworkEvent onActivedEvent = null;
        [SerializeField] NetworkEvent onHoldEvent = null;
        [SerializeField] NetworkEvent onUnholdEvent = null;
        private NetworkObject owner = null;

        public bool IsCooldown => timer > 0;
        private float timer = 0f;

        public virtual void Init(NetworkObject owner)
        {
            this.owner = owner;

            onActivedEvent = new NetworkEvent($"{WeaponData.name}Actived");
            onActivedEvent.Register(owner);

            onHoldEvent = new NetworkEvent($"{WeaponData.name}Hold");
            onHoldEvent.AddListener(HandleWeaponHold);
            onHoldEvent.Register(owner);

            onUnholdEvent = new NetworkEvent($"{WeaponData.name}Unhold");
            onHoldEvent.AddListener(HandleWeaponUnhold);
            onUnholdEvent.Register(owner);
        }

        public virtual void Hold()
        {
            onHoldEvent?.Broadcast(false);
        }

        protected virtual void Update()
        {
            if(IsCooldown)
                timer -= Time.deltaTime;
        }

        public virtual void Unhold()
        {
            onUnholdEvent?.Broadcast(false);
        }

        public virtual void Release()
        {
            onActivedEvent.Unregister();
        }

        public void ActiveWeapon()
        {
            if(IsCooldown)
                return;

            timer = WeaponData.cooldown;

            OnWeaponActived();
            onActivedEvent?.Broadcast(false);
        }

        protected virtual void HandleWeaponHold()
        {
            gameObject.SetActive(true);
        }

        protected virtual void HandleWeaponUnhold()
        {
            gameObject.SetActive(false);
        }
        
        protected abstract void OnWeaponActived();
    }
}
