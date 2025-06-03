using System;
using Entity.NPC.AI;
using Entity.NPC.StandardAI.Chase;
using Entity.NPC.State.Attack;
using Helpers;
using UnityEngine;

namespace Entity.NPC.StandardAI.Attack
{
    public class StandardNpcMeleeAttackState : NPCAttackState
    {
        protected StandardNPCMeleeAttackProperties StandardNpcMeleeAttackProperties;

        public StandardNpcMeleeAttackState(NPCAIConfiguration npcaiConfiguration) :
            this(HelperAnimationStateName.IS_ATTACKING, new StandardNPCMeleeAttackProperties(npcaiConfiguration))
        {
        }

        private StandardNpcMeleeAttackState(string animationBoolName,
            StandardNPCMeleeAttackProperties entityStateProperties) : base(animationBoolName, entityStateProperties)
        {
            StandardNpcMeleeAttackProperties = entityStateProperties;
        }

        private float attackCooldownTimer;


        public override void FixedUpdateState()
        {
            base.FixedUpdateState();
            if (IsAnimationEnded())
            {
                npcAIController.ChangeState(HelperNPCStateName.Idle);
            }
        }

        protected override void OnTakingEffect(object sender, EventArgs e)
        {
            _entity.AttackHitbox.PerformAttack(StandardNpcMeleeAttackProperties.AttackType,
                StandardNpcMeleeAttackProperties.AttackDamage);
        }
    }
}