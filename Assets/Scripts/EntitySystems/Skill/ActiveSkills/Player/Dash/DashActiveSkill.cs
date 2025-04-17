using Entity.Player;
using UnityEngine;

namespace EntitySystems.Skill.ActiveSkills.Player.Dash
{
    public class DashActiveSkill : ActiveSkill
    {
        private PlayerDashingState _dashingState;


        public DashActiveSkill(ActiveSkillInfoSO activeSkillInfoSO, PlayerDashingState dashingState) : base(
            activeSkillInfoSO)
        {
            _dashingState = dashingState;
        }

        public override void Initialize(ActiveSkillSystem parent)
        {
            base.Initialize(parent);
            if (base.parent.Parent is Entity.Player.Player player)
            {
                _dashingState.Initialize(player);
            }
            else
            {
                Debug.LogError($"DashSkill initialized with a non-Player entity: {base.parent.Parent}");
            }
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