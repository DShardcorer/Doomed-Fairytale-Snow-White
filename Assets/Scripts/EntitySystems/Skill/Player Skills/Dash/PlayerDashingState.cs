

using Entity.Player.State;

namespace EntitySystems.Skill.Player_Skills.Dash
{
    public class PlayerDashingState : PlayerState
    {
        protected PlayerDashingProperties _playerDashingProperties;

        public PlayerDashingState(string animationBoolName, PlayerDashingProperties entityStateProperties) : base(animationBoolName, entityStateProperties)
        {
            _playerDashingProperties = entityStateProperties;
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
}
