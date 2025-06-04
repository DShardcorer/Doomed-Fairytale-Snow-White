using System.Collections.Generic;
using Entity.Faction;
using Entity.NPC.AI;
using EntitySystems.Skill;
using EntitySystems.Stats;
using Item;
using Item.Inventory;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Entity.NPC.Spawning
{
    [CreateAssetMenu(fileName = "NPCSpawnData", menuName = "NPC/NPC Spawn Data")]
    public class NPCSpawnData : ScriptableObject
    {
        [Header("Identity")]
        public EntityProfile npcProfile;
        
        [Header("Visual")]
        public AssetReferenceGameObject viewAssetReferencePrefab;
        public GameObject viewPrefab;

        
        [Header("Stats")]
        public AbilityStatboardSO abilityStatboard;
        public int level = 1;
        
        [Header("Faction")]
        public EntityFaction faction = EntityFaction.Native;
        public List<EntityFaction> enemyFactions = new List<EntityFaction>();
        
        [Header("AI Configuration")]
        public NPCAIType aiType = NPCAIType.Melee;
        public NPCAIConfiguration aiConfiguration;
        
        [Header("Skills")]
        public List<ActiveSkillInfoSO> activeSkills = new List<ActiveSkillInfoSO>();
        public List<PassiveSkillInfoSO> passiveSkills = new List<PassiveSkillInfoSO>();
        
        [Header("Equipment")]
        public List<ItemDataSOEquipment> startingEquipment = new List<ItemDataSOEquipment>();
        public List<InventoryItem> startingInventory = new List<InventoryItem>();
        
        [Header("Behavior")]
        public float speed = 2f;
        public float detectionRange = 5f;
        public float aggroRange = 3f;
        public bool isHostile = true;
        public bool canPatrol = true;
        
        [Header("Spawn Settings")]
        public int poolSize = 5;
        public float spawnWeight = 1f; // For weighted random spawning
        
        // Factory method to create NPC configuration presets
        public static NPCSpawnData CreateMeleeWarrior()
        {
            var data = CreateInstance<NPCSpawnData>();
            data.npcProfile = new EntityProfile(
                "Melee Warrior",
                "A fierce warrior skilled in close combat."
            );
            data.aiType = NPCAIType.Melee;
            data.faction = EntityFaction.Native;
            data.enemyFactions = new List<EntityFaction> { EntityFaction.Civilized, EntityFaction.Player };
            data.isHostile = true;
            data.canPatrol = true;
            return data;
        }
        
        public static NPCSpawnData CreateRangedArcher()
        {
            var data = CreateInstance<NPCSpawnData>();
            data.npcProfile = new EntityProfile(
                "Ranged Archer",
                "A skilled archer who attacks from a distance."
            );
            data.aiType = NPCAIType.Ranged;
            data.faction = EntityFaction.Native;
            data.enemyFactions = new List<EntityFaction> { EntityFaction.Civilized, EntityFaction.Player };
            data.isHostile = true;
            data.canPatrol = true;
            return data;
        }
        
        public static NPCSpawnData CreatePeacefulVillager()
        {
            var data = CreateInstance<NPCSpawnData>();
            data.npcProfile = new EntityProfile(
                "Peaceful Villager",
                "A villager who prefers to avoid conflict and live in harmony."
            );
            data.aiType = NPCAIType.Peaceful;
            data.faction = EntityFaction.Civilized;
            data.enemyFactions = new List<EntityFaction> { EntityFaction.Native };
            data.isHostile = false;
            data.canPatrol = true;
            return data;
        }
    }
    
    public enum NPCAIType
    {
        Melee,
        Ranged,
        Guard,
        Peaceful,
        Stealth,
        Merchant,
        Custom
    }
}