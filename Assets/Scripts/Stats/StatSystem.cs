using System.Collections.Generic;
using UnityEngine;

public class StatSystem : ILifecycle<Entity>
{
    private Entity _parent;
    public Entity Entity => _parent;
    public AbilityStatBoard BaseAbilityStats { get; private set; }
    // Ability stats after applying modifiers (e.g., buffs/debuffs/equipment affecting abilities).
    public AbilityStatBoard CurrentAbilityStats { get; private set; }

    // Derived combat stats calculated from current ability stats without combat modifiers.
    public CombatStatBoard BaseCombatStats { get; private set; }
    // Final combat stats after applying combat-specific modifiers.
    public CombatStatBoard CombatStats { get; private set; }
    // Determines which ability stat is used for physical attack.
    public AttackStatType PreferredAttackStat { get; set; }

    private List<StatModifier> _abilityModifiers = new List<StatModifier>();
    private List<StatModifier> _combatModifiers = new List<StatModifier>();

    public StatSystem(AbilityStatBoard baseStats, AttackStatType preferredAttackStat)
    {
        BaseAbilityStats = baseStats;
        PreferredAttackStat = preferredAttackStat;
        RecalculateStats();
    }
    public void Initialize(Entity parent)
    {
        _parent = parent;
    }

    public void Dispose()
    {
        _parent = null;
    }
    public void RecalculateStats()
    {
        CurrentAbilityStats = AbilityStatCalculator.ApplyModifiers(BaseAbilityStats, _abilityModifiers);
        BaseCombatStats = CombatStatCalculator.Calculate(CurrentAbilityStats, PreferredAttackStat, null);
        CombatStats = CombatStatCalculator.Calculate(CurrentAbilityStats, PreferredAttackStat, _combatModifiers);
        // Optionally, trigger events for UI updates.
    }

    public void AddAbilityModifier(StatModifier modifier)
    {
        _abilityModifiers.Add(modifier);
        RecalculateStats();
    }

    public void RemoveAbilityModifier(StatModifier modifier)
    {
        _abilityModifiers.Remove(modifier);
        RecalculateStats();
    }

    public void AddCombatModifier(StatModifier modifier)
    {
        _combatModifiers.Add(modifier);
        RecalculateStats();
    }

    public void RemoveCombatModifier(StatModifier modifier)
    {
        _combatModifiers.Remove(modifier);
        RecalculateStats();
    }


}
