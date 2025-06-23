using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssetManagement;
using DefaultNamespace.EntitySystems.Buff;
using EntityBase.NPC.StandardAI;
using EntityBase.NPC.AI;
using EntitySystems.Equipment;
using EntitySystems.Level;
using EntitySystems.Skill;
using EntitySystems.Skill.SkillRegistry;
using EntitySystems.Stats;
using EntitySystems.VitalStatSystems.Health_System;
using EntitySystems.VitalStatSystems.Mana_System;
using EntitySystems.VitalStatSystems.Stamina_System;
using GeneralManagers;
using Item;
using Item.Inventory;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EntityBase.NPC.Spawning
{
    public class NPCSpawnManager : MonoBehaviour, ILifecycle<GameManager>
    {
        private GameManager _parent;
        protected AddressablesManager addressablesManager;

        public void Initialize(GameManager parent)
        {
            _parent = parent;
            addressablesManager = AddressablesManager.Instance;
        }

        public void Dispose()
        {
            _parent = null;
            addressablesManager = null;
        }

        /// <summary>
        /// Asynchronously spawns an NPC at the specified position
        /// </summary>
        public virtual async Task<NPC> SpawnNPCAsync(NPCSpawnData npcData, Vector3 position,
            Quaternion rotation = default)
        {
            // Use AddressablesManager to instantiate the NPC view
            GameObject viewObject = await addressablesManager.LoadAndInstantiate(npcData.viewAssetReferencePrefab);

            if (viewObject == null)
            {
                Debug.LogError($"Failed to instantiate prefab for {npcData.npcProfile.Name}");
                return null;
            }

            // Position the spawned object
            viewObject.transform.position = position;
            viewObject.transform.rotation = rotation;

            NPCView npcView = viewObject.GetComponent<NPCView>();
            if (npcView == null)
            {
                Debug.LogError(
                    $"Failed to find NPCView component on instantiated prefab for {npcData.npcProfile.Name}");
                Object.Destroy(viewObject);
                return null;
            }

            // Create NPC systems and initialize
            NPC npc = CreateNPCFromData(npcData, npcView);
            if (npc != null)
            {
                npc.Initialize();
                return npc;
            }

            // Clean up if NPC creation failed
            Destroy(viewObject);
            return null;
        }
        //Make a synchronous version
        public virtual NPC SpawnNPC(NPCSpawnData npcData, Vector3 position,
            Quaternion rotation = default)
        {
            GameObject viewObject = Instantiate(npcData.viewPrefab);
            if (viewObject == null)
            {
                Debug.LogError($"Failed to instantiate prefab for {npcData.npcProfile.Name}");
                return null;
            }

            // Position the spawned object
            viewObject.transform.position = position;
            viewObject.transform.rotation = rotation;

            NPCView npcView = viewObject.GetComponent<NPCView>();
            if (npcView == null)
            {
                Debug.LogError(
                    $"Failed to find NPCView component on instantiated prefab for {npcData.npcProfile.Name}");
                Object.Destroy(viewObject);
                return null;
            }

            // Create NPC systems and initialize
            NPC npc = CreateNPCFromData(npcData, npcView);
            if (npc != null)
            {
                npc.Initialize();
                return npc;
            }

            // Clean up if NPC creation failed
            Destroy(viewObject);
            return null;
        }
        
        

        /// <summary>
        /// Non-async wrapper method for backward compatibility
        /// </summary>
        public virtual void SpawnNPC(NPCSpawnData npcData, Vector3 position, Quaternion rotation = default,
            Action<NPC> onSpawned = null)
        {
            // Fire and forget with callback
            _ = SpawnNPCWithCallback(npcData, position, rotation, onSpawned);
        }

        private async Task SpawnNPCWithCallback(NPCSpawnData npcData, Vector3 position, Quaternion rotation,
            Action<NPC> onSpawned)
        {
            NPC npc = await SpawnNPCAsync(npcData, position, rotation);
            onSpawned?.Invoke(npc);
        }

        protected virtual NPC CreateNPCFromData(NPCSpawnData npcData, NPCView npcView)
        {
            // Create skill systems using SkillRegistry
            var activeSkills = CreateActiveSkills(npcData.activeSkills);
            var passiveSkills = CreatePassiveSkills(npcData.passiveSkills);
            ActiveSkillSystem activeSkillSystem = new ActiveSkillSystem(activeSkills);
            PassiveSkillSystem passiveSkillSystem = new PassiveSkillSystem(passiveSkills);

            // Create state machine
            EntityStateMachine stateMachine = new EntityStateMachine();

            // Create stat system
            AbilityStatBoard abilityStatBoard = new AbilityStatBoard(npcData.abilityStatboard);
            StatSystem statSystem = new StatSystem(abilityStatBoard, DetermineAttackStatType(npcData.aiType));

            // Create equipment system and add starting equipment
            EquipmentSystem equipmentSystem = new EquipmentSystem();
            AddStartingEquipment(equipmentSystem, npcData.startingEquipment);

            // Create NPC properties
            NPCProperties npcProperties = new NPCProperties(
                npcData.faction,
                npcData.enemyFactions,
                npcData.speed,
                npcData.aggroRange
            );

            // Create level system
            LevelSystem levelSystem = new LevelSystem(npcData.level);
            // levelSystem.SetCurrentLevel(npcData.level);

            // Create vital stat systems
            HealthSystem healthSystem = new HealthSystem((int)statSystem.CombatStatBoard.Health.ModifiedValue);
            ManaSystem manaSystem = new ManaSystem((int)statSystem.CombatStatBoard.Mana.ModifiedValue);
            StaminaSystem staminaSystem = new StaminaSystem((int)statSystem.CombatStatBoard.Stamina.ModifiedValue);

            // Create inventory and add starting items
            InventorySystem inventory = new InventorySystem();
            AddStartingInventory(inventory, npcData.startingInventory);
            
            BuffSystem buffSystem = new BuffSystem();

            // Create AI controller
            NPCAIController aiController = CreateAIController(npcData);
            

            // Create and return NPC
            return new NPC(
                npcData.npcProfile,
                npcView,
                npcProperties,
                statSystem,
                equipmentSystem,
                activeSkillSystem,
                passiveSkillSystem,
                levelSystem,
                healthSystem,
                manaSystem,
                staminaSystem,
                stateMachine,
                inventory,
                buffSystem,
                aiController
            );
        }

        protected virtual List<ActiveSkill> CreateActiveSkills(List<ActiveSkillInfoSO> skillSOs)
        {
            var skills = new List<ActiveSkill>();
            foreach (var skillSO in skillSOs)
            {
                if (skillSO != null)
                {
                    // Use SkillRegistry to create the skill from its name
                    ActiveSkill skill = SkillRegistry.CreateActiveSkill(skillSO.SkillName);
                    if (skill != null)
                    {
                        skills.Add(skill);
                    }
                }
            }
            return skills;
        }

        protected virtual List<PassiveSkill> CreatePassiveSkills(List<PassiveSkillInfoSO> skillSOs)
        {
            var skills = new List<PassiveSkill>();
            foreach (var skillSO in skillSOs)
            {
                if (skillSO != null)
                {
                    // Use SkillRegistry to create the skill from its name
                    PassiveSkill skill = SkillRegistry.CreatePassiveSkill(skillSO.SkillName);
                    if (skill != null)
                    {
                        skills.Add(skill);
                    }
                }
            }
            return skills;
        }

        protected virtual AttackStatType DetermineAttackStatType(NPCAIType aiType)
        {
            switch (aiType)
            {
                case NPCAIType.PatrolNormal:
                case NPCAIType.GuardAgressive:
                    return AttackStatType.Strength;
                case NPCAIType.Ranged:
                case NPCAIType.Stealth:
                    return AttackStatType.Dexterity;
                default:
                    return AttackStatType.Strength;
            }
        }

        protected virtual void AddStartingEquipment(EquipmentSystem equipmentSystem, List<ItemDataSOEquipment> equipment)
        {
            foreach (var itemData in equipment)
            {
                if (itemData != null)
                {
                    EquipmentInventoryItem item = new EquipmentInventoryItem(itemData, 1);
                    equipmentSystem.EquipItem(item);
                }
            }
        }

        protected virtual void AddStartingInventory(InventorySystem inventory, List<InventoryItem> items)
        {
            foreach (var item in items)
            {
                if (item != null)
                {
                    inventory.AddItem(item);
                }
            }
        }

        protected virtual NPCAIController CreateAIController(NPCSpawnData npcData)
        {
            return NPCAIFactory.Create(npcData);
        }


    }
}