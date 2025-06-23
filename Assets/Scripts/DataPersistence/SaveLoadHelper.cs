using System.Collections.Generic;
using DataPersistence.Data;
using DateDayNightSystem;
using DefaultNamespace.EntitySystems.VitalStatSystems;
using EntityBase.Player;
using EntitySystems.PlayerSystems;
using EntitySystems.Skill;
using EntitySystems.Skill.SkillRegistry;
using EntitySystems.Stats;
using Item;
using Item.Inventory;
using UnityEngine;

namespace DataPersistence
{
    public static partial class SaveLoadHelper
    {
        #region Inventory

        public static InventorySaveData CreateSaveData(PlayerInventorySystem inventory)
        {
            InventorySaveData saveData = new InventorySaveData
            {
                capacity = inventory.Capacity,
                currentWeight = inventory.CurrentWeight,
                items = ConvertItems(inventory.ItemList),
                materialItems = ConvertItems(inventory.materialItems),
                consumableItems = ConvertItems(inventory.consumableItems),
                equipmentItems = ConvertItems(inventory.equipmentItems),
                miscellaneousItems = ConvertItems(inventory.miscellaneousItems)
            };

            return saveData;
        }

        public static void LoadFromSaveData(PlayerInventorySystem inventory, InventorySaveData saveData)
        {
            inventory.ClearAll();

            inventory.capacity = saveData.capacity;
            inventory.currentWeight = saveData.currentWeight;

            inventory.ItemList = CreateItems(saveData.items, inventory.itemDictionary);
            inventory.materialItems = CreateItems(saveData.materialItems, inventory.materialItemDictionary);
            inventory.consumableItems = CreateItems(saveData.consumableItems, inventory.consumableItemDictionary);
            inventory.equipmentItems = CreateItems(saveData.equipmentItems, inventory.equipmentItemDictionary);
            inventory.miscellaneousItems =
                CreateItems(saveData.miscellaneousItems, inventory.miscellaneousItemDictionary);
        }

        private static List<InventoryItemSaveData> ConvertItems(List<InventoryItem> items)
        {
            var result = new List<InventoryItemSaveData>();
            foreach (var item in items)
            {
                result.Add(new InventoryItemSaveData
                {
                    itemName = item.itemDataSo.itemName,
                    stackSize = item.stackSize
                });
            }

            return result;
        }

        private static List<InventoryItem> CreateItems(List<InventoryItemSaveData> dataList,
            Dictionary<ItemDataSO, InventoryItem> dictionary)
        {
            var result = new List<InventoryItem>();
            foreach (var data in dataList)
            {
                var itemData = ItemRegistry.GetItemDataByName(data.itemName);
                var item = ItemRegistry.CreateInventoryItem(data.itemName, data.stackSize);
                item.stackSize = data.stackSize;
                result.Add(item);
                dictionary[itemData] = item;
            }

            return result;
        }

        #endregion

        #region Player Health

        public static HealthSystemSaveData CreateSaveData(PlayerHealthSystem healthSystem)
        {
            HealthSystemSaveData saveData = new HealthSystemSaveData
            {
                maxHealth = healthSystem.MaxHealth,
                currentHealth = healthSystem.CurrentHealth,
                activeRecoveryEffects = ConvertRecoveryEffects(healthSystem.GetActiveRecoveryEffects())
            };

            return saveData;
        }

        public static void LoadFromSaveData(PlayerHealthSystem healthSystem, HealthSystemSaveData saveData)
        {
            // Set basic properties
            healthSystem.SetMaxHealth(saveData.maxHealth);
            healthSystem.SetCurrentHealth(saveData.currentHealth);
    
            // Load recovery effects
            healthSystem.ClearRecoveryEffects();
            if (saveData.activeRecoveryEffects != null)
            {
                foreach (var effectData in saveData.activeRecoveryEffects)
                {
                    healthSystem.AddRecoveryEffect(effectData);
                }
            }
        }

        private static List<RecoveryEffectSaveData> ConvertRecoveryEffects(List<RecoveryOverTimeEffect> effects)
        {
            if (effects == null) return null;
    
            var result = new List<RecoveryEffectSaveData>();
            foreach (var effect in effects)
            {
                result.Add(new RecoveryEffectSaveData
                {
                    totalAmount = effect.TotalAmount,
                    remainingAmount = effect.RemainingAmount,
                    duration = effect.Duration,
                    remainingTime = effect.RemainingTime
                });
            }

            return result;
        }

        #endregion

        #region Player Mana

        public static ManaSystemSaveData CreateSaveData(PlayerManaSystem manaSystem)
        {
            ManaSystemSaveData saveData = new ManaSystemSaveData
            {
                maxMana = manaSystem.MaxMana,
                currentMana = manaSystem.CurrentMana,
                activeRecoveryEffects = ConvertRecoveryEffects(manaSystem.GetActiveRecoveryEffects())
            };

            return saveData;
        }
        public static void LoadFromSaveData(PlayerManaSystem manaSystem, ManaSystemSaveData saveData)
        {
            // Set basic properties
            manaSystem.SetMaxMana(saveData.maxMana);
            manaSystem.SetCurrentMana(saveData.currentMana);
    
            // Load recovery effects
            manaSystem.ClearRecoveryEffects();
            if (saveData.activeRecoveryEffects != null)
            {
                foreach (var effectData in saveData.activeRecoveryEffects)
                {
                    manaSystem.AddRecoveryEffect(effectData);
                }
            }
        }
        
        #endregion

        #region Player Stamina System
        public static StaminaSystemSaveData CreateSaveData(PlayerStaminaSystem staminaSystem)
        {
            StaminaSystemSaveData saveData = new StaminaSystemSaveData
            {
                maxStamina = staminaSystem.MaxStamina,
                currentStamina = staminaSystem.CurrentStamina,
                activeRecoveryEffects = ConvertRecoveryEffects(staminaSystem.GetActiveRecoveryEffects())
            };

            return saveData;
        }
        public static void LoadFromSaveData(PlayerStaminaSystem staminaSystem, StaminaSystemSaveData saveData)
        {
            // Set basic properties
            staminaSystem.SetMaxStamina(saveData.maxStamina);
            staminaSystem.SetCurrentStamina(saveData.currentStamina);
    
            // Load recovery effects
            staminaSystem.ClearRecoveryEffects();
            if (saveData.activeRecoveryEffects != null)
            {
                foreach (var effectData in saveData.activeRecoveryEffects)
                {
                    staminaSystem.AddRecoveryEffect(effectData);
                }
            }
        }
        
        

        #endregion
        #region Player Stats

        public static StatSystemSaveData CreateSaveData(PlayerStatSystem statSystem)
        {
            StatSystemSaveData saveData = new StatSystemSaveData
            {
                abilityStats = new AbilityStatBoardSaveData
                {
                    strengthBase = (int)statSystem.AbilityStatBoard.Strength.BaseValue,
                    dexterityBase = (int)statSystem.AbilityStatBoard.Dexterity.BaseValue,
                    constitutionBase = (int)statSystem.AbilityStatBoard.Constitution.BaseValue,
                    intelligenceBase = (int)statSystem.AbilityStatBoard.Intelligence.BaseValue,
                    wisdomBase = (int)statSystem.AbilityStatBoard.Wisdom.BaseValue,
                    charismaBase = (int)statSystem.AbilityStatBoard.Charisma.BaseValue
                },
                unallocatedAbilityStatPoints = statSystem.UnallocatedAbilityStatPoints,
                preferredAttackStat = statSystem.PreferredAttackStat
            };

            return saveData;
        }

        public static void LoadFromSaveData(PlayerStatSystem statSystem, StatSystemSaveData saveData)
        {
            // Load base ability stats
            statSystem.AbilityStatBoard.SetStat(StatType.Strength, saveData.abilityStats.strengthBase);
            statSystem.AbilityStatBoard.SetStat(StatType.Dexterity, saveData.abilityStats.dexterityBase);
            statSystem.AbilityStatBoard.SetStat(StatType.Constitution, saveData.abilityStats.constitutionBase);
            statSystem.AbilityStatBoard.SetStat(StatType.Intelligence, saveData.abilityStats.intelligenceBase);
            statSystem.AbilityStatBoard.SetStat(StatType.Wisdom, saveData.abilityStats.wisdomBase);
            statSystem.AbilityStatBoard.SetStat(StatType.Charisma, saveData.abilityStats.charismaBase);

            // Set preferred attack stat
            statSystem.PreferredAttackStat = saveData.preferredAttackStat;

            // Set unallocated points
            statSystem.SetUnallocatedAbilityStatPoints(saveData.unallocatedAbilityStatPoints);

            // Recalculate stats to update derived values
            statSystem.RecalculateStats();
        }

        #endregion
        #region Player Level

        public static LevelSystemSaveData CreateSaveData(PlayerLevelSystem levelSystem)
        {
            LevelSystemSaveData saveData = new LevelSystemSaveData
            {
                level = levelSystem.Level,
                experience = levelSystem.Experience,
                experienceToNextLevel = levelSystem.ExperienceToNextLevel
            };

            return saveData;
        }

        public static void LoadFromSaveData(PlayerLevelSystem levelSystem, LevelSystemSaveData saveData)
        {
            levelSystem.SetLevel(saveData.level);
            levelSystem.SetExperience(saveData.experience);
            levelSystem.SetExperienceToNextLevel(saveData.experienceToNextLevel);
        }

        #endregion
        #region Player Equipment

        public static EquipmentSystemSaveData CreateSaveData(PlayerEquipmentSystem equipmentSystem)
        {
            var equippedSlots = new List<EquipmentSlotSaveData>();
    
            foreach (var kvp in equipmentSystem.EquippedItems)
            {
                if (kvp.Value != null)
                {
                    equippedSlots.Add(new EquipmentSlotSaveData
                    {
                        slotType = kvp.Key,
                        itemName = kvp.Value.itemDataSo.itemName
                    });
                }
            }
    
            return new EquipmentSystemSaveData
            {
                equippedItems = equippedSlots
            };
        }

        public static void LoadFromSaveData(PlayerEquipmentSystem equipmentSystem, EquipmentSystemSaveData saveData, PlayerInventorySystem inventorySystem)
        {
            // Clear current equipment
            foreach (var item in equipmentSystem.EquippedItems.Values)
            {
                if (item != null)
                {
                    equipmentSystem.UnequipItem(item);
                }
            }
    
            // Re-equip items from save data
            if (saveData?.equippedItems != null)
            {
                foreach (var slotData in saveData.equippedItems)
                {
                    // Find the item in player's inventory
                    foreach (var item in inventorySystem.equipmentItems)
                    {
                        if (item is EquipmentInventoryItem equipItem && 
                            item.itemDataSo.itemName == slotData.itemName &&
                            equipItem.SoEquipmentDataSo.equipmentSlotType == slotData.slotType)
                        {
                            equipmentSystem.EquipItem(equipItem);
                            break;
                        }
                    }
                }
            }
        }

        #endregion
        #region Player Active Skills

        public static ActiveSkillSystemSaveData CreateSaveData(PlayerActiveSkillSystem skillSystem)
        {
            var activeSkills = new List<ActiveSkillSaveData>();
    
            foreach (var skill in skillSystem.ActiveSkills)
            {
                activeSkills.Add(new ActiveSkillSaveData
                {
                    skillName = skill.activeSkillInfo.SkillName
                });
            }
    
            return new ActiveSkillSystemSaveData
            {
                activeSkills = activeSkills
            };
        }

        public static void LoadFromSaveData(PlayerActiveSkillSystem skillSystem, ActiveSkillSystemSaveData saveData)
        {
            // Track current skill names to avoid duplicates
            var currentSkillNames = new HashSet<string>();
    
            foreach (var skill in skillSystem.ActiveSkills)
            {
                currentSkillNames.Add(skill.activeSkillInfo.SkillName);
            }
    
            // Add skills from save data that aren't already in the system
            if (saveData?.activeSkills != null)
            {
                foreach (var skillData in saveData.activeSkills)
                {
                    if (!currentSkillNames.Contains(skillData.skillName))
                    {
                        var newSkill = SkillRegistry.CreateActiveSkill(skillData.skillName);
                        if (newSkill != null)
                        {
                            skillSystem.AddSkill(newSkill);
                        }
                    }
                }
            }
        }

        #endregion
        #region Player Passive Skills

        public static PassiveSkillSystemSaveData CreateSaveData(PlayerPassiveSkillSystem skillSystem)
        {
            var passiveSkills = new List<PassiveSkillSaveData>();

            foreach (var skill in skillSystem.PassiveSkills)
            {
                passiveSkills.Add(new PassiveSkillSaveData
                {
                    skillName = skill.SkillInfo.SkillName
                });
            }

            return new PassiveSkillSystemSaveData
            {
                passiveSkills = passiveSkills
            };
        }

        public static void LoadFromSaveData(PlayerPassiveSkillSystem skillSystem, PassiveSkillSystemSaveData saveData)
        {
            // Track current skill names to avoid duplicates
            var currentSkillNames = new HashSet<string>();

            foreach (var skill in skillSystem.PassiveSkills)
            {
                currentSkillNames.Add(skill.SkillInfo.SkillName);
            }

            // Add skills from save data that aren't already in the system
            if (saveData?.passiveSkills != null)
            {
                foreach (var skillData in saveData.passiveSkills)
                {
                    if (!currentSkillNames.Contains(skillData.skillName))
                    {
                        var newSkill = SkillRegistry.CreatePassiveSkill(skillData.skillName);
                        if (newSkill != null)
                        {
                            skillSystem.AddSkill(newSkill);
                        }
                    }
                }
            }
        }

        #endregion
        #region Player Equipped Skills

        public static EquippedSkillSystemSaveData CreateSaveData(PlayerEquippedSkillSystem skillSystem)
        {
            var equippedSkills = new List<EquippedSkillSaveData>();
    
            foreach (var pair in skillSystem.GetAllEquippedSkills())
            {
                equippedSkills.Add(new EquippedSkillSaveData
                {
                    slotIndex = pair.Key,
                    skillName = pair.Value.activeSkillInfo.SkillName
                });
            }
    
            return new EquippedSkillSystemSaveData
            {
                equippedSkills = equippedSkills
            };
        }

        public static void LoadFromSaveData(PlayerEquippedSkillSystem skillSystem, EquippedSkillSystemSaveData saveData, PlayerActiveSkillSystem activeSkillSystem)
        {
            // Clear current equipped skills
            foreach (int slot in new List<int>(skillSystem.GetAllEquippedSkills().Keys))
            {
                skillSystem.UnequipSkill(slot);
            }
    
            // Re-equip skills from save data
            if (saveData?.equippedSkills != null)
            {
                foreach (var skillData in saveData.equippedSkills)
                {
                    // Find the skill in player's active skills
                    foreach (var skill in activeSkillSystem.ActiveSkills)
                    {
                        if (skill.activeSkillInfo.SkillName == skillData.skillName)
                        {
                            skillSystem.EquipSkill(skillData.slotIndex, skill);
                            break;
                        }
                    }
                }
            }
        }

        #endregion
        #region Player Position

        public static PlayerPositionSaveData CreateSaveData(PlayerView playerView, SceneSwitch.SceneSwitchManager sceneSwitchManager)
        {
            var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            var currentPosition = playerView.transform.position;
    
            // Get the overworld position from SceneSwitchManager
            var overworldPosition = sceneSwitchManager.OverworldSpawnPosition;
    
            return new PlayerPositionSaveData
            {
                // Current scene data
                currentSceneName = currentScene,
                positionX = currentPosition.x,
                positionY = currentPosition.y,
                positionZ = currentPosition.z,
        
                // Overworld position
                overworldPositionX = overworldPosition.x,
                overworldPositionY = overworldPosition.y,
                overworldPositionZ = overworldPosition.z
            };
        }

        public static void LoadFromSaveData(PlayerView playerView, PlayerPositionSaveData saveData, SceneSwitch.SceneSwitchManager sceneSwitchManager)
        {
            // Set the overworld spawn position first
            Vector3 overworldPosition = new Vector3(
                saveData.overworldPositionX,
                saveData.overworldPositionY,
                saveData.overworldPositionZ
            );
            sceneSwitchManager.SetLastOverworldSpawnPosition(overworldPosition);
    
            // Handle current scene/position
            string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    
            if (currentScene != saveData.currentSceneName)
            {
                // We need to switch scenes
                Vector3 savedPosition = new Vector3(saveData.positionX, saveData.positionY, saveData.positionZ);
                sceneSwitchManager.ExitSpecialScene(saveData.currentSceneName, savedPosition);
            }
            else
            {
                // Already in the correct scene, just update position
                playerView.transform.position = new Vector3(saveData.positionX, saveData.positionY, saveData.positionZ);
            }
        }

        #endregion
        #region Game Time

        public static GameTimeSaveData CreateSaveData(GameTimeManager timeManager)
        {
            return new GameTimeSaveData
            {
                currentDay = timeManager.CurrentDay,
                currentTimeOfDay = timeManager.CurrentTime.hourOfDay,
                isPaused = timeManager.IsPaused,
                isReversing = timeManager.IsReversing
            };
        }

        public static void LoadFromSaveData(GameTimeManager timeManager, GameTimeSaveData saveData)
        {
            timeManager.SetDateTime(saveData.currentDay, saveData.currentTimeOfDay);
    
            // Set pause state
            if (saveData.isPaused)
                timeManager.PauseTime();
            else
                timeManager.ResumeTime();
        
            // Set time reversal state
            timeManager.IsReversing = saveData.isReversing;
        }

        #endregion
    }
    
}