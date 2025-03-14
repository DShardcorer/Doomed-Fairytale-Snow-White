using UnityEngine;

public class PlayerMovingState : PlayerState
{
    private PlayerMovingProperties _playerMovingProperties;
    public PlayerMovingState(PlayerMovingProperties playerMovingProperties, string animationBoolName) : base(animationBoolName)
    {
        _playerMovingProperties = playerMovingProperties;
    }

    public override void FixedUpdateState()
    {
        if(_inputManager.GetMovementVector() != Vector2.zero)
        {
            _rigidbody.linearVelocity = _inputManager.GetMovementVector() * _playerMovingProperties.MoveSpeed;
            _playerMovingProperties.lastMovementVector = _inputManager.GetMovementVector();
            _view.SetAnimationDirection(_playerMovingProperties.lastMovementVector);
        }
        else
        {
            _stateMachine.ChangeState(_player.PlayerIdlingState);
        }
        base.FixedUpdateState();

    }


}
