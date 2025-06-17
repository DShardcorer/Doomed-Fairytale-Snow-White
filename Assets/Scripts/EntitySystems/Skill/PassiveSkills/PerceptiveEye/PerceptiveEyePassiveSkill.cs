using EntitySystems.Stats;

namespace EntitySystems.Skill.PassiveSkills.PerceptiveEye
{
    public class PerceptiveEyePassiveSkill: PassiveSkill
    {
        private int wisdomIncrease = 1;
        private StatModifier modifier;
        public PerceptiveEyePassiveSkill(SkillInfoSO skillInfo) : base(skillInfo)
        {
            modifier = new StatModifier(StatType.Wisdom, StatModifierType.Flat, wisdomIncrease);
        }

        public override void ApplyEffect()
        {
            statSystem.AddAbilityModifier(modifier);
        }

        public override void UnapplyEffect()
        {
            statSystem.RemoveAbilityModifier(modifier);
        }
    }
}