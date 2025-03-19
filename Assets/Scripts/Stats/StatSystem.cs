using System.Collections.Generic;
using UnityEngine;

public class StatSystem : ILifecycle<Entity>
{
    private Entity _parent;
    public Entity Entity => _parent;
    public AbilityStatBoard AbilityStats { get; private set; }
    public CombatStatBoard CombatStats { get; private set; }
    public AttackStatType PreferredAttackStat { get; set; }

    private List<StatModifier> _abilityModifiers = new List<StatModifier>();
    private List<StatModifier> _combatModifiers = new List<StatModifier>();

    public StatSystem(AbilityStatBoard baseStats, AttackStatType preferredAttackStat)
    {
        AbilityStats = baseStats;
        PreferredAttackStat = preferredAttackStat;
        CombatStats = new CombatStatBoard(AbilityStats, PreferredAttackStat);
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
        AbilityStats.CalculateModified(_abilityModifiers);
        CombatStats.CalculateBase(AbilityStats, PreferredAttackStat);
        CombatStats.CalculateModified(AbilityStats, PreferredAttackStat, _combatModifiers);
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
