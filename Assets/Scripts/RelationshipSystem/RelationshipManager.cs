using System.Collections.Generic;
using UnityEngine;

namespace RelationshipSystem
{
    public class RelationshipManager : MonoBehaviour
    {
        [System.Serializable]
        public class Relationship
        {
            public string characterID;
            public float affection;
            public RelationshipStage currentStage;
            public List<string> completedEvents = new List<string>();
        }

        public enum RelationshipStage
        {
            Stranger,
            Acquaintance, 
            Friend,
            Close,
            Romantic
        }
    
        private Dictionary<string, Relationship> relationships = new Dictionary<string, Relationship>();
    
        public void ModifyAffection(string characterID, float amount)
        {
            if (!relationships.ContainsKey(characterID))
                // InitializeRelationship(characterID);
            
            relationships[characterID].affection += amount;
            CheckForStageProgression(characterID);
        }
    
        private void CheckForStageProgression(string characterID) {
            // Logic to advance relationship stages based on affection and completed events
        }
    }
}