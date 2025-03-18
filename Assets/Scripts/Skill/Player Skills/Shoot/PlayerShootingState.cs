using UnityEngine;

public class PlayerShootingState : PlayerState
{
    private PlayerShootingProperties _playerShootingProperties;
    public PlayerShootingState(string animationBoolName, PlayerShootingProperties playerShootingProperties) : base(animationBoolName)
    {
        _playerShootingProperties = playerShootingProperties;
    }

    public override void EnterState()
    {
        Debug.Log($"Entering {this.GetType().Name}. Player: {_player}, Rigidbody: {_rigidbody}, StateMachine: {_stateMachine}");
        base.EnterState();
        _player.IsBusy = true;
        
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
        if (!_isAnimationEnded)
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
