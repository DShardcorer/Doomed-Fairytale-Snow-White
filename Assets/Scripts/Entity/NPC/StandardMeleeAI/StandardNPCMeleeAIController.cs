using Entity.NPC.AI;
using Entity.NPC.BeingInteractedWith;
using Entity.NPC.Idle;
using Entity.NPC.Move;
using Entity.NPC.StandardAI.Attack;
using Entity.NPC.StandardAI.Chase;
using Helpers;
using UnityEngine;

namespace Entity.NPC.StandardAI
{
    public class StandardNPCMeleeAIController : NPCAIController
    {
        public StandardNPCMeleeAIController(NPCAIConfiguration config) : base(config)
        {
            // Instantiate each state using the shared configuration

            _npcIdlingState = new NPCIdlingState(HelperAnimationStateName.IS_IDLING, new NPCIdlingProperties(config));
            _npcMovingState = new NPCMovingState(HelperAnimationStateName.IS_MOVING, new NPCMovingProperties(config));
            _npcChasingState = new StandardNPCMeleeChasingState(HelperAnimationStateName.IS_CHASING,
                new StandardNPCMeleeChasingProperties(config));
            _npcAttackingState = new StandardNPCMeleeAttackingState(HelperAnimationStateName.IS_ATTACKING,
                new StandardNPCMeleeAttackingProperties(config));
            _npcBeingInteractedWithState = new NPCBeingInteractedWithState(HelperAnimationStateName.IS_IDLING,
                new NPCBeingInteractedWithProperties());
        }

        // The AI starts in the Idle state
        protected override NPCState GetInitialState()
        {
            return _npcIdlingState;
        }

        public override void FixedUpdateLogic()
        {
            base.FixedUpdateLogic();
            if (npc.NPCProperties.target != null)
            {
                npc.NPCProperties.lastMovementVector =
                    (npc.NPCProperties.target.View.transform.position - npc.View.transform.position).normalized;
                if (Vector3.Distance(npc.View.transform.position, npc.NPCProperties.target.View.transform.position) <=
                    _config.attackRange && _stateMachine.CurrentState != _npcAttackingState)
                    //enemy is in range
                {
                    ChangeState(_npcAttackingState);
                }
            }
        }

        // When something enters the FOV, switch to chase if allowed
        protected override void OnTargetSpottedInFOV(object sender, Entity entity)
        {
            if (npc.IsBusy || !_config.shouldChaseTargets)
                return;

            SetTarget(entity);
            ChangeState(_npcChasingState);
        }

        // When something gets close, optionally attack
        protected override void OnTargetSpottedInProximity(object sender, Entity e)
        {
            if (npc.IsBusy)
                return;

            npc.NPCProperties.lastMovementVector =
                (e.View.transform.position - npc.View.transform.position).normalized;
        }
    }
}