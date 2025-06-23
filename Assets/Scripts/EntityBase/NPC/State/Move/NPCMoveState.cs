using EntityBase.NPC.AI;
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
        }

        public override void EnterState()
        {
            base.EnterState();
            astarAI.canMove = true;
            _properties.lastMovementVector = (_targetPosition - npc.View.transform.position).normalized;
            astarAI.destination = _targetPosition;
        }

        public override void FixedUpdateState()
        {
            base.FixedUpdateState();
            //set last movement vector to the current direction
            _properties.lastMovementVector = astarAI.velocity.normalized;
            npc.FOVDetector.SetColliderRotation(_properties.lastMovementVector);
            npc.AttackHitbox.SetAttackHitBoxRotation(_properties.lastMovementVector);
            if (astarAI.reachedDestination)
            {
                astarAI.canMove = false;

                if (_stateToReturnToWhenMovingEnds != null)
                    npcAIController.ChangeState(_stateToReturnToWhenMovingEnds);
                else
                    npcAIController.ChangeState(HelperNPCStateName.Idle);
            }
        }

        public override void ExitState()
        {
            astarAI.canMove = false;
            base.ExitState();
        }

        
    }
}