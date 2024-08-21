using System.Collections.Generic;
using GB.Characters;
using GB.Extensions;
using GB.Weapons;
using UnityEngine;

namespace GB.Players
{
    public class PlayerWeaponHandler : CharacterComponent
    {
        [SerializeField] Transform weaponContainer = null;
        private List<Weapon> weapons = new List<Weapon>();

        private Weapon currentWeapon = null;

        public void SetWeaponTable(WeaponDataSO[] weaponDatas)
        {
            weaponDatas.ForEach(i => {
                Weapon weapon = Instantiate(i.weaponPrefab, weaponContainer);
                weapon.Init(controller.NetworkObject);
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
                return;

            currentWeapon.ActiveWeapon();
        }

        public void ChangeWeapon(int index)
        {
            if(weapons.Count >= index || index < 0)
                return;

            currentWeapon?.Unhold();
            currentWeapon = weapons[index];
            currentWeapon?.Hold();
        }
    }
}
