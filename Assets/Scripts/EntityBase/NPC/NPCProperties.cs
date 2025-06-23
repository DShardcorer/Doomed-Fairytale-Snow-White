using System.Collections.Generic;
using EntityBase.Faction;

namespace EntityBase.NPC
{
    public class NPCProperties : EntityProperties
    {
        private List<EntityFaction> hostileToFactions;
        public List<EntityFaction> HostileToFactions => hostileToFactions;



        public NPCProperties(EntityFaction entityFaction, List<EntityFaction> hostileToFactions,  float moveSpeed, float chaseRange) : base(entityFaction, moveSpeed)
        {

            this.hostileToFactions = hostileToFactions;
        }

    }
}

