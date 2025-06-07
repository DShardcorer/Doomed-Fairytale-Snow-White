using UnityEngine;
using Helpers;

namespace Entity.NPC.AI.SubControllers
{
    public class GuardNPCSubAIController : NPCSubAIController
    {
        private Vector3 _guardPosition;
        private float _guardRadius = 3f;
        private float _returnTimer = 0f;
        private float _returnDelay = 2f;
        private GuardState _currentGuardState = GuardState.Guarding;

        private enum GuardState
        {
            Guarding,
            Returning
        }

        public override void Initialize(NPCAIController parent)
        {
            base.Initialize(parent);
            _guardRadius = config.patrolRadius * 0.6f;
            _returnDelay = config.patrolWaitTime;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _guardPosition = npc.View.transform.position;
            _currentGuardState = GuardState.Guarding;
            _returnTimer = 0f;
            ChangeToState(HelperNPCStateName.Idle);
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
            switch (_currentGuardState)
            {
                case GuardState.Guarding:
                    HandleGuardingLogic();
                    break;
                case GuardState.Returning:
                    HandleReturningLogic();
                    break;
            }
        }

        private void HandleGuardingLogic()
        {
            float distanceFromPost = Vector3.Distance(npc.View.transform.position, _guardPosition);

            if (distanceFromPost > _guardRadius)
            {
                MoveToPosition(_guardPosition);
                return;
            }

            if (HasTarget())
            {
                // Delegate all combat to CombatNPCSubAIController
                RequestControllerChange(HelperNPCSubAIControllerName.Combat, "Target spotted while guarding");
                return;
            }

            string currentStateId = parent.GetCurrentStateId();
            if (currentStateId != HelperNPCStateName.Idle)
            {
                ChangeToState(HelperNPCStateName.Idle);
            }
        }

        private void HandleReturningLogic()
        {
            _returnTimer += Time.deltaTime;

            // If we get a new target while returning and it's close to our post, re-engage
            if (HasTarget())
            {
                float targetDistanceFromPost = Vector3.Distance(npc.NPCProperties.target.View.transform.position, _guardPosition);
                if (targetDistanceFromPost <= _guardRadius * 2f)
                {
                    RequestControllerChange(HelperNPCSubAIControllerName.Combat, "Target spotted while returning");
                    return;
                }
            }

            float distanceFromPost = Vector3.Distance(npc.View.transform.position, _guardPosition);

            if (distanceFromPost > _guardRadius * 0.5f)
            {
                string currentStateId = parent.GetCurrentStateId();
                if (currentStateId == HelperNPCStateName.Idle)
                {
                    MoveToPosition(_guardPosition);
                }
            }
            else
            {
                if (_returnTimer >= _returnDelay)
                {
                    _currentGuardState = GuardState.Guarding;
                    ChangeToState(HelperNPCStateName.Idle);
                }
            }
        }

        public override bool ShouldRemainActiveDespiteGlobalConditions()
        {
            // Guards always remain active unless explicitly told to change
            return true;
        }
        
        public void SetGuardPosition(Vector3 newPosition)
        {
            _guardPosition = newPosition;
            _currentGuardState = GuardState.Returning;
            _returnTimer = 0f;
        }
    }
}