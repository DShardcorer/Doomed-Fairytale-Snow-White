using Entity.NPC.AI;

namespace Entity.NPC
{
    public class NPCState : EntityState
    {
        public NPCState(string animationBoolName, EntityStateProperties entityStateProperties) : base(animationBoolName, entityStateProperties)
        {
        }

        protected NPC npc;
        public NPC NPC => npc;

        protected NPCAIController npcAIController;
        public NPCAIController NPCAIController => npcAIController;
        public virtual void Initialize(NPC controller)
        {
            npc = controller;
            npcAIController = controller.NPCAIController;
            base.Initialize(controller);
        }
    }
}

