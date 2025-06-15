using Entity.Faction;

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
            npcInteractingWith = npc;
        }
        public void ClearEntityInteractingWith()
        {
            npcInteractingWith = null;
        }
    }
}
