using EntityBase.Player.State;
using UnityEngine;

namespace EntityBase.Player.Move
{
    public class PlayerMoveState : PlayerState
    {
        private PlayerMovingProperties _playerMovingProperties;

        public PlayerMoveState(string animationBoolName, PlayerMovingProperties entityStateProperties) : base(animationBoolName, entityStateProperties)
        {
            _playerMovingProperties = entityStateProperties;
        }

        public override void FixedUpdateState()
        {
            base.FixedUpdateState();
            if(_inputManager.GetMovementVector() != Vector2.zero)
            {
                _rigidbody.linearVelocity = _inputManager.GetMovementVector() * _playerMovingProperties.MoveSpeed;
                _player.PlayerProperties.lastMovementVector = _inputManager.GetMovementVector();
                _player.AttackHitbox.SetAttackHitBoxRotation(_player.PlayerProperties.lastMovementVector);
                _player.PlayerInteraction.SetInteractRotation(_player.PlayerProperties.lastMovementVector);
            
            }
            else
            {
                _stateMachine.ChangeState(_entity.IdleState);
            }
            base.FixedUpdateState();

        }


    }
}
