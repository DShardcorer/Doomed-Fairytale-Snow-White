using System.Collections.Generic;

public enum StatType { Constitution, Strength, Dexterity, Intelligence, Wisdom, Charisma, Health, Damage, Defense }

public static class AbilityStatCalculator
{
    public static AbilityStatBoard ApplyModifiers(AbilityStatBoard baseStats, List<StatModifier> modifiers)
    {
        // Start with base values.
        float newCon = baseStats.Constitution.BaseValue;
        float newStr = baseStats.Strength.BaseValue;
        float newDex = baseStats.Dexterity.BaseValue;
        float newInt = baseStats.Intelligence.BaseValue;
        float newWis = baseStats.Wisdom.BaseValue;
        float newCha = baseStats.Charisma.BaseValue;

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
        return new AbilityStatBoard(newCon, newStr, newDex, newInt, newWis, newCha);
    }
}
