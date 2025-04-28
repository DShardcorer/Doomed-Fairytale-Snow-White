using Entity.NPC.AI;
using Helpers;
using UnityEngine;

namespace Entity.NPC.Move
{
    public class NPCMoveState : NPCState
    {
        private NPCMoveProperties _npcMoveProperties;

        public NPCMoveState(NPCAIConfiguration npcaiConfiguration) : this(HelperAnimationStateName.IS_MOVING,
            new NPCMoveProperties(npcaiConfiguration))
        {
        }

        private NPCMoveState(string animationBoolName, NPCMoveProperties npcMoveProperties) : base(animationBoolName,
            npcMoveProperties)
        {
            _npcMoveProperties = npcMoveProperties;
        }

        private float movingTimer;


        public override void EnterState()
        {
            base.EnterState();
            astarAI.canMove = true;
            movingTimer = _npcMoveProperties.MovingTime;
            _properties.lastMovementVector = GetRandomDirection();
            //Get a random position towards the direction of the last movement vector
            Vector2 randomPosition =
                (Vector2)_view.transform.position + _properties.lastMovementVector * (_properties.MoveSpeed * _npcMoveProperties.MovingTime);
            astarAI.destination = randomPosition;
        }

        public override void FixedUpdateState()
        {
            base.FixedUpdateState();
            movingTimer -= Time.fixedDeltaTime;
            //set last movement vector to the current direction
            _properties.lastMovementVector = astarAI.velocity.normalized;
            npc.FOVDetector.SetColliderRotation(_properties.lastMovementVector);
            npc.AttackHitbox.SetAttackHitBoxRotation(_properties.lastMovementVector);
            if (movingTimer <= 0)
            {
                _stateMachine.ChangeState(npcAIController.NpcIdleState);
            }
        }

        public override void ExitState()
        {
            astarAI.canMove = false;
            base.ExitState();
        }


        public Vector2 GetRandomDirection()
        {
            return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        }
    }
}