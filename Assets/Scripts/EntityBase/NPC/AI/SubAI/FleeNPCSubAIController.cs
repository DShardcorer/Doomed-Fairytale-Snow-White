using UnityEngine;
using Helpers;

namespace EntityBase.NPC.AI.SubControllers
{
    public class FleeNPCSubAIController : NPCSubAIController
    {
        private float _fleeTimer = 0f;
        private float _fleeTimeout = 5f;
        private float _fleeDistance = 10f;
        private bool _hasSetFleeDestination = false;
        private Vector3 _safePosition;
        private int _fleeAttempts = 0;
        private int _maxFleeAttempts = 3;
        private string _subControllerToChangeToWhenFleeingFinished;

        public FleeNPCSubAIController(string subControllerToChangeToWhenFleeingFinished)
        {
            _subControllerToChangeToWhenFleeingFinished = subControllerToChangeToWhenFleeingFinished;
        }

        public override void Initialize(NPCAIController parent)
        {
            base.Initialize(parent);
            _fleeDistance = config.fleeDistance;
            _fleeTimeout = config.fleeTime;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _fleeTimer = 0f;
            _hasSetFleeDestination = false;
            _fleeAttempts = 0;
            SetFleeDestination();
        }

        private void SetFleeDestination()
        {
            _fleeAttempts++;
            Vector3 fleeDirection;

            if (HasTarget())
            {
                // Flee away from target
                fleeDirection = (npc.View.transform.position - npc.NPCProperties.target.View.transform.position).normalized;
            }
            else
            {
                // Flee in random direction if no specific threat
                fleeDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
            }

            // Add some randomness to avoid predictable movement
            float randomAngle = Random.Range(-45f, 45f) * Mathf.Deg2Rad;
            Vector3 randomizedDirection = new Vector3(
                fleeDirection.x * Mathf.Cos(randomAngle) - fleeDirection.y * Mathf.Sin(randomAngle),
                fleeDirection.x * Mathf.Sin(randomAngle) + fleeDirection.y * Mathf.Cos(randomAngle),
                0
            ).normalized;

            _safePosition = npc.View.transform.position + randomizedDirection * _fleeDistance;
            MoveToPosition(_safePosition);
            _hasSetFleeDestination = true;
        }

        public override void UpdateLogic()
        {
            _fleeTimer += Time.deltaTime;

            // If we've been fleeing long enough, return to other controller
            if (_fleeTimer >= _fleeTimeout)
            {
                parent.UnsetTarget();
                RequestControllerChange(_subControllerToChangeToWhenFleeingFinished, "Flee timeout reached");
                return;
            }

            string currentStateId = parent.GetCurrentStateId();

            // If we've reached our destination or are idle
            if (currentStateId == HelperNPCStateName.Idle && _hasSetFleeDestination)
            {
                // Check if we need to continue fleeing
                if (ShouldContinueFleeing())
                {
                    if (_fleeAttempts < _maxFleeAttempts)
                    {
                        _hasSetFleeDestination = false;
                        SetFleeDestination();
                    }
                    else
                    {
                        // We've tried enough, just stay here and wait
                        // Do nothing, just wait for timeout
                    }
                }
            }

            // If we're moving and there's immediate danger, might need to change direction
            if (currentStateId == HelperNPCStateName.Move && HasTarget())
            {
                float distanceToThreat = GetDistanceToTarget();
                if (distanceToThreat < _fleeDistance * 0.3f)
                {
                    // Threat is still very close, set new destination
                    _hasSetFleeDestination = false;
                    SetFleeDestination();
                }
            }
        }

        private bool ShouldContinueFleeing()
        {
            // Continue fleeing if:
            // 1. We still have a target and it's close
            // 2. We haven't been fleeing for too long
            // 3. We haven't reached our flee attempts limit

            if (!HasTarget()) return false;

            float distanceToThreat = GetDistanceToTarget();
            float safeDistance = _fleeDistance * 0.7f;

            return distanceToThreat < safeDistance && _fleeTimer < _fleeTimeout * 0.8f;
        }

        public override bool ShouldRemainActiveDespiteGlobalConditions()
        {
            // Remain active until timeout or we feel safe
            if (_fleeTimer >= _fleeTimeout) return false;

            // If no target, we can stop fleeing
            if (!HasTarget()) return false;

            // If target is far enough, we can stop fleeing
            float distanceToThreat = GetDistanceToTarget();
            return distanceToThreat < _fleeDistance;
        }

        public override void FixedUpdateLogic()
        {
            // Continue facing away from threat while fleeing
            if (HasTarget())
            {
                Vector3 fleeDirection = (npc.View.transform.position - npc.NPCProperties.target.View.transform.position).normalized;
                npc.NPCProperties.lastMovementVector = fleeDirection;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            // Clear any movement when exiting flee state
            if (parent.AstarAI != null)
            {
                parent.AstarAI.destination = npc.View.transform.position;
            }
        }
    }
}