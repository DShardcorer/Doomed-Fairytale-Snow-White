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
        if (_parent is Player)
        {
            PlayerEquipmentEventSystem.PlayerEquipmentSystem_OnEquipmentChanged += OnEquipmentChanged;
        }
    }
    public void InvokeInitialEvents()
    {
        if (_parent is Player)
        {
            PlayerStatusEventSystem.InvokeStatsChanged(AbilityStatBoard, CombatStatBoard);
        }
    }

    private void OnEquipmentChanged(object sender, IReadOnlyDictionary<EquipmentSlotType, EquipmentInventoryItem> e)
    {
        foreach (var item in e.Values)
        {
            if (item == null)
            {
                continue;
            }
            foreach (var modifier in item.EquipmentData.StatModifiers)
            {
                if (modifier.StatType == StatType.Strength
                || modifier.StatType == StatType.Dexterity
                || modifier.StatType == StatType.Constitution
                || modifier.StatType == StatType.Intelligence
                || modifier.StatType == StatType.Wisdom
                || modifier.StatType == StatType.Charisma)
                {
                    AddAbilityModifier(modifier);
                }
                else
                {
                    AddCombatModifier(modifier);
                }
            }
        }
        RecalculateStats();
        if (_parent is Player)
        {
            PlayerStatusEventSystem.InvokeStatsChanged(AbilityStatBoard, CombatStatBoard);
        }

    }

    public void Dispose()
    {
        _parent = null;
        _abilityModifiers.Clear();
        _combatModifiers.Clear();
        AbilityStatBoard = null;
        CombatStatBoard = null;
        if (_parent is Player)
        {
            PlayerEquipmentEventSystem.PlayerEquipmentSystem_OnEquipmentChanged -= OnEquipmentChanged;
        }
    }
    public void RecalculateStats()
    {
        AbilityStatBoard.CalculateModified(_abilityModifiers);
        CombatStatBoard.CalculateBase(AbilityStatBoard, PreferredAttackStat);
        CombatStatBoard.CalculateModified(AbilityStatBoard, PreferredAttackStat, _combatModifiers);
        OnStatsChanged?.Invoke(this, EventArgs.Empty);
    }

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
