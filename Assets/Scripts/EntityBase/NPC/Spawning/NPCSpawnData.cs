using System.Collections.Generic;
using EntityBase.Faction;
using EntityBase.NPC.AI;
using EntitySystems.Skill;
using EntitySystems.Stats;
using Item;
using Item.Inventory;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace EntityBase.NPC.Spawning
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
        
        [Header("AI Configuration")]
        public NPCAIType aiType = NPCAIType.PatrolNormal;
        public NPCAIConfiguration aiConfiguration;
        
        [Header("Skills")]
        public List<ActiveSkillInfoSO> activeSkills;
        public List<PassiveSkillInfoSO> passiveSkills;
        
        [Header("Equipment")]
        public List<ItemDataSOEquipment> startingEquipment;
        public List<InventoryItem> startingInventory;
        
        [Header("Behavior")]
        public float speed = 2f;
        
        [Header("Spawn Settings")]      
        public int poolSize = 5;
        public float spawnWeight = 1f; // For weighted random spawning
        
        // Factory method to create NPC configuration presets
    }
    
    public enum NPCAIType
    {
        PatrolNormal,
        GuardAgressive,
        WanderPassive,
        KeepPositionPassive,
        Stealth,
        Ranged,
        Merchant,
        Custom
    }
}