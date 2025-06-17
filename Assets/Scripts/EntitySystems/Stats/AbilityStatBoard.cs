
using System.Collections.Generic;

namespace EntitySystems.Stats
{
    public class AbilityStatBoard
    {
        public AbilityStat Strength { get; set; }
        public AbilityStat Dexterity { get; set; }
        public AbilityStat Constitution { get; set; }
        public AbilityStat Intelligence { get; set; }
        public AbilityStat Wisdom { get; set; }
        public AbilityStat Charisma { get; set; }

        public AbilityStatBoard(AbilityStatboardSO abilityStatboardSO)
        {
            Strength = new AbilityStat(abilityStatboardSO.Strength);
            Dexterity = new AbilityStat(abilityStatboardSO.Dexterity);
            Constitution = new AbilityStat(abilityStatboardSO.Constitution);
            Intelligence = new AbilityStat(abilityStatboardSO.Intelligence);
            Wisdom = new AbilityStat(abilityStatboardSO.Wisdom);
            Charisma = new AbilityStat(abilityStatboardSO.Charisma);
        }
        //A constructor with int parameters for each stat
        public AbilityStatBoard(float strength, float dexterity, float constitution, float intelligence, float wisdom, float charisma)
        {
            Strength = new AbilityStat(strength);
            Dexterity = new AbilityStat(dexterity);
            Constitution = new AbilityStat(constitution);
            Intelligence = new AbilityStat(intelligence);
            Wisdom = new AbilityStat(wisdom);
            Charisma = new AbilityStat(charisma);
        }

        public void SetStat(StatType statType, int points)
        {
            switch (statType)
            {
                case StatType.Strength:
                    Strength.BaseValue = points;
                    break;
                case StatType.Dexterity:
                    Dexterity.BaseValue = points;
                    break;
                case StatType.Constitution:
                    Constitution.BaseValue = points;
                    break;
                case StatType.Intelligence:
                    Intelligence.BaseValue = points;
                    break;
                case StatType.Wisdom:
                    Wisdom.BaseValue = points;
                    break;
                case StatType.Charisma:
                    Charisma.BaseValue = points;
                    break;
            }
        }
        public void IncreaseStat(StatType statType, int points)
        {
            switch (statType)
            {
                case StatType.Strength:
                    Strength.BaseValue += points;
                    break;
                case StatType.Dexterity:
                    Dexterity.BaseValue += points;
                    break;
                case StatType.Constitution:
                    Constitution.BaseValue += points;
                    break;
                case StatType.Intelligence:
                    Intelligence.BaseValue += points;
                    break;
                case StatType.Wisdom:
                    Wisdom.BaseValue += points;
                    break;
                case StatType.Charisma:
                    Charisma.BaseValue += points;
                    break;
            }
        }

        public void CalculateModified( List<StatModifier> modifiers)
        {
            // Start with base values.
            float newCon = Constitution.BaseValue;
            float newStr = Strength.BaseValue;
            float newDex = Dexterity.BaseValue;
            float newInt = Intelligence.BaseValue;
            float newWis = Wisdom.BaseValue;
            float newCha = Charisma.BaseValue;

            foreach (var mod in modifiers)
            {
                switch (mod.StatType)
                {
                    case StatType.Constitution:
                        newCon += mod.statModifierType == StatModifierType.Flat ? mod.Value : newCon * mod.Value / 100f;
                        break;
                    case StatType.Strength:
                        newStr += mod.statModifierType == StatModifierType.Flat ? mod.Value : newStr * mod.Value / 100f;
                        break;
                    case StatType.Dexterity:
                        newDex += mod.statModifierType == StatModifierType.Flat ? mod.Value : newDex * mod.Value / 100f;
                        break;
                    case StatType.Intelligence:
                        newInt += mod.statModifierType == StatModifierType.Flat ? mod.Value : newInt * mod.Value / 100f;
                        break;
                    case StatType.Wisdom:
                        newWis += mod.statModifierType == StatModifierType.Flat ? mod.Value : newWis * mod.Value / 100f;
                        break;
                    case StatType.Charisma:
                        newCha += mod.statModifierType == StatModifierType.Flat ? mod.Value : newCha * mod.Value / 100f;
                        break;

                }
            }
            Constitution.ModifiedValue = newCon;
            Strength.ModifiedValue = newStr;
            Dexterity.ModifiedValue = newDex;
            Intelligence.ModifiedValue = newInt;
            Wisdom.ModifiedValue = newWis;
            Charisma.ModifiedValue = newCha;
        }

        public override string ToString()
        {
            return $"Strength: {Strength.ModifiedValue} (+ {Strength.ModifiedValue - Strength.BaseValue}) \n" +
                   $"Dexterity: {Dexterity.ModifiedValue} (+ {Dexterity.ModifiedValue - Dexterity.BaseValue}) \n" +
                   $"Constitution: {Constitution.ModifiedValue} (+ {Constitution.ModifiedValue - Constitution.BaseValue}) \n" +
                   $"Intelligence: {Intelligence.ModifiedValue} (+ {Intelligence.ModifiedValue - Intelligence.BaseValue}) \n" +
                   $"Wisdom: {Wisdom.ModifiedValue} (+ {Wisdom.ModifiedValue - Wisdom.BaseValue}) \n" +
                   $"Charisma: {Charisma.ModifiedValue} (+ {Charisma.ModifiedValue - Charisma.BaseValue})";
        }
    }
}
