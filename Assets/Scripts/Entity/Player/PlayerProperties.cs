using Entity.Faction;
using UnityEngine;

namespace Entity.Player
{
    public class PlayerProperties : EntityProperties
    {
        public PlayerProperties(EntityFaction entityFaction, float maxHealth) : base(entityFaction, maxHealth)
        {
        }
        private NPC.NPC npcInteractingWith;
        public NPC.NPC NPCInteractingWith => npcInteractingWith;
        
        public void SetEntityInteractingWith(NPC.NPC npc)
        {
            Debug.LogWarning("Set Entity Interacting With: " + npc.Profile.Name);
            npcInteractingWith = npc;
        }
        public void ClearEntityInteractingWith()
        {
            npcInteractingWith = null;
        }
    }
}
