using Entity.NPC.Chase;
using UnityEngine;

namespace Entity.NPC.StandardAI.Chase
{
    public class StandardNPCMeleeChasingState : NPCChasingState
    {
        private StandardNPCMeleeChasingProperties _standardNpcMeleeChasingProperties;

        public StandardNPCMeleeChasingState(string animationBoolName, EntityStateProperties entityStateProperties) :
            base(animationBoolName, entityStateProperties)
        {
        }

        private float _stateTimer;

        public override void FixedUpdateState()
        {
            base.FixedUpdateState();
            Vector2 roundedDirection = new Vector2(Mathf.Round(_properties.lastMovementVector.x),
                Mathf.Round(_properties.lastMovementVector.y));
            if (_properties.target == null)
            {
                _stateMachine.ChangeState(npcAIController.NPCIdlingState);
            }

            _properties.lastMovementVector =
                (_properties.target.View.transform.position - npc.View.transform.position).normalized;
            _rigidbody.linearVelocity = _properties.MoveSpeed * 1.5f * _properties.lastMovementVector;


            // Check if the target is in attack range
            if (Vector3.Distance(npc.View.transform.position, _properties.target.View.transform.position) <=
                _standardNpcMeleeChasingProperties.AttackRange)
            {
                _stateMachine.ChangeState(npcAIController.NPCAttackingState);
            }

            // Check if the target is out of chase range
            if (Vector3.Distance(npc.View.transform.position, _properties.target.View.transform.position) >
                npc.NPCProperties.ChaseRange)
            {
                _stateMachine.ChangeState(npcAIController.NPCIdlingState);
            }
        }

        public void SetTarget(Entity target)
        {
            _properties.target = target;
        }

        public void UnsetTarget()
        {
            _properties.target = null;
        }
    }
}