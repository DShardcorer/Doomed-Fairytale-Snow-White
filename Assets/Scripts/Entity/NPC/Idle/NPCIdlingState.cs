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

        public override void EnterState()
        {
            base.EnterState();
            _stateTimer = _npcIdlingProperties.IdleTime;
        }

        public override void FixedUpdateState()
        {
            base.FixedUpdateState();
            _stateTimer -= Time.fixedDeltaTime;
            if (_stateTimer <= 0)
            {
                _stateMachine.ChangeState(_npc.NPCMovingState);
            }
        }
    }
}
