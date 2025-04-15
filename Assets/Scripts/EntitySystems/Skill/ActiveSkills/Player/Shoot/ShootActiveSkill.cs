using Entity.Player;
using UnityEngine;

namespace EntitySystems.Skill.ActiveSkills.Player.Shoot
{
    public class ShootActiveSkill : ActiveSkill
    {
        private PlayerShootingState _shootingState;
        public ShootActiveSkill(string skillName, float cooldown, 
            float healthCost, float manaCost, float staminaCost,
            PlayerShootingState playerShootingState) 
            : base(skillName, cooldown, healthCost ,manaCost, staminaCost)
        {
            _shootingState = playerShootingState;
        }
        public override void Initialize(ActiveSkillSystem parent)
        {
            base.Initialize(parent);
            if (base.parent.Parent is Entity.Player.Player player)
            {
                _shootingState.Initialize(player);
            }
            else
            {
                Debug.LogError($"ShootSkill initialized with a non-Player entity: {base.parent.Parent}");
            }

        }


        public override void UpdateLogic()
        {
            base.UpdateLogic();
        }
        protected override void UseSkill()
        {
            base.UseSkill();
            parent.StateMachine.ChangeState(_shootingState);
        }




    }
}
