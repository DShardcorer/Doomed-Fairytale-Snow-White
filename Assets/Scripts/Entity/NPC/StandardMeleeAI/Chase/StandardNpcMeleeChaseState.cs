using Entity.NPC.AI;
using Entity.NPC.Chase;
using Helpers;
using UnityEngine;
using Pathfinding; // Add this namespace

namespace Entity.NPC.StandardAI.Chase
{
    public class StandardNpcMeleeChaseState : NPCChaseState
    {
        private StandardNPCMeleeChaseProperties _standardNpcMeleeChaseProperties;


        public StandardNpcMeleeChaseState(NPCAIConfiguration npcaiConfiguration) :
            this(HelperAnimationStateName.IS_CHASING, new StandardNPCMeleeChaseProperties(npcaiConfiguration))
        {
        }

        private StandardNpcMeleeChaseState(string animationBoolName,
            StandardNPCMeleeChaseProperties entityStateProperties) :
            base(animationBoolName, entityStateProperties)
        {
            _standardNpcMeleeChaseProperties = entityStateProperties;
        }

        public override void EnterState()
        {
            base.EnterState();
            
            // Enable the AI movement
            astarAI.canMove = true;

            // Set destination to target position
            if (_properties.target != null)
            {
                astarAI.destination = _properties.target.View.transform.position;
            }
        }

        public override void FixedUpdateState()
        {
            base.FixedUpdateState();

            if (_properties.target == null)
                return;
            //
            // // Update the destination every frame to follow the target
            // _astarAI.destination = _properties.target.View.transform.position;

            // Store the movement vector for animation purposes
            _properties.lastMovementVector = astarAI.velocity.normalized;


            // Set the movement speed
            astarAI.maxSpeed = _properties.MoveSpeed * 1.5f;

            // Check if the target is in attack range
            if (Vector3.Distance(npc.View.transform.position, _properties.target.View.transform.position) <=
                _standardNpcMeleeChaseProperties.AttackRange)
            {
                if (npcAIController.NpcAttackState == null)
                {
                    Debug.LogError("npcAttackingState is null");
                }

                _stateMachine.ChangeState(npcAIController.NpcAttackState);
            }

            // Check if the target is out of chase range
            Vector3 myPosition = npc.View.transform.position;
            Vector3 targetPosition = npc.Properties.target.View.transform.position;
            if (Vector3.Distance(myPosition, targetPosition) >
                _standardNpcMeleeChaseProperties.ChaseRange)
            {
                npcAIController.UnsetTarget();
                npcAIController.ChangeState(npcAIController.NpcIdleState);
            }
        }

        public override void ExitState()
        {
            base.ExitState();

            // Stop movement when exiting this state
            if (astarAI != null)
            {
                astarAI.canMove = false;
            }
        }
    }
}