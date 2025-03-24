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

    private int _unallocatedAbilityStatPoints = 0;
    public int UnallocatedAbilityStatPoints => _unallocatedAbilityStatPoints;

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
            PlayerEquipmentEventSystem.PlayerEquipmentSystem_OnEquipmentChanged += PlayerEquipmentEventSystem_OnEquipmentChanged;
            PlayerLevelEventSystem.OnLevelChanged += PlayerLevelEventSystem_OnLevelChanged;
            PlayerStatsEventSystem.OnStatPointAllocated += PlayerStatsEventSystem_OnStatPointAllocated;
        }
    }
    public void InvokeInitialEvents()
    {
        if (_parent is Player)
        {
            PlayerStatsEventSystem.InvokeInitialStatsSet(AbilityStatBoard, CombatStatBoard);

        }
    }

    private void PlayerStatsEventSystem_OnStatPointAllocated(object sender, StatType e)
    {
        AllocateAbilityStatPoints(e, 1);
    }

    private void PlayerLevelEventSystem_OnLevelChanged(object sender, OnLevelChangedEventArgs e)
    {
        _unallocatedAbilityStatPoints += 6;
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
        if (_parent is Player)
        {
            PlayerStatsEventSystem.InvokeStatsChanged(AbilityStatBoard, CombatStatBoard);
        }
    }



    private void PlayerEquipmentEventSystem_OnEquipmentChanged(object sender, IReadOnlyDictionary<EquipmentSlotType, EquipmentInventoryItem> e)
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
            PlayerEquipmentEventSystem.PlayerEquipmentSystem_OnEquipmentChanged -= PlayerEquipmentEventSystem_OnEquipmentChanged;
            PlayerLevelEventSystem.OnLevelChanged -= PlayerLevelEventSystem_OnLevelChanged;
        }
    }
    public void RecalculateStats()
    {
        AbilityStatBoard.CalculateModified(_abilityModifiers);
        CombatStatBoard.CalculateBase(AbilityStatBoard, PreferredAttackStat);
        CombatStatBoard.CalculateModified(AbilityStatBoard, PreferredAttackStat, _combatModifiers);
        EntityStatsEventSystem.InvokeAbilityStatsChanged(_parent, AbilityStatBoard);
        if (_parent is Player)
        {
            PlayerStatsEventSystem.InvokeStatsChanged(AbilityStatBoard, CombatStatBoard);
        }
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
