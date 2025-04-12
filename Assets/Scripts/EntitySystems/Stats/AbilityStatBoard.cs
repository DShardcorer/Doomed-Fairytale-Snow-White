
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
                        newCon += mod.ModifierType == ModifierType.Flat ? mod.Value : newCon * mod.Value / 100f;
                        break;
                    case StatType.Strength:
                        newStr += mod.ModifierType == ModifierType.Flat ? mod.Value : newStr * mod.Value / 100f;
                        break;
                    case StatType.Dexterity:
                        newDex += mod.ModifierType == ModifierType.Flat ? mod.Value : newDex * mod.Value / 100f;
                        break;
                    case StatType.Intelligence:
                        newInt += mod.ModifierType == ModifierType.Flat ? mod.Value : newInt * mod.Value / 100f;
                        break;
                    case StatType.Wisdom:
                        newWis += mod.ModifierType == ModifierType.Flat ? mod.Value : newWis * mod.Value / 100f;
                        break;
                    case StatType.Charisma:
                        newCha += mod.ModifierType == ModifierType.Flat ? mod.Value : newCha * mod.Value / 100f;
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
