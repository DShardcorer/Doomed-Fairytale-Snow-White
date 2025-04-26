using System.Collections.Generic;
using Entity.Faction;
using EntitySystems.Skill;
using EntitySystems.Skill.ActiveSkills;
using EntitySystems.Skill.PassiveSkills;
using EntitySystems.Stats;
using UnityEngine;

namespace Entity.Player._PlayerCreation
{
    [System.Serializable]
    public class PlayerStartingInfo
    {
        // Basic Stats
        public AbilityStatboardSO abilityStatboardSo;
        
        // Equipment Settings
        public List<string> startingEquipmentIds = new List<string>();
        public string startingWeaponId;
        public string startingArmorId;
        
        // Skills
        public List<string> startingActiveSkillIds = new List<string>();
        public List<string> startingPassiveSkillIds = new List<string>();
        
        
        // Inventory
        public struct startingInventoryItem
        {
            public string itemId;
            public int stackSize;
        }
        public List<startingInventoryItem> startingInventoryItems = new List<startingInventoryItem>();
        
       
    }
}