using EntityBase.NPC.AI;
using EntityBase.NPC.BehaviourTrees;
using EntityBase.NPC.State;
using Pathfinding;
using UnityEngine;

namespace EntityBase.NPC
{
    public class NPCState : EntityState, IStateful
    {
        protected IAstarAI astarAI; // Reference to AIPath or other IAstarAI implementation
        public IAstarAI AstarAI => astarAI; 
        protected NPCStateSystem stateSystem;
        private Node.Status _status = Node.Status.Running;
        public NPCState(string animationBoolName, EntityStateProperties entityStateProperties) : base(animationBoolName,
            entityStateProperties)
        {
        }

        protected NPC npc;

        protected NPCAIController npcAIController;
        public NPCAIController NPCAIController => npcAIController;

        public virtual void Initialize(NPC controller)
        {
            npc = controller;
            npcAIController = controller.NPCAIController;
            stateSystem = npc.NPCStateSystem;
            astarAI = npcAIController.AstarAI;
            base.Initialize(controller);
        }

        public override void EnterState()
        {
            base.EnterState();
            _status = Node.Status.Running;
        }

        public Node.Status GetStatus()
        {
            return _status;
        }

        public bool IsRunning => _status == Node.Status.Running;
        public bool HasSucceeded => _status == Node.Status.Success;
        public bool HasFailed => _status == Node.Status.Failure;

        protected void SetStatus(Node.Status status)
        {
            _status = status;
        }

        public virtual void OnSuccess()
        {
            SetStatus(Node.Status.Success);
        }

        public virtual void OnFailure()
        {
            SetStatus(Node.Status.Failure);
        }
    }
}