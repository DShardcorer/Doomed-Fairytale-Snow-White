using EntitySystems.Stats;

namespace EntitySystems.Skill.PassiveSkills
{
    public class BodyControlPassiveSkill: PassiveSkill
    {
        private int strengthIncrease = 1;
        private int dexterityIncrease = 1;
        private int constitutionIncrease = 1;
        private StatModifier strengthModifier;
        private StatModifier dexterityModifier;
        private StatModifier constitutionModifier;
        public BodyControlPassiveSkill(SkillInfoSO skillInfo) : base(skillInfo)
        {
            strengthModifier = new StatModifier(StatType.Strength, StatModifierType.Flat, strengthIncrease);
            dexterityModifier = new StatModifier(StatType.Dexterity, StatModifierType.Flat, dexterityIncrease);
            constitutionModifier = new StatModifier(StatType.Constitution, StatModifierType.Flat, constitutionIncrease);
        }

        public override void ApplyEffect()
        {
            statSystem.AddAbilityModifier(strengthModifier);
            statSystem.AddAbilityModifier(dexterityModifier);
            statSystem.AddAbilityModifier(constitutionModifier);
        }

        public override void UnapplyEffect()
        {
            statSystem.RemoveAbilityModifier(strengthModifier);
            statSystem.RemoveAbilityModifier(dexterityModifier);
            statSystem.RemoveAbilityModifier(constitutionModifier);
        }
    }
}