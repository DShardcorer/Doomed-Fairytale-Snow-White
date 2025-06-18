using System.Collections.Generic;

namespace Entity.Faction
{
    public class FactionRelation
    {
        public EntityFaction Faction { get; private set; }
        public List<EntityFaction> Allies { get; private set; }
        public List<EntityFaction> Enemies { get; private set; }

        public FactionRelation(EntityFaction faction)
        {
            Faction = faction;
            Allies = new List<EntityFaction>();
            Enemies = new List<EntityFaction>();
        }
        public FactionRelation(EntityFaction faction, List<EntityFaction> allies, List<EntityFaction> enemies)
        {
            Faction = faction;
            Allies = allies ?? new List<EntityFaction>();
            Enemies = enemies ?? new List<EntityFaction>();
        }

        public bool IsAlly(EntityFaction faction)
        {
            return Allies.Contains(faction);
        }
        public bool IsEnemy(EntityFaction faction)
        {
            return Enemies.Contains(faction);
        }

        public void AddAlly(EntityFaction ally)
        {
            if (!Allies.Contains(ally))
            {
                Allies.Add(ally);
            }
        }

        public void AddEnemy(EntityFaction enemy)
        {
            if (!Enemies.Contains(enemy))
            {
                Enemies.Add(enemy);
            }
        }
    }
}