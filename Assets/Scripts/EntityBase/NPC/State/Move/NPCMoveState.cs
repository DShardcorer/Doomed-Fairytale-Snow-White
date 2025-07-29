using EntityBase.NPC.AI;
using EntityBase.NPC.BehaviourTrees;
using Helpers;
using UnityEngine;

namespace EntityBase.NPC.State.Move
{
    public class NPCMoveState : NPCState
    {
        private NPCMoveProperties _npcMoveProperties;
        private string _stateToReturnToWhenMovingEnds;
        private Vector3 _targetPosition;

        public NPCMoveState(NPCAIConfiguration npcaiConfiguration) : this(HelperAnimationStateName.IS_MOVING,
            new NPCMoveProperties(npcaiConfiguration))
        {
        }

        private NPCMoveState(string animationBoolName, NPCMoveProperties npcMoveProperties) : base(animationBoolName,
            npcMoveProperties)
        {
            _npcMoveProperties = npcMoveProperties;
        }

        public void Setup(string stateToReturnToWhenMovingEnds, Vector3 targetPosition)
        {
            _stateToReturnToWhenMovingEnds = stateToReturnToWhenMovingEnds;
            _targetPosition = targetPosition;
            SetStatus(Node.Status.Running);
        }

        public override void EnterState()
        {
            base.EnterState();
            astarAI.canMove = true;
            astarAI.destination = _targetPosition;
        }

        public override void FixedUpdateState()
        {
            base.FixedUpdateState();
            
            // Set last movement vector to the current direction
            _properties.lastMovementVector = astarAI.velocity.normalized;
            
            if (astarAI.reachedDestination)
            {
                astarAI.canMove = false;
                
                // Set success status before changing state
                SetStatus(Node.Status.Success);
                
                if (_stateToReturnToWhenMovingEnds != null)
                    stateSystem.ChangeState(_stateToReturnToWhenMovingEnds);
                else
                    stateSystem.ChangeState(HelperNPCStateName.Idle);
            }
        }

        public override void ExitState()
        {
            astarAI.canMove = false;
            base.ExitState();
        }
    }
}