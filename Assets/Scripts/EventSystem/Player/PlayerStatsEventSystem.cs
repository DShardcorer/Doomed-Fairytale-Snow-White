using System;
using UnityEngine;

public static class PlayerStatsEventSystem
{
    public static event EventHandler<AbilityStatBoard> OnInitialAbilityStatsSet;
    public static event EventHandler<CombatStatBoard> OnInitialCombatStatsSet;
    public static event EventHandler<AbilityStatBoard> OnAbilityStatsChanged;
    public static event EventHandler<CombatStatBoard> OnCombatStatsChanged;
    public static event EventHandler<StatType> OnStatPointAllocated;

    public static void InvokeAbilityStatsChanged(AbilityStatBoard e)
    {
        OnAbilityStatsChanged?.Invoke(null, e);
    }

    public static void InvokeCombatStatsChanged(CombatStatBoard e)
    {
        OnCombatStatsChanged?.Invoke(null, e);
    }

    public static void InvokeStatsChanged(AbilityStatBoard abilityStats, CombatStatBoard combatStats)
    {
        OnAbilityStatsChanged?.Invoke(null, abilityStats);
        OnCombatStatsChanged?.Invoke(null, combatStats);
    }
    public static void InvokeStatPointAllocated(StatType statType)
    {
        OnStatPointAllocated?.Invoke(null, statType);
    }
    public static void InvokeInitialStatsSet(AbilityStatBoard abilityStats, CombatStatBoard combatStats)
    {
        OnInitialAbilityStatsSet?.Invoke(null, abilityStats);
        OnInitialCombatStatsSet?.Invoke(null, combatStats);
    }

}
