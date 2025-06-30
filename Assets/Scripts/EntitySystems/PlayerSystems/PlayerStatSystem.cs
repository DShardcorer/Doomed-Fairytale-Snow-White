using System.Collections.Generic;
using DataPersistence;
using DataPersistence.Data;
using EntitySystems.Equipment;
using EntitySystems.Stats;
using EventBus.Player;
using Item.Inventory;

namespace EntitySystems.PlayerSystems
{
    public class PlayerStatSystem : StatSystem, IDataPersistence
    {
        public PlayerStatSystem(AbilityStatBoard baseStats, AttackStatType preferredAttackStat)
            : base(baseStats, preferredAttackStat)
        {
        }

        public override void Initialize(EntityBase.Entity parent)
        {
            base.Initialize(parent);
            ((IDataPersistence)this).AddDataPersistenceObject();
            PlayerEquipmentEventSystem.OnEquipmentChanged += PlayerEquipmentEventSystem_OnEquipmentChanged;
            PlayerLevelEventSystem.OnLevelChanged += PlayerLevelEventSystem_OnLevelChanged;
            PlayerStatsEventSystem.OnStatPointAllocated += PlayerStatsEventSystem_OnStatPointAllocated;
        }

        public override void InvokeInitialEvents()
        {
            PlayerStatsEventSystem.InvokeInitialStatsSet(AbilityStatBoard, CombatStatBoard);
        }

        public override void Dispose()
        {
            PlayerEquipmentEventSystem.OnEquipmentChanged -= PlayerEquipmentEventSystem_OnEquipmentChanged;
            PlayerLevelEventSystem.OnLevelChanged -= PlayerLevelEventSystem_OnLevelChanged;
            PlayerStatsEventSystem.OnStatPointAllocated -= PlayerStatsEventSystem_OnStatPointAllocated;
            base.Dispose();
        }

        protected override void OnStatsChanged()
        {
            PlayerStatsEventSystem.InvokeStatsChanged(AbilityStatBoard, CombatStatBoard);
        }

        // Event handler: when a stat point is allocated via the UI.
        private void PlayerStatsEventSystem_OnStatPointAllocated(object sender, StatType e)
        {
            AllocateAbilityStatPoints(e, 1);
        }

        // Event handler: when the player levels up, add unallocated stat points.
        private void PlayerLevelEventSystem_OnLevelChanged(object sender, OnLevelChangedEventArgs e)
        {
            _unallocatedAbilityStatPoints += 6;
        }

        // Event handler: when equipment changes, update stat modifiers and recalc stats.
        private void PlayerEquipmentEventSystem_OnEquipmentChanged(object sender, IReadOnlyDictionary<EquipmentSlotType, EquipmentInventoryItem> e)
        {
            foreach (var item in e.Values)
            {
                if (item == null)
                {
                    continue;
                }
                foreach (var modifier in item.EquipmentDataSo.StatModifiers)
                {
                    // Check for ability vs. combat modifier.
                    if (modifier.StatType == StatType.Strength ||
                        modifier.StatType == StatType.Dexterity ||
                        modifier.StatType == StatType.Constitution ||
                        modifier.StatType == StatType.Intelligence ||
                        modifier.StatType == StatType.Wisdom ||
                        modifier.StatType == StatType.Charisma)
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

        public void LoadData(GameData saveData)
        {
            SaveLoadHelper.LoadFromSaveData(this, saveData.PlayerStatSystemSaveData);
            InvokeInitialEvents();
        }

        public void SaveData(ref GameData data)
        {
            data.PlayerStatSystemSaveData = SaveLoadHelper.CreateSaveData(this);
        }
    }
}