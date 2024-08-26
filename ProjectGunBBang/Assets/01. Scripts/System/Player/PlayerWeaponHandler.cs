using System.Collections.Generic;
using GB.Entities;
using GB.Extensions;
using GB.Weapons;
using UnityEngine;
using UnityEngine.Events;

namespace GB.Players
{
    public class PlayerWeaponHandler : EntityComponent
    {
        [SerializeField] UnityEvent onFailedToActiveEvent = null;
        [SerializeField] Transform weaponContainer = null;
        private List<Weapon> weapons = new List<Weapon>();

        private Weapon currentWeapon = null;

        public void SetWeaponTable(WeaponDataSO[] weaponDatas)
        {
            weaponDatas.ForEach(i => {
                Weapon weapon = Instantiate(i.weaponPrefab, weaponContainer);
                weapon.Init(controller.NetworkObject);
                weapon.Unhold();
                weapons.Add(weapon);
            });
        }

        public override void Release()
        {
            base.Release();
            weapons.ForEach(i => {
                i.Release();
                Destroy(i.gameObject);
            });
        }

        public void ActiveWeapon()
        {
            if(currentWeapon.IsCooldown)
            {
                onFailedToActiveEvent?.Invoke();
                return;
            }

            currentWeapon.ActiveWeapon();
        }

        public void ChangeWeapon(int index)
        {
            if(weapons.Count <= index || index < 0)
                return;

            currentWeapon?.Unhold();

            if(currentWeapon == weapons[index])
                currentWeapon = null;
            else
                currentWeapon = weapons[index];
            
            currentWeapon?.Hold();
        }
    }
}
