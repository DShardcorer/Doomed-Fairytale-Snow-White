using System;
using UnityEngine;

public static class EntityStatsEventSystem
{
    public static event EventHandler<AbilityStatBoard> OnAbilityStatsChanged;
    public static event EventHandler<CombatStatBoard> OnCombatStatsChanged;

    public static void InvokeAbilityStatsChanged(object sender, AbilityStatBoard e)
    {
        OnAbilityStatsChanged?.Invoke(sender, e);
    }

    public static void InvokeCombatStatsChanged(object sender, CombatStatBoard e)
    {
        OnCombatStatsChanged?.Invoke(sender, e);
    }

    public static void InvokeStatsChanged(object sender, AbilityStatBoard abilityStats, CombatStatBoard combatStats)
    {
        OnAbilityStatsChanged?.Invoke(sender, abilityStats);
        OnCombatStatsChanged?.Invoke(sender, combatStats);
    }
}
