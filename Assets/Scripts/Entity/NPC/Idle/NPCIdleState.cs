using Entity.NPC.AI;
using Helpers;
using UnityEngine;

namespace Entity.NPC.Idle
{
    public class NPCIdleState : NPCState
    {
        private NPCIdleProperties _npcIdleProperties;
        private float idleTimer;
        private string _stateToReturnToWhenIdleEnds;
        private bool _isSetup = false;

        public NPCIdleState(NPCAIConfiguration npcaiConfiguration) : this(HelperAnimationStateName.IS_IDLING,
            new NPCIdleProperties(npcaiConfiguration))
        {
        }

        private NPCIdleState(string animationBoolName, NPCIdleProperties entityStateProperties) : base(
            animationBoolName, entityStateProperties)
        {
            _npcIdleProperties = entityStateProperties;
        }



        public void Setup(string stateToReturnToWhenIdleEnds,float idleTime)
        {
            _isSetup = true;
            _stateToReturnToWhenIdleEnds = stateToReturnToWhenIdleEnds;
            idleTimer = idleTime;
        }

        public override void FixedUpdateState()
        {
            base.FixedUpdateState();
            if (!_isSetup) return;
            idleTimer -= Time.fixedDeltaTime;
            if (idleTimer <= 0)
            {
                _isSetup = false;
                npcAIController.ChangeState(_stateToReturnToWhenIdleEnds);
            }
        }
    }
}