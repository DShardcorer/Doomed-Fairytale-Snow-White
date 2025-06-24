using EntityBase.Player.State;
using Helpers;
using UnityEngine;

namespace EntityBase.Player.Idle
{
    public class PlayerIdleState : PlayerState
    {
        public PlayerIdleState(EntityStateProperties entityStateProperties) : 
            this(HelperAnimationStateName.IS_IDLING,
            entityStateProperties)
        {
        }

        public PlayerIdleState(string animationBoolName, EntityStateProperties entityStateProperties) : base(
            animationBoolName, entityStateProperties)
        {
        }

        public override void FixedUpdateState()
        {
            if (_inputManager.GetMovementVector() != Vector2.zero)
            {
                _stateMachine.ChangeState(_player.PlayerMoveState);
            }

            base.FixedUpdateState();
        }
    }
}