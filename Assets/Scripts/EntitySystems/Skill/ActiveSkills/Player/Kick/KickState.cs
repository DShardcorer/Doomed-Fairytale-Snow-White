using EntityBase;
using EntityBase.Player.State;
using EntitySystems.Skill.ActiveSkills.Player.Shoot;
using Helpers;

namespace EntitySystems.Skill.ActiveSkills.Player.Kick
{
    public class KickState : PlayerState
    {
        public KickState(KickActiveSkillInfoSO activeSkillInfoSO)
            : this(HelperAnimationStateName.IS_USING_SKILL_KICK, new KickProperties(activeSkillInfoSO))
        {
        }

        private KickProperties _kickProperties;

        public KickState(string animationBoolName, KickProperties entityStateProperties) : base(
            animationBoolName, entityStateProperties)
        {
            _kickProperties = entityStateProperties;
        }


        public override void EnterState()
        {
            base.EnterState();
            _entity.IsBusy = true;
            _entity.View.AddVelocity(_kickProperties.KickVelocity * _entity.Properties.lastMovementVector);
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
            _entity.AttackHitbox.PerformRaycastAttack(_kickProperties.KickDamage, _kickProperties.KickRange);
        }

        public override void ExitState()
        {
            _entity.IsBusy = false;
            _entity.View.RemoveVelocity();
            base.ExitState();
        }
    }
}