using Pathfinding;
using UnityEngine;

namespace EntityBase.NPC.BehaviourTrees.Strategies
{
    public class MoveToPositionStrategy : IStrategy
    {
        private NPC npc;
        private IAstarAI astarAI;

        public MoveToPositionStrategy(NPC npc, Vector3 position)
        {
            this.npc = npc;
            this.astarAI = npc.View.GetComponent<IAstarAI>();

            astarAI.canMove = true;
            astarAI.destination = position;
        }

        public Node.Status Process()
        {
            if (astarAI.reachedDestination)
            {
                return Node.Status.Success;
            }

            return Node.Status.Running;
        }
    }
}