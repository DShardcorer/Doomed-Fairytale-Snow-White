using System;
using EntityBase.Player.State;
using Helpers;

namespace EntitySystems.Skill.ActiveSkills.Player.Shoot
{
    public class ShootState : PlayerState
    {
        private ShootProperties _shootProperties;
        public ShootState(ShootActiveSkillInfoSO activeSkillInfoSO)
            : this(HelperAnimationStateName.IS_SHOOTING, new ShootProperties(activeSkillInfoSO))
        {
        }

        private ShootState(string animationBoolName, ShootProperties entityStateProperties) : base(animationBoolName, entityStateProperties)
        {
            _shootProperties = entityStateProperties;
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
                _stateMachine.ChangeState(_entity.IdleState);
            }
        }
        protected override void OnTakingEffect()
        {
            _entity.AttackHitbox.PerformRaycastAttack(_shootProperties.ShootDamage, _shootProperties.ShootRange);
        }
        public override void ExitState()
        {
            _entity.IsBusy = false;
            base.ExitState();
        }



    }
}
