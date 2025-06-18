using System.Collections.Generic;
using UnityEngine;

namespace Entity.Faction
{
    public static class FactionRegistry
    {
        public static Dictionary<EntityFaction, FactionRelation> FactionRelations;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Initialize()
        {
            InitializeFactions();
            SetupAlliances();
            SetupEnemies();
        }

        private static void InitializeFactions()
        {
            FactionRelations = new Dictionary<EntityFaction, FactionRelation>();
            
            // Initialize all factions
            foreach (EntityFaction faction in System.Enum.GetValues(typeof(EntityFaction)))
            {
                FactionRelations[faction] = new FactionRelation(faction);
            }
        }

        private static void SetupAlliances()
        {
            // Create mutual alliance between two factions
            SetupMutualAlliance(EntityFaction.Player, EntityFaction.Civilized);
            SetupMutualAlliance(EntityFaction.Player, EntityFaction.Dwarfs);
            SetupMutualAlliance(EntityFaction.Civilized, EntityFaction.Dwarfs);
        }

        private static void SetupEnemies()
        {
            // Create mutual enemy relationship between factions
            SetupMutualEnemy(EntityFaction.Native, EntityFaction.Player);
            SetupMutualEnemy(EntityFaction.Native, EntityFaction.Civilized);
            SetupMutualEnemy(EntityFaction.Native, EntityFaction.Dwarfs);
            SetupMutualEnemy(EntityFaction.Monsters, EntityFaction.Player);
            SetupMutualEnemy(EntityFaction.Monsters, EntityFaction.Civilized);
            SetupMutualEnemy(EntityFaction.Animals, EntityFaction.Monsters);
        }

        private static void SetupMutualAlliance(EntityFaction factionA, EntityFaction factionB)
        {
            FactionRelations[factionA].AddAlly(factionB);
            FactionRelations[factionB].AddAlly(factionA);
        }

        private static void SetupMutualEnemy(EntityFaction factionA, EntityFaction factionB)
        {
            FactionRelations[factionA].AddEnemy(factionB);
            FactionRelations[factionB].AddEnemy(factionA);
        }

        public static bool AreAllies(EntityFaction factionA, EntityFaction factionB)
        {
            return FactionRelations[factionA].IsAlly(factionB);
        }

        public static bool AreEnemies(EntityFaction factionA, EntityFaction factionB)
        {
            return FactionRelations[factionA].IsEnemy(factionB);
        }
        public static void AddAlly(EntityFaction faction, EntityFaction ally)
        {
            if (FactionRelations.ContainsKey(faction))
            {
                FactionRelations[faction].AddAlly(ally);
            }
        }

        public static void AddEnemy(EntityFaction faction, EntityFaction enemy)
        {
            if (FactionRelations.ContainsKey(faction))
            {
                FactionRelations[faction].AddEnemy(enemy);
            }
        }
        public static void RemoveAlly(EntityFaction faction, EntityFaction ally)
        {
            if (FactionRelations.ContainsKey(faction))
            {
                FactionRelations[faction].Allies.Remove(ally);
            }
        }

        public static void RemoveEnemy(EntityFaction faction, EntityFaction enemy)
        {
            if (FactionRelations.ContainsKey(faction))
            {
                FactionRelations[faction].Enemies.Remove(enemy);
            }
        }
    }
}