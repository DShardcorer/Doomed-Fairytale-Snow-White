using System;
using System.Collections.Generic;
using UnityEngine;

public class StatSystem : ILifecycle<Entity>
{
    protected Entity _parent;
    public Entity Entity => _parent;
    
    public AbilityStatBoard AbilityStatBoard { get; private set; }
    public CombatStatBoard CombatStatBoard { get; private set; }
    public AttackStatType PreferredAttackStat { get; set; }

    protected List<StatModifier> _abilityModifiers = new List<StatModifier>();
    protected List<StatModifier> _combatModifiers = new List<StatModifier>();

    protected int _unallocatedAbilityStatPoints = 0;
    public int UnallocatedAbilityStatPoints => _unallocatedAbilityStatPoints;

    public StatSystem(AbilityStatBoard baseStats, AttackStatType preferredAttackStat)
    {
        AbilityStatBoard = baseStats;
        PreferredAttackStat = preferredAttackStat;
        CombatStatBoard = new CombatStatBoard(AbilityStatBoard, PreferredAttackStat);
        RecalculateStats();
    }

    public virtual void Initialize(Entity parent)
    {
        _parent = parent;
    }

    public virtual void InvokeInitialEvents()
    {
        // Base system does not fire any UI events.
    }

    public virtual void Dispose()
    {
        _parent = null;
        _abilityModifiers.Clear();
        _combatModifiers.Clear();
        AbilityStatBoard = null;
        CombatStatBoard = null;
    }

    public void AllocateAbilityStatPoints(StatType statType, int points)
    {
        if (_unallocatedAbilityStatPoints < points)
        {
            return;
        }
        _unallocatedAbilityStatPoints -= points;
        AbilityStatBoard.IncreaseStat(statType, points);
        RecalculateStats();
        OnStatsChanged();
    }

    public virtual void RecalculateStats()
    {
        AbilityStatBoard.CalculateModified(_abilityModifiers);
        CombatStatBoard.CalculateBase(AbilityStatBoard, PreferredAttackStat);
        CombatStatBoard.CalculateModified(AbilityStatBoard, PreferredAttackStat, _combatModifiers);
        EntityStatsEventSystem.InvokeAbilityStatsChanged(_parent, AbilityStatBoard);
        OnStatsChanged();
    }

    // Virtual hook for notifying that stats have changed.
    protected virtual void OnStatsChanged() { }

    public void AddAbilityModifier(StatModifier modifier)
    {
        _abilityModifiers.Add(modifier);
    }

    public void RemoveAbilityModifier(StatModifier modifier)
    {
        _abilityModifiers.Remove(modifier);
    }

    public void AddCombatModifier(StatModifier modifier)
    {
        _combatModifiers.Add(modifier);
    }

    public void RemoveCombatModifier(StatModifier modifier)
    {
        _combatModifiers.Remove(modifier);
    }
}
