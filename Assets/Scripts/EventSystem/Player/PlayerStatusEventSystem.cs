using System;
using UnityEngine;

public static class PlayerStatusEventSystem
{
    public static event EventHandler<AbilityStatBoard> OnAbilityStatsChanged;
    public static event EventHandler<CombatStatBoard> OnCombatStatsChanged;

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

}
