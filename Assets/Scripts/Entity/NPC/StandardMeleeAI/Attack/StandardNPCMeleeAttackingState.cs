using System;
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
            if (IsAnimationEnded())
            {
                npcAIController.ChangeState(npcAIController.NPCIdlingState);
            }
        }

        protected override void OnTakingEffect(object sender, EventArgs e)
        {
            _entity.AttackHitbox.PerformAttack(_standardNPCMeleeAttackingProperties.AttackType,
                _standardNPCMeleeAttackingProperties.AttackDamage);
        }
    }
}