using System.Collections.Generic;
using GeneralManagers;
using UnityEngine;

namespace Entity.Faction
{
    public class FactionManager : MonoBehaviour, ILifecycle<GameManager>
    {
        private GameManager _parent;
        
        // Dictionary to track temporary enemy relationships with their expiration times
        private Dictionary<string, float> _temporaryEnemies = new Dictionary<string, float>();
        
        public void Initialize(GameManager parent)
        {
            _parent = parent;
        }

        public void Dispose()
        {
            _parent = null;
            _temporaryEnemies.Clear();
        }

        public void Update()
        {
            UpdateTemporaryEnemies();
        }

        private void UpdateTemporaryEnemies()
        {
            if (_temporaryEnemies.Count == 0)
                return;

            float currentTime = Time.time;
            List<string> expiredRelations = new List<string>();

            // Find expired enemy relationships
            foreach (var entry in _temporaryEnemies)
            {
                if (entry.Value <= currentTime)
                {
                    expiredRelations.Add(entry.Key);
                }
            }

            // Remove expired relationships
            foreach (var relationKey in expiredRelations)
            {
                string[] factions = relationKey.Split('|');
                EntityFaction faction1 = (EntityFaction)System.Enum.Parse(typeof(EntityFaction), factions[0]);
                EntityFaction faction2 = (EntityFaction)System.Enum.Parse(typeof(EntityFaction), factions[1]);
                
                FactionRegistry.RemoveEnemy(faction1, faction2);
                FactionRegistry.RemoveEnemy(faction2, faction1);
                _temporaryEnemies.Remove(relationKey);
            }
        }

        private string GetRelationKey(EntityFaction faction1, EntityFaction faction2)
        {
            // Create a consistent key regardless of parameter order
            return (int)faction1 < (int)faction2 
                ? $"{faction1}|{faction2}" 
                : $"{faction2}|{faction1}";
        }

        public void AddTemporaryEnemy(EntityFaction faction, EntityFaction enemyFaction, float duration)
        {
            if (duration <= 0)
                return;

            string relationKey = GetRelationKey(faction, enemyFaction);
            float expirationTime = Time.time + duration;
            
            // If this isn't already a temporary enemy relationship, add as enemies
            if (!_temporaryEnemies.ContainsKey(relationKey))
            {
                FactionRegistry.AddEnemy(faction, enemyFaction);
                FactionRegistry.AddEnemy(enemyFaction, faction);
            }
            
            // Update/refresh expiration time
            _temporaryEnemies[relationKey] = expirationTime;
        }

        public void AddPermanentEnemy(EntityFaction faction, EntityFaction enemyFaction)
        {
            FactionRegistry.AddEnemy(faction, enemyFaction);
        }
    }
}