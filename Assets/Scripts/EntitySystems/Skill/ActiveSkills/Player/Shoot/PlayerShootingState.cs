using System;
using Entity.Player.State;
using Helpers;

namespace EntitySystems.Skill.ActiveSkills.Player.Shoot
{
    public class PlayerShootingState : PlayerState
    {
        private PlayerShootingProperties _playerShootingProperties;
        public PlayerShootingState(ShootActiveSkillInfoSO activeSkillInfoSO)
            : this(HelperAnimationStateName.IS_SHOOTING, new PlayerShootingProperties(activeSkillInfoSO))
        {
        }

        private PlayerShootingState(string animationBoolName, PlayerShootingProperties entityStateProperties) : base(animationBoolName, entityStateProperties)
        {
            _playerShootingProperties = entityStateProperties;
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

            }
            else
            {
                _stateMachine.ChangeState(_player.PlayerIdlingState);
            }
        }
        protected override void OnTakingEffect(object sender, EventArgs e)
        {
            _entity.AttackHitbox.PerformRaycastAttack(_playerShootingProperties.ShootDamage, _playerShootingProperties.ShootRange);
        }
        public override void ExitState()
        {
            _player.IsBusy = false;
            base.ExitState();
        }



    }
}
