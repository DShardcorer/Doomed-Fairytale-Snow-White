using Entity.NPC.Chase;
using UnityEngine;
using Pathfinding; // Add this namespace

namespace Entity.NPC.StandardAI.Chase
{
    public class StandardNPCMeleeChasingState : NPCChasingState
    {
        private StandardNPCMeleeChasingProperties _standardNpcMeleeChasingProperties;
        private Seeker _seeker; // Reference to the Seeker component
        private IAstarAI _astarAI; // Reference to AIPath or other IAstarAI implementation

        public StandardNPCMeleeChasingState(string animationBoolName, StandardNPCMeleeChasingProperties entityStateProperties) :
            base(animationBoolName, entityStateProperties)
        {
            _standardNpcMeleeChasingProperties = entityStateProperties;
        }

        public override void EnterState()
        {
            base.EnterState();
            
            // Get the required components if we don't have them already
            if (_seeker == null)
                _seeker = npc.View.GetComponent<Seeker>();
            
            if (_astarAI == null)
                _astarAI = npc.View.GetComponent<IAstarAI>();
            
            if (_seeker == null || _astarAI == null)
            {
                Debug.LogError("Missing Seeker or AIPath component on NPC GameObject");
                return;
            }
            
            // Enable the AI movement
            _astarAI.canMove = true;
            
            // Set destination to target position
            if (_properties.target != null)
            {
                _astarAI.destination = _properties.target.View.transform.position;
            }
        }

        public override void FixedUpdateState()
        {
            base.FixedUpdateState();
            
            if (_properties.target == null)
                return;

            // Update the destination every frame to follow the target
            _astarAI.destination = _properties.target.View.transform.position;
            
            // Store the movement vector for animation purposes
            _properties.lastMovementVector = _astarAI.velocity.normalized;
            
            
            // Set the movement speed
            _astarAI.maxSpeed = _properties.MoveSpeed * 1.5f;
            
            // Check if the target is in attack range
            if (Vector3.Distance(npc.View.transform.position, _properties.target.View.transform.position) <=
                _standardNpcMeleeChasingProperties.AttackRange)
            {
                if (npcAIController.NPCAttackingState == null)
                {
                    Debug.LogError("npcAttackingState is null");
                }

                _stateMachine.ChangeState(npcAIController.NPCAttackingState);
            }

            // Check if the target is out of chase range
            Vector3 myPosition = npc.View.transform.position;
            Vector3 targetPosition = npc.Properties.target.View.transform.position;
            if (Vector3.Distance(myPosition, targetPosition) >
                _standardNpcMeleeChasingProperties.ChaseRange)
            {
                npcAIController.UnsetTarget();
                npcAIController.ChangeState(npcAIController.NPCIdlingState);
            }
        }

        public override void ExitState()
        {
            base.ExitState();
            
            // Stop movement when exiting this state
            if (_astarAI != null)
            {
                _astarAI.canMove = false;
            }
        }
    }
}