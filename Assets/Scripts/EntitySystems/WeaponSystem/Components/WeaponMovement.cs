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
            Vector2 cardinalizedMovementVector = _entity.Properties.CardinalizedLastMovementVector();

            Direction movementDirectionComparedToCardinalizedMovementVector =
                _weaponMovementData.Movements[_entity.CurrentAttackCounter()].DirectionCompareToEntityLastMovementVector2;
    
            // Calculate movement vector based on direction and entity's last movement
            Vector2 movementVector = CalculateMovementVector(cardinalizedMovementVector, movementDirectionComparedToCardinalizedMovementVector);
    
            // Get the movement speed from the current attack data
            float movementSpeed = _weaponMovementData.Movements[_entity.CurrentAttackCounter()].Velocity;
    
            // Apply velocity to entity
            _entity.View.AddVelocity(movementVector * movementSpeed);
        }

        private Vector2 CalculateMovementVector(Vector2 baseDirection, Direction direction)
        {
            switch (direction)
            {
                case Direction.Forward:
                    return baseDirection;
                case Direction.Backward:
                    return -baseDirection;
                case Direction.Left:
                    return new Vector2(-baseDirection.y, baseDirection.x);
                case Direction.Right:
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