using UnityEngine;

namespace EntitySystems.Skill.PassiveSkills
{
    [CreateAssetMenu(fileName = "NaturalStrengthPassiveSkillInfoSO", menuName = "SkillInfoSO/PassiveSkillInfoSO/NaturalStrengthPassiveSkillInfoSO")]
    public class NaturalStrengthPassiveSkillInfoSO: PassiveSkillInfoSO
    {
        public override PassiveSkill Create()
        {
            return new NaturalStrengthPassiveSkill(this);
        }
    }
}