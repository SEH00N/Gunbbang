using GB.Entities.Components;
using UnityEngine;

namespace GB.Tests
{
    public class TDummy : MonoBehaviour
    {
        private EntityHealth health = null;

        private void Awake()
        {
            health = GetComponent<EntityHealth>();
        }

        private void Start()
        {
            health.SetMaxHP(100);
            health.ResetHP();
        }

        public void HandleHit()
        {
            Debug.Log("퍽");
        }

        public void HandleDamaged()
        {
            Debug.Log($"아얏 시발 ({health.CurrentHP})");
        }

        public void HandleDead()
        {
            Debug.Log("께꼬닥");
        }
    }
}
