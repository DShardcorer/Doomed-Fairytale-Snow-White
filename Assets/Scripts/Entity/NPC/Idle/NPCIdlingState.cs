using UnityEngine;

namespace Entity.NPC.Idle
{
    public class NPCIdlingState : NPCState
    {
        private NPCIdlingProperties _npcIdlingProperties;

        public NPCIdlingState(string animationBoolName, NPCIdlingProperties entityStateProperties) : base(animationBoolName, entityStateProperties)
        {
            _npcIdlingProperties = entityStateProperties;
        }
        private float idleTimer;

        public override void EnterState()
        {
            base.EnterState();
            idleTimer = _npcIdlingProperties.IdleTime;
        }

        public override void FixedUpdateState()
        {
            base.FixedUpdateState();
            idleTimer -= Time.fixedDeltaTime;
            if (idleTimer <= 0)
            {
                _stateMachine.ChangeState(npcAIController.NPCMovingState);
            }
        }
    }
}
