using Entity.NPC.Attack;
using UnityEngine;

namespace Entity.NPC.StandardAI.Attack
{
    public class StandardNPCMeleeAttackingState : NPCAttackingState
    {
        protected StandardNPCMeleeAttackingProperties _standardNPCMeleeAttackingProperties;

        public StandardNPCMeleeAttackingState(string animationBoolName,
            StandardNPCMeleeAttackingProperties entityStateProperties) : base(animationBoolName, entityStateProperties)
        {
            _standardNPCMeleeAttackingProperties = entityStateProperties;
        }

        private float attackCooldownTimer;

        public override void FixedUpdateState()
        {
            base.FixedUpdateState();
            if (!_isAnimationEnded)
            {
                _rigidbody.linearVelocity = 0.25f * npc.NPCProperties.lastMovementVector * npc.NPCProperties.MoveSpeed;
            }
            else
            {
                if (Vector3.Distance(npc.View.transform.position, _properties.target.View.transform.position) <=
                    _standardNPCMeleeAttackingProperties.AttackRange)
                    //enemy is in range
                {
                    _stateMachine.ChangeState(npcAIController.NPCAttackingState);
                }
                else //enemy is out of range
                {
                    _stateMachine.ChangeState(npcAIController.NPCChasingState);
                }
            }
        }
    }
}