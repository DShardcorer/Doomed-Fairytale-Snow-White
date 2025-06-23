using EntityBase.Player.State;
using UnityEngine;

namespace EntityBase.Player.Idle
{
    public class PlayerIdleState : PlayerState
    {
        public PlayerIdleState(string animationBoolName, EntityStateProperties entityStateProperties) : base(animationBoolName, entityStateProperties)
        {
        }

        public override void FixedUpdateState()
        {
            if(_inputManager.GetMovementVector() != Vector2.zero)
            {
                _stateMachine.ChangeState(_player.PlayerMoveState);
            }
            base.FixedUpdateState();
        }

    }
}
