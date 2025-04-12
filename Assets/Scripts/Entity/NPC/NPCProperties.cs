using System.Collections.Generic;
using Entity.Faction;

namespace Entity.NPC
{
    public class NPCProperties : EntityProperties
    {
        private List<EntityFaction> hostileToFactions;
        public List<EntityFaction> HostileToFactions => hostileToFactions;
        private float chaseRange;
        public float ChaseRange => chaseRange;


        public NPCProperties(EntityFaction entityFaction, List<EntityFaction> hostileToFactions,  float moveSpeed, float chaseRange) : base(entityFaction, moveSpeed)
        {
            this.chaseRange = chaseRange;
            this.hostileToFactions = hostileToFactions;
        }

    }
}

