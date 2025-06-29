using EntityBase;
using EntitySystems.WeaponSystem.Components.ComponentData;
using EntitySystems.WeaponSystem.Components.ComponentData.AttackData;
using UnityEngine;
using Utility;

namespace EntitySystems.WeaponSystem.Components
{
    public class WeaponMovement : WeaponComponent
    {
        private WeaponMovementData _weaponMovementData;

        public override void Initialize(Weapon parent)
        {
            base.Initialize(parent);
            _entityAnimationTriggers.OnAttackMovementStart += HandleAttackMovementStart;
            _entityAnimationTriggers.OnAttackMovementEnd += HandleAttackMovementEnd;
            _weaponMovementData = _weapon.View.WeaponData.GetComponentData<WeaponMovementData>();
        }

        public override void Dispose()
        {
            base.Dispose();
            _entityAnimationTriggers.OnAttackMovementStart -= HandleAttackMovementStart;
            _entityAnimationTriggers.OnAttackMovementEnd -= HandleAttackMovementEnd;
            _weaponMovementData = null;
        }


        private void HandleAttackMovementStart()
        {
            if (!_entity.IsAttacking())
            {
                return;
            }

            if (!_weapon.IsActive)
            {
                return;
            }
            Vector2 cardinalizedMovementVector = _entity.Properties.CardinalizedLastMovementVector();

            DirectionComparedToFacingDirection movementDirectionComparedToCardinalizedMovementVector =
                _weaponMovementData.Movements[_entity.CurrentAttackCounter()].DirectionCompareToEntityLastMovementVector2;
    
            // Calculate movement vector based on direction and entity's last movement
            Vector2 movementVector = CalculateMovementVector(cardinalizedMovementVector, movementDirectionComparedToCardinalizedMovementVector);
    
            // Get the movement speed from the current attack data
            float movementSpeed = _weaponMovementData.Movements[_entity.CurrentAttackCounter()].Velocity;
    
            // Apply velocity to entity
            _entity.View.AddVelocity(movementVector * movementSpeed);
        }

        private Vector2 CalculateMovementVector(Vector2 baseDirection, DirectionComparedToFacingDirection directionComparedToFacingDirection)
        {
            switch (directionComparedToFacingDirection)
            {
                case DirectionComparedToFacingDirection.Forward:
                    return baseDirection;
                case DirectionComparedToFacingDirection.Backward:
                    return -baseDirection;
                case DirectionComparedToFacingDirection.Left:
                    return new Vector2(-baseDirection.y, baseDirection.x);
                case DirectionComparedToFacingDirection.Right:
                    return new Vector2(baseDirection.y, -baseDirection.x);
                default:
                    return Vector2.zero;
            }
        }
        private void HandleAttackMovementEnd()
        {
            _entity.View.RemoveVelocity();
        }
    }
}