using UnityEngine;

namespace GB.Weapons
{
    [CreateAssetMenu(menuName = "SO/Weapon/WeaponData")]
    public class WeaponDataSO : ScriptableObject
    {
        [Tooltip("Generic Property")]
        public string weaponName;
        public Sprite weaponIcon;

        [Tooltip("Attack Property")]
        public float cooldown;
        public float damage;
    }
}
