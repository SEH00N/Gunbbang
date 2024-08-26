using GB.NetworkEvents;
using Unity.Netcode;
using UnityEngine;
using NetworkEvent = GB.NetworkEvents.NetworkEvent;

namespace GB.Entities.Components
{
    public class EntityHealth : EntityComponent
    {
        [SerializeField] NetworkEvent<AttackParams> onHitEvent = new NetworkEvent<AttackParams>("HitEvent");
        [SerializeField] NetworkEvent<AttackParams> onDamagedEvent = new NetworkEvent<AttackParams>("DamagedEvent");
        [SerializeField] NetworkEvent<FloatParams, float> onHealEvent = new NetworkEvent<FloatParams, float>("HealEvent");
        [SerializeField] NetworkEvent onDeadEvent = new NetworkEvent("DeadEvent");

        private bool isDead = false;
        public bool IsDead => isDead;

        private float maxHP = 0;
        public float MaxHP => maxHP;

        private float currentHP = 0;
        public float CurrentHP => currentHP;

        public override void Init(EntityController controller)
        {
            base.Init(controller);

            onHitEvent.AddListener(HandleHit);
            onHitEvent.Register(controller.NetworkObject);

            onDamagedEvent.Register(controller.NetworkObject);
            onHealEvent.Register(controller.NetworkObject);
            onDeadEvent.Register(controller.NetworkObject);
        }

        public override void Release()
        {
            base.Release();

            onHitEvent.Unregister();
            onDamagedEvent.Unregister();
            onHealEvent.Unregister();
            onDeadEvent.Unregister();
        }

        public void SetMaxHP(float maxHP)
        {
            this.maxHP = maxHP;
        }

        public void ResetHP()
        {
            currentHP = maxHP;
        }

        public void Heal(float amount)
        {
            if(IsOwner == false)
                return;
            
            currentHP += amount;
            currentHP = Mathf.Min(currentHP, maxHP);
            onHealEvent?.Broadcast(currentHP);
        }

        public void InflictDamage(NetworkObject attacker, float damage, Vector3 point, Vector3 normal)
        {
            AttackParams attackParams = new AttackParams(attacker.OwnerClientId, damage, point, normal);
            onHitEvent?.Broadcast(attackParams, false);
        }

        private void HandleHit(AttackParams attackParams)
        {
            if(isDead)
                return;

            currentHP -= attackParams.Damage;
            onDamagedEvent?.Broadcast(attackParams, false);

            if(currentHP <= 0f)
            {
                isDead = true;
                onDeadEvent?.Broadcast();
            }
        }
    }
}
