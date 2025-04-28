using Entity.NPC.AI;
using Helpers;
using UnityEngine;

namespace Entity.NPC.Idle
{
    public class NPCIdleState : NPCState
    {
        private NPCIdleProperties _npcIdleProperties;

        public NPCIdleState(NPCAIConfiguration npcaiConfiguration) : this(HelperAnimationStateName.IS_IDLING,
            new NPCIdleProperties(npcaiConfiguration))
        {
        }

        private NPCIdleState(string animationBoolName, NPCIdleProperties entityStateProperties) : base(
            animationBoolName, entityStateProperties)
        {
            _npcIdleProperties = entityStateProperties;
        }

        private float idleTimer;

        public override void EnterState()
        {
            base.EnterState();
            idleTimer = _npcIdleProperties.IdleTime;
        }

        public override void FixedUpdateState()
        {
            base.FixedUpdateState();
            idleTimer -= Time.fixedDeltaTime;
            if (idleTimer <= 0)
            {
                _stateMachine.ChangeState(npcAIController.NpcMoveState);
            }
        }
    }
}