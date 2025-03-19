

using System.Collections.Generic;

public enum AttackStatType { Strength, Dexterity }
public class CombatStatBoard
{
    public CombatStat Health { get; set; }
    public CombatStat PhysicalAttack { get; set; }
    public CombatStat MagicAttack { get; set; }
    public CombatStat PhysicalDefense { get; set; }
    public CombatStat MagicalDefense { get; set; }


    public CombatStatBoard(AbilityStatBoard abilityStats, AttackStatType preferredAttackStat)
    {
        Health = new CombatStat();
        PhysicalAttack = new CombatStat();
        MagicAttack = new CombatStat();
        PhysicalDefense = new CombatStat();
        MagicalDefense = new CombatStat();
        CalculateBase(abilityStats, preferredAttackStat);
    }
    /// <summary>
    /// Calculates combat stats based on ability stats and combat modifiers.
    /// </summary>
    public void CalculateModified(AbilityStatBoard abilityStats, AttackStatType preferredAttackStat, List<StatModifier> combatModifiers = null)
    {
        // Base formulas (adjust these formulas as needed)
        float baseHealth = abilityStats.Constitution.ModifiedValue * 10;
        float basePhysicalAttack = (preferredAttackStat == AttackStatType.Dexterity)
            ? abilityStats.Dexterity.ModifiedValue
            : abilityStats.Strength.ModifiedValue;
        float baseMagicAttack = abilityStats.Intelligence.ModifiedValue;
        float basePhysicalDefense = abilityStats.Constitution.ModifiedValue;
        float baseMagicalDefense = abilityStats.Wisdom.ModifiedValue;

        // Apply combat modifiers to each stat.
        float finalHealth = ApplyModifiersToCombatStat(baseHealth, combatModifiers, StatType.Health);
        float finalPhysicalAttack = ApplyModifiersToCombatStat(basePhysicalAttack, combatModifiers, StatType.Damage);
        float finalMagicAttack = ApplyModifiersToCombatStat(baseMagicAttack, combatModifiers, StatType.Damage);
        float finalPhysicalDefense = ApplyModifiersToCombatStat(basePhysicalDefense, combatModifiers, StatType.Defense);
        float finalMagicalDefense = ApplyModifiersToCombatStat(baseMagicalDefense, combatModifiers, StatType.Defense);

        // Update combat stat board.
        Health.ModifiedValue = finalHealth;
        PhysicalAttack.ModifiedValue = finalPhysicalAttack;
        MagicAttack.ModifiedValue = finalMagicAttack;
        PhysicalDefense.ModifiedValue = finalPhysicalDefense;
        MagicalDefense.ModifiedValue = finalMagicalDefense;


    }

    public void CalculateBase(AbilityStatBoard abilityStats, AttackStatType preferredAttackStat)
    {
        // Base formulas (adjust these formulas as needed)
        float baseHealth = abilityStats.Constitution.ModifiedValue * 10;
        float basePhysicalAttack = (preferredAttackStat == AttackStatType.Dexterity)
            ? abilityStats.Dexterity.ModifiedValue
            : abilityStats.Strength.ModifiedValue;
        float baseMagicAttack = abilityStats.Intelligence.ModifiedValue;
        float basePhysicalDefense = abilityStats.Constitution.ModifiedValue;
        float baseMagicalDefense = abilityStats.Wisdom.ModifiedValue;

        // Update combat stat board.
        Health.BaseValue = baseHealth;
        PhysicalAttack.BaseValue = basePhysicalAttack;
        MagicAttack.BaseValue = baseMagicAttack;
        PhysicalDefense.BaseValue = basePhysicalDefense;
        MagicalDefense.BaseValue = baseMagicalDefense;
    }

    private float ApplyModifiersToCombatStat(float baseValue, List<StatModifier> modifiers, StatType statType)
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
    public override string ToString()
    {
        return $"Health: {Health.ModifiedValue}, Physical Attack: {PhysicalAttack.ModifiedValue}, Magic Attack: {MagicAttack.ModifiedValue}, Physical Defense: {PhysicalDefense.ModifiedValue}, Magical Defense: {MagicalDefense.ModifiedValue}";
    }
}
