using System.Linq;
using EntitySystems.Stats;
using EntitySystems.WeaponSystem.Components.ComponentData;
using GeneralInterfaces;
using UnityEngine;

namespace EntitySystems.WeaponSystem.Components
{
    public class WeaponDamage : WeaponComponent
    {
        private WeaponDamageData _data;
        private WeaponHitbox _hitboxComponent;
        private StatSystem _entityStatSystem;

        public override void Initialize(Weapon parent)
        {
            base.Initialize(parent);
            _entityStatSystem = _entity.StatSystem;
            _data = parent.View.WeaponData.GetComponentData<WeaponDamageData>();
            _hitboxComponent = parent.Components
                .OfType<WeaponHitbox>()
                .FirstOrDefault();
            if (_hitboxComponent == null)
            {
                Debug.LogWarning(
                    "WeaponDamage: Hitbox component not found. Ensure WeaponHitbox is added to the weapon.");
                return;
            }

            _hitboxComponent.OnDamagableHitboxesDetected += HandleDamagableHitboxesDetected;
        }

        public override void Dispose()
        {
            base.Dispose();
            _entityStatSystem = null;
            if (_hitboxComponent != null)
            {
                _hitboxComponent.OnDamagableHitboxesDetected -= HandleDamagableHitboxesDetected;
            }

            _data = null;
            _hitboxComponent = null;
        }

        private void HandleDamagableHitboxesDetected(Collider2D[] obj)
        {
            if (!_weapon.IsActive)
            {
                return;
            }

            foreach (Collider2D collider in obj)
            {
                if (collider.TryGetComponent(out IDamagable damagable))
                {
                    float damageAmount = _data.Damages[_entity.CurrentAttackCounter()].Coefficient *
                                         _entityStatSystem.CombatStatBoard.PhysicalAttack.ModifiedValue;
                    // Apply damage to the damagable entity
                    damagable.TakeDamage(damageAmount, _entity);
                    Debug.Log($"WeaponDamage: Applied {damageAmount} damage to {damagable}");
                }
                else
                {
                    Debug.LogWarning("WeaponDamage: Collider does not implement IDamagable interface.");
                }
            }
        }
    }
}