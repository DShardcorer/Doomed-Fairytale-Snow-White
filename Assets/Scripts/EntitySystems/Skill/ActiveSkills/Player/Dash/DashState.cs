using EntityBase;
using EntityBase.NPC;
using EntityBase.Player;
using EntitySystems.Skill.ActiveSkills.Player.Dash;
using Helpers;
using UnityEngine;

namespace EntitySystems.States.Movement
{
    public class DashState : EntityState
    {
        protected DashProperties DashProperties;
        
        public DashState(DashActiveSkillInfoSO activeSkillInfoSO)
            : this(HelperAnimationStateName.IS_DASHING, new DashProperties(activeSkillInfoSO))
        {
        }
        
        protected DashState(string animationBoolName, DashProperties entityStateProperties) 
            : base(animationBoolName, entityStateProperties)
        {
            DashProperties = entityStateProperties;
        }
        
        public override void EnterState()
        {
            base.EnterState();
            _entity.IsBusy = true;
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
                _stateMachine.ChangeState(_entity.IdleState);
            }
        }
        
        public override void ExitState()
        {
            _entity.IsBusy = false;
            base.ExitState();
        }
    }
}