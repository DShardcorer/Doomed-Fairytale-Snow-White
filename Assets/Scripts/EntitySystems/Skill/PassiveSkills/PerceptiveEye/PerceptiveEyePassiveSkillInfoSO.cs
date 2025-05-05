using UnityEngine;

namespace EntitySystems.Skill.PassiveSkills.PerceptiveEye
{
    [CreateAssetMenu(fileName = "PerceptiveEyePassiveSkillInfoSO", menuName = "SkillInfoSO/PassiveSkillInfoSO/PerceptiveEyePassiveSkillInfoSO")]
    public class PerceptiveEyePassiveSkillInfoSO: PassiveSkillInfoSO
    {
        public override PassiveSkill Create()
        {
            return new PerceptiveEyePassiveSkill(this);
        }
    }
}