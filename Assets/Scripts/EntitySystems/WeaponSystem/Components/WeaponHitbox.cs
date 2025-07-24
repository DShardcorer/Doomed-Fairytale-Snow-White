using System;
using System.Collections.Generic;
using EntitySystems.WeaponSystem.Components.ComponentData;
using EntitySystems.WeaponSystem.Components.ComponentData.AttackData;
using UnityEngine;
using Utilities;

namespace EntitySystems.WeaponSystem.Components
{
    public class WeaponHitbox : WeaponComponent, IDrawGizmo
    {
        private WeaponHitboxData _data;
        public Action<Collider2D[]> OnDamagableHitboxesDetected;

        public override void Initialize(Weapon parent)
        {
            base.Initialize(parent);
            _data = parent.View.WeaponData.GetBodyTypeHitboxData(_entity.View.BodyType);
            _entityAnimationTriggers.OnTakingEffect += HandleAttackTakingEffect;
            // GizmoDrawer.Instance.AddDrawGizmoObject(this);
        }

        public override void Dispose()
        {
            _entityAnimationTriggers.OnTakingEffect -= HandleAttackTakingEffect;
            _data = null;
            // GizmoDrawer.Instance.RemoveDrawGizmoObject(this);
            base.Dispose();
        }

        private void HandleAttackTakingEffect()
        {
            if (!_entity.IsAttacking())
            {
                return;
            }
            if (!_weapon.IsActive)
            {
                return;
            }
            int attackIndex = _entity.CurrentAttackCounter();
            Vector2 movementVector = _entity.Properties.CardinalizedLastMovementVector();
            Direction playerFacingDirection = GetDirectionFromVector(movementVector);
            
            // Use HashSet to store unique colliders
            HashSet<Collider2D> uniqueColliders = new HashSet<Collider2D>();

            // Find hitboxes matching current player direction
            foreach (HitboxPerAttackDirection hitboxDirection in _data.Hitboxes[attackIndex].DirectionalHitboxes)
            {
                if (hitboxDirection.Direction != playerFacingDirection)
                    continue;

                // Calculate position based on player position and hitbox rect
                Vector2 position = new Vector2(
                    _entityView.transform.position.x + hitboxDirection.HitboxRect.x,
                    _entityView.transform.position.y + hitboxDirection.HitboxRect.y
                );

                // Check for collisions using the angle property
                Collider2D[] colliders = Physics2D.OverlapBoxAll(
                    position,
                    hitboxDirection.HitboxRect.size,
                    hitboxDirection.Angle,
                    _data.HitboxLayers
                );

                // Add all colliders to the HashSet (duplicates will be automatically ignored)
                foreach (Collider2D collider in colliders)
                {
                    uniqueColliders.Add(collider);
                }
            }

            // If we found any colliders, invoke the callback with the unique colliders
            if (uniqueColliders.Count > 0)
            {
                Collider2D[] uniqueCollidersArray = new Collider2D[uniqueColliders.Count];
                uniqueColliders.CopyTo(uniqueCollidersArray);
                
                OnDamagableHitboxesDetected?.Invoke(uniqueCollidersArray);
                
                if (uniqueCollidersArray.Length > 0)
                {
                    Debug.LogWarning("Hitbox detected: " + uniqueCollidersArray[0].gameObject.name);
                }
            }
        }

        private Direction GetDirectionFromVector(Vector2 vector)
        {
            if (vector.y < 0) return Direction.Down;
            if (vector.y > 0) return Direction.Up;
            if (vector.x < 0) return Direction.Left;
            if (vector.x > 0) return Direction.Right;

            // Default to Down if no movement
            return Direction.Down;
        }

        public void DrawGizmoSelected()
        {
            return;
        }

        public void DrawGizmo()
        {
            if (_data == null || _entity == null)
            {
                return;
            }

            Vector2 movementVector = _entity.Properties.CardinalizedLastMovementVector();
            Direction playerFacingDirection = GetDirectionFromVector(movementVector);

            foreach (var hitboxPerAttack in _data.Hitboxes)
            {
                foreach (var hitboxDirection in hitboxPerAttack.DirectionalHitboxes)
                {
                    if (!hitboxDirection.DrawGizmo)
                        continue;

                    // Only draw hitboxes for current direction
                    if (hitboxDirection.Direction == playerFacingDirection)
                    {
                        Vector2 position = new Vector2(
                            _entityView.transform.position.x + hitboxDirection.HitboxRect.x,
                            _entityView.transform.position.y + hitboxDirection.HitboxRect.y
                        );

                        // Draw rotated hitbox
                        DrawRotatedGizmoCube(position, hitboxDirection.HitboxRect.size, hitboxDirection.Angle);
                    }
                }
            }
        }

        private void DrawRotatedGizmoCube(Vector2 center, Vector2 size, float angle)
        {
            Matrix4x4 originalMatrix = Gizmos.matrix;

            // Create rotation matrix
            Gizmos.matrix = Matrix4x4.TRS(
                center,
                Quaternion.Euler(0, 0, angle),
                Vector3.one
            );

            // Draw the cube with rotation applied
            Gizmos.DrawWireCube(Vector3.zero, size);

            // Restore original matrix
            Gizmos.matrix = originalMatrix;
        }
    }
}