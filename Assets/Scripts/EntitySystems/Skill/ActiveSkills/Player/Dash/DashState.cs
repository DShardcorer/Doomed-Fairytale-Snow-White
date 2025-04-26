using Entity.Player.State;
using Helpers;

namespace EntitySystems.Skill.ActiveSkills.Player.Dash
{
    public class DashState : PlayerState
    {
        protected DashProperties DashProperties;

        public DashState(DashActiveSkillInfoSO activeSkillInfoSO)
            : this(HelperAnimationStateName.IS_DASHING, new DashProperties(activeSkillInfoSO))
        {
        }


        private DashState(string animationBoolName, DashProperties entityStateProperties) : base(
            animationBoolName, entityStateProperties)
        {
            DashProperties = entityStateProperties;
        }

        public override void EnterState()
        {
            base.EnterState();
            _player.IsBusy = true;
        }

        public override void FixedUpdateState()
        {
            base.FixedUpdateState();
            if (!_isAnimationEnded)
            {
                _rigidbody.linearVelocity =
                    _entity.Properties.lastMovementVector * DashProperties.DashSpeed;
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