using Helpers;
using UnityEngine;

namespace EntityBase.NPC.BehaviourTrees.Strategies
{
    public class MoveToPositionStrategy : IStrategy
    {
        private readonly NPC npc;
        private readonly Vector3 targetPosition;
        private readonly string returnState;
        private NPCStateSystem stateSystem;
        private bool hasStartedMoving = false;

        public MoveToPositionStrategy(NPC npc, Vector3 position, string returnState = null)
        {
            this.npc = npc;
            this.targetPosition = position;
            this.returnState = returnState ?? HelperNPCStateName.Idle;
            this.stateSystem = npc.NPCStateSystem;
        }

        public Node.Status Process()
        {
            // First time or if we're not in the move state, start moving
            if (!hasStartedMoving || stateSystem.CurrentStateId != HelperNPCStateName.Move)
            {
                stateSystem.MoveToPosition(targetPosition, returnState);
                hasStartedMoving = true;
                return Node.Status.Running;
            }

            // We're in the move state, check its status
            if (stateSystem.CurrentState.HasSucceeded)
            {
                return Node.Status.Success;
            }
            else if (stateSystem.CurrentState.HasFailed)
            {
                return Node.Status.Failure;
            }

            return Node.Status.Running;
        }

        public void Reset()
        {
            hasStartedMoving = false;
        }
    }
}