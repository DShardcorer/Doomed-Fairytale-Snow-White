using System;
using Entity.NPC.AI;
using Entity.NPC.State.Attack;
using Helpers;
using UnityEngine;

namespace Entity.NPC.StandardAI.Attack
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

        public override void FixedUpdateState()
        {
            base.FixedUpdateState();
        }


        protected override void OnTakingEffect(object sender, EventArgs e)
        {
            base.OnTakingEffect(sender, e);
            _entity.AttackHitbox.PerformAttack(NpcMeleeAttackProperties.AttackType,
                npc.StatSystem.CombatStatBoard.PhysicalAttack.ModifiedValue);
        }
    }
}