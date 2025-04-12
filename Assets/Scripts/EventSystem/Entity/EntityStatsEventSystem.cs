using System;
using EntitySystems.Stats;

namespace EventSystem.Entity
{
    public static class EntityStatsEventSystem
    {
        public static event Action<global::Entity.Entity, AbilityStatBoard> AbilityStatsChanged;
        public static event Action<global::Entity.Entity, CombatStatBoard> CombatStatsChanged;

        public static void InvokeAbilityStatsChanged(global::Entity.Entity entity, AbilityStatBoard abilityStats)
        {
            AbilityStatsChanged?.Invoke(entity, abilityStats);
        }

        public static void InvokeCombatStatsChanged(global::Entity.Entity entity, CombatStatBoard combatStats)
        {
            CombatStatsChanged?.Invoke(entity, combatStats);
        }

        public static void InvokeStatsChanged(global::Entity.Entity entity, AbilityStatBoard abilityStats, CombatStatBoard combatStats)
        {
            AbilityStatsChanged?.Invoke(entity, abilityStats);
            CombatStatsChanged?.Invoke(entity, combatStats);
        }
    }
}
