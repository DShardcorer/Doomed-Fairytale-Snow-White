using System;
using UnityEngine;

public static class EntityStatsEventSystem
{
    public static event Action<Entity, AbilityStatBoard> AbilityStatsChanged;
    public static event Action<Entity, CombatStatBoard> CombatStatsChanged;

    public static void InvokeAbilityStatsChanged(Entity entity, AbilityStatBoard abilityStats)
    {
        AbilityStatsChanged?.Invoke(entity, abilityStats);
    }

    public static void InvokeCombatStatsChanged(Entity entity, CombatStatBoard combatStats)
    {
        CombatStatsChanged?.Invoke(entity, combatStats);
    }

    public static void InvokeStatsChanged(Entity entity, AbilityStatBoard abilityStats, CombatStatBoard combatStats)
    {
        AbilityStatsChanged?.Invoke(entity, abilityStats);
        CombatStatsChanged?.Invoke(entity, combatStats);
    }
}
