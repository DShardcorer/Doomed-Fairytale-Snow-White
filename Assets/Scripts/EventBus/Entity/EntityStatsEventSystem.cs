using System;
using EntitySystems.Stats;

namespace EventBus.Entity
{
    public static class EntityStatsEventSystem
    {
        public static event Action<global::EntityBase.Entity, AbilityStatBoard> AbilityStatsChanged;
        public static event Action<global::EntityBase.Entity, CombatStatBoard> CombatStatsChanged;

        public static void InvokeAbilityStatsChanged(global::EntityBase.Entity entity, AbilityStatBoard abilityStats)
        {
            AbilityStatsChanged?.Invoke(entity, abilityStats);
        }

        public static void InvokeCombatStatsChanged(global::EntityBase.Entity entity, CombatStatBoard combatStats)
        {
            CombatStatsChanged?.Invoke(entity, combatStats);
        }

        public static void InvokeStatsChanged(global::EntityBase.Entity entity, AbilityStatBoard abilityStats, CombatStatBoard combatStats)
        {
            AbilityStatsChanged?.Invoke(entity, abilityStats);
            CombatStatsChanged?.Invoke(entity, combatStats);
        }
    }
}
