using System.Collections.Generic;

public static class CombatStatCalculator
{
    /// <summary>
    /// Calculates combat stats based on ability stats and combat modifiers.
    /// </summary>
    public static CombatStatBoard Calculate(AbilityStatBoard abilityStats, AttackStatType preferredAttackStat, List<StatModifier> combatModifiers = null)
    {
        // Base formulas (adjust these formulas as needed)
        float baseHealth = abilityStats.Constitution.BaseValue * 10;
        float basePhysicalAttack = (preferredAttackStat == AttackStatType.Dexterity)
            ? abilityStats.Dexterity.BaseValue
            : abilityStats.Strength.BaseValue;
        float baseMagicAttack = abilityStats.Intelligence.BaseValue;
        float basePhysicalDefense = abilityStats.Constitution.BaseValue;
        float baseMagicalDefense = abilityStats.Wisdom.BaseValue;

        // Apply combat modifiers to each stat.
        float finalHealth = ApplyModifiersToCombatStat(baseHealth, combatModifiers, StatType.Health);
        float finalPhysicalAttack = ApplyModifiersToCombatStat(basePhysicalAttack, combatModifiers, StatType.Damage);
        float finalMagicAttack = ApplyModifiersToCombatStat(baseMagicAttack, combatModifiers, StatType.Damage);
        float finalPhysicalDefense = ApplyModifiersToCombatStat(basePhysicalDefense, combatModifiers, StatType.Defense);
        float finalMagicalDefense = ApplyModifiersToCombatStat(baseMagicalDefense, combatModifiers, StatType.Defense);

        return new CombatStatBoard(
            new CombatStat(finalHealth),
            new CombatStat(finalPhysicalAttack),
            new CombatStat(finalMagicAttack),
            new CombatStat(finalPhysicalDefense),
            new CombatStat(finalMagicalDefense)
        );
    }

    private static float ApplyModifiersToCombatStat(float baseValue, List<StatModifier> modifiers, StatType statType)
    {
        float finalValue = baseValue;
        float percentIncrease = 0f;

        if (modifiers != null)
        {
            foreach (var mod in modifiers)
            {
                if (mod.StatType == statType)
                {
                    if (mod.ModifierType == ModifierType.Flat)
                        finalValue += mod.Value;
                    else if (mod.ModifierType == ModifierType.Percentage)
                        percentIncrease += mod.Value;
                }
            }
        }
        finalValue *= (1 + percentIncrease / 100f);
        return finalValue;
    }
}
