using System.Collections.Generic;

public enum AttackStatType { Strength, Dexterity }

public class CombatStatBoard
{
    public CombatStat Health { get; set; }
    public CombatStat Mana { get; set; }
    public CombatStat Stamina { get; set; }
    public CombatStat PhysicalAttack { get; set; }
    public CombatStat MagicAttack { get; set; }
    public CombatStat PhysicalDefense { get; set; }
    public CombatStat MagicalDefense { get; set; }



    public CombatStatBoard(AbilityStatBoard abilityStats, AttackStatType preferredAttackStat)
    {
        Health = new CombatStat();
        Mana = new CombatStat();
        Stamina = new CombatStat();
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
        float baseHealth = abilityStats.Constitution.ModifiedValue * 10;
        float baseMana = abilityStats.Intelligence.ModifiedValue * 5;
        float baseStamina = abilityStats.Constitution.ModifiedValue * 5;
        float basePhysicalAttack = (preferredAttackStat == AttackStatType.Dexterity)
            ? abilityStats.Dexterity.ModifiedValue
            : abilityStats.Strength.ModifiedValue;
        float baseMagicAttack = abilityStats.Intelligence.ModifiedValue;
        float basePhysicalDefense = abilityStats.Constitution.ModifiedValue;
        float baseMagicalDefense = abilityStats.Wisdom.ModifiedValue;





        float finalHealth = ApplyModifiersToCombatStat(baseHealth, combatModifiers, StatType.Health);
        float finalMana = ApplyModifiersToCombatStat(baseMana, combatModifiers, StatType.Mana);
        float finalStamina = ApplyModifiersToCombatStat(baseStamina, combatModifiers, StatType.Stamina);
        float finalPhysicalAttack = ApplyModifiersToCombatStat(basePhysicalAttack, combatModifiers, StatType.Damage);
        float finalMagicAttack = ApplyModifiersToCombatStat(baseMagicAttack, combatModifiers, StatType.Damage);
        float finalPhysicalDefense = ApplyModifiersToCombatStat(basePhysicalDefense, combatModifiers, StatType.Defense);
        float finalMagicalDefense = ApplyModifiersToCombatStat(baseMagicalDefense, combatModifiers, StatType.Defense);



        Health.ModifiedValue = finalHealth;
        Mana.ModifiedValue = finalMana;
        Stamina.ModifiedValue = finalStamina;
        PhysicalAttack.ModifiedValue = finalPhysicalAttack;
        MagicAttack.ModifiedValue = finalMagicAttack;
        PhysicalDefense.ModifiedValue = finalPhysicalDefense;
        MagicalDefense.ModifiedValue = finalMagicalDefense;


    }

    public void CalculateBase(AbilityStatBoard abilityStats, AttackStatType preferredAttackStat)
    {
        float baseHealth = abilityStats.Constitution.ModifiedValue * 10;
        float baseMana = abilityStats.Intelligence.ModifiedValue * 5;
        float baseStamina = abilityStats.Constitution.ModifiedValue * 5;
        float basePhysicalAttack = (preferredAttackStat == AttackStatType.Dexterity)
            ? abilityStats.Dexterity.ModifiedValue
            : abilityStats.Strength.ModifiedValue;
        float baseMagicAttack = abilityStats.Intelligence.ModifiedValue;
        float basePhysicalDefense = 5;
        float baseMagicalDefense = abilityStats.Wisdom.ModifiedValue;




        Health.BaseValue = baseHealth;
        Mana.BaseValue = baseMana;
        Stamina.BaseValue = baseStamina;
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
        finalValue *= 1 + percentIncrease / 100f;
        return finalValue;
    }


    public override string ToString()
    {
        return $"Health: {Health.ModifiedValue} (+ {Health.ModifiedValue - Health.BaseValue}) \n" +
                $"Mana: {Mana.ModifiedValue} (+ {Mana.ModifiedValue - Mana.BaseValue}) \n" +
                $"Stamina: {Stamina.ModifiedValue} (+ {Stamina.ModifiedValue - Stamina.BaseValue}) \n" +
                $"Physical Attack: {PhysicalAttack.ModifiedValue} (+ {PhysicalAttack.ModifiedValue - PhysicalAttack.BaseValue}) \n" +
                $"Magic Attack: {MagicAttack.ModifiedValue} (+ {MagicAttack.ModifiedValue - MagicAttack.BaseValue}) \n" +
                $"Physical Defense: {PhysicalDefense.ModifiedValue} (+ {PhysicalDefense.ModifiedValue - PhysicalDefense.BaseValue}) \n" +
                $"Magical Defense: {MagicalDefense.ModifiedValue} (+ {MagicalDefense.ModifiedValue - MagicalDefense.BaseValue}) \n";
    }
}
