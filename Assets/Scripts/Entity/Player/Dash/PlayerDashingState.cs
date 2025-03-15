using UnityEngine;

public class PlayerDashingState : PlayerState
{
    protected PlayerDashingProperties _playerDashingProperties;
    public PlayerDashingState(PlayerDashingProperties playerDashingProperties, string animationBoolName) : base(animationBoolName)
    {
        _playerDashingProperties = playerDashingProperties;
    }
    public override void EnterState()
    {
        base.EnterState();
        _player.IsBusy = true;
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
        if(!_isAnimationEnded)
        {
            _rigidbody.linearVelocity =  _player.PlayerProperties.lastMovementVector * _playerDashingProperties.DashSpeed;
        }
        else
        {
            _stateMachine.ChangeState(_player.PlayerIdlingState);
        }
    }

    public override void ExitState()
    {
        _player.IsBusy = false;
        base.ExitState();
    }

}
