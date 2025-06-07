using UnityEngine;
using Helpers;

namespace Entity.NPC.AI.SubControllers
{
    public class StealthNPCSubAIController : NPCSubAIController
    {
        private float _stealthTimer = 0f;
        private float _maxStealthTime = 10f;
        private float _hideDistance = 8f;
        private Vector3 _hidePosition;
        private bool _isHidden = false;
        private float _detectionCheckInterval = 0.5f;
        private float _lastDetectionCheck = 0f;
        
        public override void Initialize(NPCAIController parent)
        {
            base.Initialize(parent);
            _hideDistance = config.preferredDistance;
            _maxStealthTime = config.patrolWaitTime * 3f; // Longer stealth time
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            _stealthTimer = 0f;
            _isHidden = false;
            _lastDetectionCheck = 0f;
            
            if (HasTarget())
            {
                FindHidingSpot();
            }
            else
            {
                // No target to hide from, return to patrol
                RequestControllerChange("patrol", "No target to stealth from");
            }
        }
        
        private void FindHidingSpot()
        {
            if (!HasTarget()) return;
            
            Vector3 targetPosition = npc.NPCProperties.target.View.transform.position;
            Vector3 targetForward = npc.NPCProperties.target.View.transform.forward;
            
            // Try to find a position behind the target or away from their view
            Vector3 awayDirection = -targetForward;
            
            // Add some randomness to make hiding less predictable
            float randomAngle = Random.Range(-90f, 90f) * Mathf.Deg2Rad;
            Vector3 randomizedDirection = new Vector3(
                awayDirection.x * Mathf.Cos(randomAngle) - awayDirection.y * Mathf.Sin(randomAngle),
                awayDirection.x * Mathf.Sin(randomAngle) + awayDirection.y * Mathf.Cos(randomAngle),
                0
            ).normalized;
            
            _hidePosition = targetPosition + randomizedDirection * _hideDistance;
            MoveToPosition(_hidePosition);
        }
        
        public override void UpdateLogic()
        {
            _stealthTimer += Time.deltaTime;
            
            // Check if we've been stealthing too long
            if (_stealthTimer >= _maxStealthTime)
            {
                RequestControllerChange("patrol", "Stealth timeout reached");
                return;
            }
            
            // Lose target if we don't have one
            if (!HasTarget())
            {
                RequestControllerChange("patrol", "Lost target while stealthing");
                return;
            }
            
            string currentStateName = parent.GetCurrentState()?.GetType().Name;
            
            // Check if we've reached our hiding spot
            if (currentStateName == "NPCIdleState" && !_isHidden)
            {
                _isHidden = true;
                Debug.Log($"{npc.View.gameObject.name} has hidden from target");
            }
            
            // Periodic detection checks
            if (Time.time - _lastDetectionCheck >= _detectionCheckInterval)
            {
                _lastDetectionCheck = Time.time;
                CheckDetectionStatus();
            }
        }
        
        private void CheckDetectionStatus()
        {
            if (!HasTarget()) return;
            
            // Check if we're still hidden from the target
            if (IsVisibleToTarget())
            {
                // We've been spotted, find a new hiding spot or flee
                if (_stealthTimer < _maxStealthTime * 0.5f)
                {
                    // Try to find another hiding spot
                    _isHidden = false;
                    FindHidingSpot();
                }
                else
                {
                    // We've been trying to hide for too long, flee instead
                    RequestControllerChange("flee", "Spotted while trying to stealth");
                }
            }
            else if (_isHidden)
            {
                // We're successfully hidden, check if target has moved away
                float distanceToTarget = GetDistanceToTarget();
                if (distanceToTarget > _hideDistance * 2f)
                {
                    // Target is far enough, we can return to patrol
                    parent.UnsetTarget();
                    RequestControllerChange("patrol", "Target moved away while hidden");
                }
            }
        }
        
        private bool IsVisibleToTarget()
        {
            if (!HasTarget()) return false;
            
            Vector3 targetToNPC = npc.View.transform.position - npc.NPCProperties.target.View.transform.position;
            Vector3 targetForward = npc.NPCProperties.target.View.transform.forward;
            
            // Simple check - are we in front of target's view?
            float angle = Vector3.Angle(targetForward, targetToNPC);
            float distance = targetToNPC.magnitude;
            
            // Closer targets are easier to spot, wider angles
            float detectionAngle = Mathf.Lerp(30f, 90f, Mathf.InverseLerp(10f, 3f, distance));
            
            return angle < detectionAngle && distance < config.detectionRadius;
        }
        
        public override bool ShouldRemainActiveDespiteGlobalConditions()
        {
            // Remain active if we have a target and haven't timed out
            return HasTarget() && _stealthTimer < _maxStealthTime;
        }
        
        public override void FixedUpdateLogic()
        {
            if (HasTarget() && _isHidden)
            {
                // When hidden, try to face away from target to reduce visibility
                Vector3 awayDirection = (npc.View.transform.position - npc.NPCProperties.target.View.transform.position).normalized;
                npc.NPCProperties.lastMovementVector = awayDirection;
            }
        }
        
        public override void OnExit()
        {
            base.OnExit();
            _isHidden = false;
        }
    }
}