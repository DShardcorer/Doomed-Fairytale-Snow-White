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
        public NPCAIType aiType = NPCAIType.PatrolNormal;
        public NPCAIConfiguration aiConfiguration;
        
        [Header("Skills")]
        public List<ActiveSkillInfoSO> activeSkills = new List<ActiveSkillInfoSO>();
        public List<PassiveSkillInfoSO> passiveSkills = new List<PassiveSkillInfoSO>();
        
        [Header("Equipment")]
        public List<ItemDataSOEquipment> startingEquipment = new List<ItemDataSOEquipment>();
        public List<InventoryItem> startingInventory = new List<InventoryItem>();
        
        [Header("Behavior")]
        public float speed = 2f;
        public float aggroRange = 3f;
        
        [Header("Spawn Settings")]
        public int poolSize = 5;
        public float spawnWeight = 1f; // For weighted random spawning
        
        // Factory method to create NPC configuration presets
    }
    
    public enum NPCAIType
    {
        PatrolNormal,
        Ranged,
        GuardAgressive,
        WanderPassive,
        Stealth,
        Merchant,
        Custom
    }
}