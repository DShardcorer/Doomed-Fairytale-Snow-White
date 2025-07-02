using UnityEngine;

namespace EntitySystems.Skill.ActiveSkills.Player.Kick
{
    [CreateAssetMenu(fileName = "KickActiveSkillInfoSO", menuName = "SkillInfoSO/ActiveSkillInfoSO/KickActiveSkillInfoSO")]
    public class KickActiveSkillInfoSO: ActiveSkillInfoSO
    {
        public float KickDamage = 10f;
        public float KickVelocity = 5f;
        public float KickRange = 2f;
        public float KickKnockbackForce = 5f;
    }
}