using EntityBase.NPC.AI;
using Pathfinding;
using UnityEngine;

namespace EntityBase.NPC
{
    public class NPCState : EntityState
    {
        protected Seeker seeker; // Reference to the Seeker component
        public Seeker Seeker => seeker;
        protected IAstarAI astarAI; // Reference to AIPath or other IAstarAI implementation
        public IAstarAI AstarAI => astarAI; 

        public NPCState(string animationBoolName, EntityStateProperties entityStateProperties) : base(animationBoolName,
            entityStateProperties)
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
            seeker = npcAIController.Seeker;
            astarAI = npcAIController.AstarAI;
            base.Initialize(controller);
        }
    }
}