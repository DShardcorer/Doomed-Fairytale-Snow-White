using Entity.NPC.Chase;
using UnityEngine;

namespace Entity.NPC.StandardAI.Chase
{
    public class StandardNPCMeleeChasingState : NPCChasingState
    {
        private StandardNPCMeleeChasingProperties _standardNpcMeleeChasingProperties;

        public StandardNPCMeleeChasingState(string animationBoolName, StandardNPCMeleeChasingProperties entityStateProperties) :
            base(animationBoolName, entityStateProperties)
        {
            _standardNpcMeleeChasingProperties = entityStateProperties;
        }

        private float _stateTimer;

        public override void FixedUpdateState()
        {
            base.FixedUpdateState();
            Vector2 roundedDirection = new Vector2(Mathf.Round(_properties.lastMovementVector.x),
                Mathf.Round(_properties.lastMovementVector.y));
            // _properties.lastMovementVector =
            //     (_properties.target.View.transform.position - npc.View.transform.position).normalized;
            _rigidbody.linearVelocity = _properties.MoveSpeed * 1.5f * _properties.lastMovementVector;
            
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
            if (Vector3.Distance(myPosition, targetPosition ) >
                _standardNpcMeleeChasingProperties.ChaseRange)
            {
                npcAIController.UnsetTarget();
                npcAIController.ChangeState(npcAIController.NPCIdlingState);
            }
        }
    }
}