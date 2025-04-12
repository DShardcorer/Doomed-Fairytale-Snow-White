using Entity.Player;
using UnityEngine;

namespace EntitySystems.Skill.Player_Skills.Dash
{
    public class DashSkill : Skill
    {
        private PlayerDashingState _dashingState;
        public DashSkill(string skillName, float cooldown, float healthCost, float manaCost, float staminaCost,
            PlayerDashingState playerDashingState) 
            : base(skillName, cooldown, healthCost ,manaCost, staminaCost)
        {
            _dashingState = playerDashingState;
        }
        public override void Initialize(SkillSystem parent)
        {
            base.Initialize(parent);
            if (_parent.Parent is Player player)
            {
                _dashingState.Initialize(player);
            }
            else
            {
                Debug.LogError($"DashSkill initialized with a non-Player entity: {_parent.Parent}");
            }

        }


        public override void UpdateLogic()
        {
            base.UpdateLogic();
        }
        protected override void UseSkill()
        {
            base.UseSkill();
            _parent.StateMachine.ChangeState(_dashingState);
        }


    }
}
