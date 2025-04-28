using Entity.Player;
using EntitySystems.States.Movement;
using UnityEngine;

namespace EntitySystems.Skill.ActiveSkills.Player.Dash
{
    public class DashActiveSkill : ActiveSkill
    {
        private DashState _dashingState;


        public DashActiveSkill(DashActiveSkillInfoSO activeSkillInfoSO) : base(
            activeSkillInfoSO)
        {
            _dashingState = new DashState(activeSkillInfoSO);
        }

        public override void Initialize(ActiveSkillSystem parent)
        {
            base.Initialize(parent);
            _dashingState.Initialize(parent.Parent);
        }


        public override void UpdateLogic()
        {
            base.UpdateLogic();
        }

        protected override void UseSkill()
        {
            base.UseSkill();
            parent.StateMachine.ChangeState(_dashingState);
        }
    }
}