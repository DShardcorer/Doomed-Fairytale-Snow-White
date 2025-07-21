using System.Collections.Generic;
using UnityEngine;

namespace RelationshipSystem
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "Romance/Character Data")]
    public class RomanceableCharacterData : ScriptableObject
    {
        public string characterID;
        public string displayName;
    
        [Header("Preferences")]
        public List<Item.Item> favoriteGifts;
        public List<Item.Item> dislikedGifts;
        public List<string> favoriteTopics;
    
        [Header("Relationship Events")]
        public List<RelationshipEvent> relationshipEvents;
    
        [System.Serializable]
        public class RelationshipEvent
        {
            public string eventID;
            public RelationshipManager.RelationshipStage requiredStage;
            public float affectionImpact;
            // public DialogueScriptableObject dialogue;
        }
    }
}