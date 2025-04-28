using UnityEngine;

namespace EntitySystems.Skill.ActiveSkills.Player.Dash
{
    [CreateAssetMenu(fileName = "DashActiveSkillInfoSO", menuName = "SkillInfoSO/ActiveSkillInfoSO/DashActiveSkillInfoSO")]
    public class DashActiveSkillInfoSO: ActiveSkillInfoSO
    {
        public float DashSpeed = 10f;
        public override ActiveSkill Create()
        {
            return new DashActiveSkill(this);
        }
    }
}