using UnityEngine;

public class PlayerAttackingState : PlayerState
{
    private PlayerAttackingProperties _playerAttackProperties;
    public PlayerAttackingState(PlayerAttackingProperties playerAttackProperties, string animationBoolName) : base(animationBoolName)
    {
        _playerAttackProperties = playerAttackProperties;
    }
    public override void EnterState()
    {
        base.EnterState();
        _player.IsBusy = true;
        _rigidbody.linearVelocity = _playerAttackProperties.AttackVelocity * _player.PlayerProperties.lastMovementVector;
    }
    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
        if(!_isAnimationEnded)
        {
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
