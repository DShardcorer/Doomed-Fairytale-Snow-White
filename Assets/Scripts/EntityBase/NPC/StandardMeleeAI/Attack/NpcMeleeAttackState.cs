using System;
using EntityBase.NPC.State.Attack;
using EntityBase.NPC.AI;
using Helpers;
using UnityEngine;

namespace EntityBase.NPC.StandardAI.Attack
{
    public class NpcMeleeAttackState : NPCState
    {
        protected NPCMeleeAttackProperties NpcMeleeAttackProperties;

        public NpcMeleeAttackState(NPCAIConfiguration npcaiConfiguration) :
            this(HelperAnimationStateName.IS_ATTACKING, new NPCMeleeAttackProperties(npcaiConfiguration))
        {
        }

        private NpcMeleeAttackState(string animationBoolName,
            NPCMeleeAttackProperties entityStateProperties) : base(animationBoolName, entityStateProperties)
        {
            NpcMeleeAttackProperties = entityStateProperties;
        }

        private float attackCooldownTimer;

        public override void EnterState()
        {
            base.EnterState();
            npcAIController.SetCurrentSubControllerBusy(true);
            npc.InvokeOnAttackStarts();
        }

        public override void ExitState()
        {
            base.ExitState();
            npcAIController.SetCurrentSubControllerBusy(false);
        }

        public override void UpdateState()
        {
            base.UpdateState();
            if (IsAnimationEnded())
            {
                npcAIController.ChangeState(HelperNPCStateName.Idle);
            }
        }


        protected override void OnTakingEffect()
        {
            base.OnTakingEffect();
            _entity.AttackHitbox.PerformAttack(NpcMeleeAttackProperties.AttackType,
                npc.StatSystem.CombatStatBoard.PhysicalAttack.ModifiedValue);
        }
    }
}