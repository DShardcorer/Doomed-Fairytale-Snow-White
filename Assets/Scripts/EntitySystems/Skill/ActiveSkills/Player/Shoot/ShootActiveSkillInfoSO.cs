using UnityEngine;

namespace EntitySystems.Skill.ActiveSkills.Player.Shoot
{
    [CreateAssetMenu(fileName = "ShootActiveSkillInfoSO", menuName = "SkillInfoSO/ActiveSkillInfoSO/ShootActiveSkillInfoSO")]
    public class ShootActiveSkillInfoSO: ActiveSkillInfoSO
    {
        public float ShootDamage = 10f;
        public float ShootRange = 20f;
        public float ShootKnockbackForce = 5f;
        
        public override ActiveSkill Create()
        {
            return new ShootActiveSkill(this);
        }
    }
}