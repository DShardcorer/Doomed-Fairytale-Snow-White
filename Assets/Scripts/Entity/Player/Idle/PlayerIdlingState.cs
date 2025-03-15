using UnityEngine;

public class PlayerIdlingState : PlayerState
{
    public PlayerIdlingState(string animationBoolName) : base(animationBoolName)
    {
    }

    public override void FixedUpdateState()
    {
        if(_inputManager.GetMovementVector() != Vector2.zero)
        {
            _stateMachine.ChangeState(_player.PlayerMovingState);
        }
        base.FixedUpdateState();
    }

}
