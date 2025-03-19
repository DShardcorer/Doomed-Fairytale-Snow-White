using System;
using System.Collections.Generic;
using UnityEngine;

public class StatSystem : ILifecycle<Entity>
{
    private Entity _parent;
    public Entity Entity => _parent;
    public AbilityStatBoard AbilityStatBoard { get; private set; }
    public CombatStatBoard CombatStatBoard { get; private set; }
    public AttackStatType PreferredAttackStat { get; set; }

    private List<StatModifier> _abilityModifiers = new List<StatModifier>();
    private List<StatModifier> _combatModifiers = new List<StatModifier>();
    public event EventHandler OnStatsChanged;

    public StatSystem(AbilityStatBoard baseStats, AttackStatType preferredAttackStat)
    {
        AbilityStatBoard = baseStats;
        PreferredAttackStat = preferredAttackStat;
        CombatStatBoard = new CombatStatBoard(AbilityStatBoard, PreferredAttackStat);
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
        AbilityStatBoard.CalculateModified(_abilityModifiers);
        CombatStatBoard.CalculateBase(AbilityStatBoard, PreferredAttackStat);
        CombatStatBoard.CalculateModified(AbilityStatBoard, PreferredAttackStat, _combatModifiers);
        // Optionally, trigger events for UI updates.
        OnStatsChanged?.Invoke(this, EventArgs.Empty);
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
