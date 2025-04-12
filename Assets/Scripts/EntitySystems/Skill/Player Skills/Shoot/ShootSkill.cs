using Entity.Player;
using UnityEngine;

namespace EntitySystems.Skill.Player_Skills.Shoot
{
    public class ShootSkill : Skill
    {
        private PlayerShootingState _shootingState;
        public ShootSkill(string skillName, float cooldown, 
            float healthCost, float manaCost, float staminaCost,
            PlayerShootingState playerShootingState) 
            : base(skillName, cooldown, healthCost ,manaCost, staminaCost)
        {
            _shootingState = playerShootingState;
        }
        public override void Initialize(SkillSystem parent)
        {
            base.Initialize(parent);
            if (_parent.Parent is Player player)
            {
                _shootingState.Initialize(player);
            }
            else
            {
                Debug.LogError($"ShootSkill initialized with a non-Player entity: {_parent.Parent}");
            }

        }


        public override void UpdateLogic()
        {
            base.UpdateLogic();
        }
        protected override void UseSkill()
        {
            base.UseSkill();
            _parent.StateMachine.ChangeState(_shootingState);
        }




    }
}
