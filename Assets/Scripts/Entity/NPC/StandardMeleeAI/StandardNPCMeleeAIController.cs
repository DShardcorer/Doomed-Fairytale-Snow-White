using Entity.NPC.AI;
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
        }

        // The AI starts in the Idle state
        protected override NPCState GetInitialState()
        {
            return _npcIdlingState;
        }

        // When something enters the FOV, switch to chase if allowed
        protected override void OnTargetSpottedInFOV(object sender, Entity e)
        {
            if (_npc.IsBusy || !_config.shouldChaseTargets)
                return;

            _npc.NPCProperties.target = e;
            _stateMachine.ChangeState(_npcChasingState);
        }

        // When something gets close, optionally attack
        protected override void OnTargetSpottedInProximity(object sender, Entity e)
        {
            if (_npc.IsBusy)
                return;

            _npc.NPCProperties.lastMovementVector =
                (e.View.transform.position - _npc.View.transform.position).normalized;

            if (_config.shouldAttackTargets &&
                Vector3.Distance(_npc.View.transform.position, e.View.transform.position) <= _config.attackRange)
            {
                _npc.NPCProperties.target = e;
                _stateMachine.ChangeState(_npcAttackingState);
            }
        }
    }
}