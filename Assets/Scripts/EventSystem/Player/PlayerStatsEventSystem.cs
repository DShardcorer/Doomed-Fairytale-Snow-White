using System;
using EntitySystems.Stats;

namespace EventSystem.Player
{
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

        public class StatPointGainedEventArgs : EventArgs
        {
            public StatType StatType { get; }
            public int Amount { get; }

            public StatPointGainedEventArgs(StatType statType, int amount)
            {
                StatType = statType;
                Amount = amount;
            }
        }
        public static Action<StatPointGainedEventArgs> OnStatPointGained;
        
        public static void InvokeStatPointGained(StatType statType, int amount)
        {
            OnStatPointGained?.Invoke(new StatPointGainedEventArgs(statType, amount));
        }

    }
}
