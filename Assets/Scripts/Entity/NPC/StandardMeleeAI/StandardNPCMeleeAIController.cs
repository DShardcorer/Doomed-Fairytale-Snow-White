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


            NPCMoveState npcMoveState = new NPCMoveState(config);
            StandardNpcMeleeChaseState npcChaseState = new StandardNpcMeleeChaseState(config);
            StandardNpcMeleeAttackState npcAttackState = new StandardNpcMeleeAttackState(config);
            // Add states to the dictionary
            states.Add(HelperNPCStateName.Move, npcMoveState);
            states.Add(HelperNPCStateName.Chase, npcChaseState);
            states.Add(HelperNPCStateName.Attack, npcAttackState);

        }

        // The AI starts in the Idle state
        protected override NPCState GetInitialState()
        {
            return GetState(HelperNPCStateName.Idle);
        }

        protected override NPCSubAIController GetInitialNPCSubAIController()
        {
            throw new System.NotImplementedException();
        }

        public override void FixedUpdateLogic()
        {
            base.FixedUpdateLogic();
            if (npc.NPCProperties.target != null)
            {
                npc.NPCProperties.lastMovementVector =
                    (npc.NPCProperties.target.View.transform.position - npc.View.transform.position).normalized;
                if (Vector3.Distance(npc.View.transform.position, npc.NPCProperties.target.View.transform.position) <=
                    _config.attackRange && _npcStateStateMachine.CurrentState != GetState(HelperNPCStateName.Attack))
                    //enemy is in range
                {
                    ChangeState(HelperNPCStateName.Attack);
                }
            }
        }

        // When something enters the FOV, switch to chase if allowed
        protected override void OnTargetSpottedInFOV(object sender, Entity entity)
        {
            if (npc.IsBusy || !_config.shouldChaseTargets)
                return;

            SetTarget(entity);
            ChangeState(HelperNPCStateName.Chase);
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