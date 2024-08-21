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
        private NetworkObject owner = null;

        private float timer = 0f;

        public virtual void Init(NetworkObject owner)
        {
            this.owner = owner;

            onActivedEvent = new NetworkEvent($"{WeaponData.name}Actived");
            onActivedEvent.Register(owner);
        }

        private void Update()
        {
            if(timer > 0f)
                timer -= Time.deltaTime;
        }

        public virtual void Release()
        {
            onActivedEvent.Unregister();
        }

        public void ActiveWeapon()
        {
            if(timer > 0f)
                return;

            timer = WeaponData.cooldown;

            OnWeaponActived();
            onActivedEvent?.Broadcast(false);
        }
        
        protected abstract void OnWeaponActived();
    }
}
