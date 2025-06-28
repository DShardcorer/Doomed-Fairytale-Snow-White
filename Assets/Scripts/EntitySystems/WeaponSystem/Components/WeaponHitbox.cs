using System;
using EntitySystems.WeaponSystem.Components.ComponentData;
using UnityEngine;
using Utility;

namespace EntitySystems.WeaponSystem.Components
{
    public class WeaponHitbox : WeaponComponent, IDrawGizmo
    {
        private WeaponHitboxData _data;
        public Action<Collider2D[]> OnDamagableHitboxesDetected;

        public override void Initialize(Weapon parent)
        {
            base.Initialize(parent);
            _data = parent.View.WeaponData.GetComponentData<WeaponHitboxData>();

            _entityAnimationTriggers.OnTakingEffect += HandleAttackTakingEffect;
            GizmoDrawer.Instance.AddDrawGizmoObject(this);
        }

        public override void Dispose()
        {
            base.Dispose();
            _entityAnimationTriggers.OnTakingEffect -= HandleAttackTakingEffect;
            _data = null;
            GizmoDrawer.Instance.RemoveDrawGizmoObject(this);
        }

        private Vector2 offset;

        private void HandleAttackTakingEffect()
        {
            offset.Set(
                _entityView.transform.position.x + (_data.Hitboxes[_entity.CurrentAttackCounter()].HitboxRect.x *
                                                    _entity.Properties.CardinalizedLastMovementVector().x),
                _entityView.transform.position.y + (_data.Hitboxes[_entity.CurrentAttackCounter()].HitboxRect.y *
                                                    _entity.Properties.CardinalizedLastMovementVector().y)
            );

            Collider2D[] colliders = Physics2D.OverlapBoxAll(
                offset,
                _data.Hitboxes[_entity.CurrentAttackCounter()].HitboxRect.size,
                0f,
                _data.HitboxLayers
            );
            if (colliders.Length == 0)
            {
                return;
            }
            // OnDamagableHitboxesDetected.Invoke(colliders);
            //print out all hit colliders names
            Debug.LogWarning("Hitbox detected: " + colliders[0].gameObject.name);
        }

        public void DrawGizmoSelected()
        {
            return;
        }

        public void DrawGizmo()
        {
            if (_data == null)
            {
                return;
            }

            foreach (var item in _data.Hitboxes)
            {
                if (!item.DrawGizmo)
                {
                    return;
                }
                Vector2 offset = new Vector2(
                    _entityView.transform.position.x + (item.HitboxRect.x * _entity.Properties.CardinalizedLastMovementVector().x),
                    _entityView.transform.position.y + (item.HitboxRect.y * _entity.Properties.CardinalizedLastMovementVector().y)
                );
                Gizmos.DrawWireCube(
                    offset,
                    item.HitboxRect.size
                );
            }
        }
    }
}