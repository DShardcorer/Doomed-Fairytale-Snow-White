using EntitySystems.Stats;

namespace EntitySystems.Skill.PassiveSkills.Flirt
{
    public class FlirtPassiveSkill: PassiveSkill
    {
        private int charismaIncrease = 1;
        private StatModifier modifier;
        public FlirtPassiveSkill(SkillInfoSO skillInfo) : base(skillInfo)
        {
            modifier = new StatModifier(StatType.Charisma, ModifierType.Flat, charismaIncrease);
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