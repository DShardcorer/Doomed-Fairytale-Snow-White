using System.Collections.Generic;
using EventBus.Entity;
using GeneralManagers;

namespace EntitySystems.Stats
{
    public class StatSystem : ILifecycle<EntityBase.Entity>
    {
        protected EntityBase.Entity _parent;
        public EntityBase.Entity Entity => _parent;
    
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

        public virtual void Initialize(EntityBase.Entity parent)
        {
            _parent = parent;
        }

        public virtual void InvokeInitialEvents()
        {
            // Base system does not fire any UI events.
        }

        #region APIS

        public List<StatModifier> GetAbilityModifiers()
        {
            return new List<StatModifier>(_abilityModifiers);
        }

        public List<StatModifier> GetCombatModifiers()
        {
            return new List<StatModifier>(_combatModifiers);
        }

        public void ClearAllModifiers()
        {
            _abilityModifiers.Clear();
            _combatModifiers.Clear();
        }

        public void SetUnallocatedAbilityStatPoints(int points)
        {
            _unallocatedAbilityStatPoints = points;
        }

        #endregion
        
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
        /// <summary>
        /// USE THIS WITH CAUTION. Straight up sets the stat points to the value given.
        /// </summary>
        /// <param name="statType"></param>
        /// <param name="points"></param>
        public void SetAbilityStatPoints(StatType statType, int points)
        {
            AbilityStatBoard.SetStat(statType, points);
            RecalculateStats();
            OnStatsChanged();
        }
        /// <summary>
        /// USE THIS WITH CAUTION. Straight up adds the stat points to the value given.
        /// THIS IS NOT RELATED TO LEVELING UP.
        /// </summary>
        /// <param name="statType"></param>
        /// <param name="points"></param>
        public void AddAbilityStatPoints(StatType statType, int points)
        {
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
}
