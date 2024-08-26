using UnityEngine;

namespace GB.Weapons
{
    [CreateAssetMenu(menuName = "SO/Weapon/WeaponData")]
    public class WeaponDataSO : ScriptableObject
    {
        [Tooltip("Generic Property")]
        public string weaponName;
        public Weapon weaponPrefab = null;
        public Sprite weaponIcon;

        [Tooltip("Attack Property")]
        public float cooldown;
        public float damage;
        public LayerMask targetLayer;
    }
}
