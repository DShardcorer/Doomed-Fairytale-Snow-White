using EntitySystems.Stats;

namespace EntitySystems.Skill.PassiveSkills
{
    public class NaturalStrengthPassiveSkill:PassiveSkill
    {
        private int strengthIncrease = 3;
        private StatModifier modifier;
        public NaturalStrengthPassiveSkill(SkillInfoSO skillInfo) : base(skillInfo)
        {
            modifier = new StatModifier(StatType.Strength, ModifierType.Flat, strengthIncrease);
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