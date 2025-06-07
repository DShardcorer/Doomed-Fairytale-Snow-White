
using UnityEngine;
using Helpers;

namespace Entity.NPC.AI.SubControllers
{
    public class RangedCombatNPCSubAIController : NPCSubAIController
    {
        private float _lostTargetTimer = 0f;
        private float _lostTargetTimeout = 3f;
        private Vector3 _lastTargetPosition;
        private float _preferredDistance = 6f;
        private float _minDistance = 3f;
        private float _maxDistance = 8f;
        private float _repositionCooldown = 1f;
        private float _lastRepositionTime = 0f;
        
        public override void Initialize(NPCAIController parent)
        {
            base.Initialize(parent);
            _preferredDistance = config.preferredDistance;
            _minDistance = _preferredDistance * 0.6f;
            _maxDistance = config.attackRange;
            _lostTargetTimeout = config.fleeTime * 0.6f; // Shorter timeout for ranged units
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            _lostTargetTimer = 0f;
            _lastRepositionTime = 0f;
            
            if (HasTarget())
            {
                _lastTargetPosition = npc.NPCProperties.target.View.transform.position;
                DetermineRangedAction();
            }
            else
            {
                ChangeToState(HelperNPCStateName.Idle);
            }
        }
        
        public override void UpdateLogic()
        {
            if (!HasTarget())
            {
                HandleLostTarget();
                return;
            }
            
            _lostTargetTimer = 0f;
            _lastTargetPosition = npc.NPCProperties.target.View.transform.position;
            
            FaceTarget();
            
            float distance = GetDistanceToTarget();
            string currentStateName = parent.GetCurrentState()?.GetType().Name;
            
            // Handle different states
            switch (currentStateName)
            {
                case "NPCIdleState":
                    DetermineRangedAction();
                    break;
                    
                case "NPCMoveState":
                    // Check if we've reached a good position
                    if (IsInGoodPosition(distance))
                    {
                        ChangeToState(HelperNPCStateName.Attack);
                    }
                    break;
                    
                case "StandardNpcMeleeAttackState": // Using melee attack state for now
                    // Check if we need to reposition
                    if (!IsInGoodPosition(distance) && CanReposition())
                    {
                        DetermineRangedAction();
                    }
                    break;
                    
                case "StandardNpcMeleeChaseState":
                    // If we're chasing but get to good range, stop and attack
                    if (IsInGoodPosition(distance))
                    {
                        ChangeToState(HelperNPCStateName.Attack);
                    }
                    break;
            }
        }
        
        private bool IsInGoodPosition(float distance)
        {
            return distance >= _minDistance && distance <= _maxDistance;
        }
        
        private bool CanReposition()
        {
            return Time.time - _lastRepositionTime >= _repositionCooldown;
        }
        
        private void DetermineRangedAction()
        {
            if (!HasTarget()) return;
            
            float distance = GetDistanceToTarget();
            
            if (distance < _minDistance)
            {
                // Too close, back away
                Vector3 direction = npc.View.transform.position - npc.NPCProperties.target.View.transform.position;
                Vector3 retreatPosition = npc.View.transform.position + direction.normalized * (_preferredDistance - distance + 2f);
                MoveToPosition(retreatPosition);
                _lastRepositionTime = Time.time;
            }
            else if (distance > _maxDistance)
            {
                // Too far, get closer but maintain preferred distance
                Vector3 direction = (npc.NPCProperties.target.View.transform.position - npc.View.transform.position).normalized;
                Vector3 approachPosition = npc.NPCProperties.target.View.transform.position - direction * _preferredDistance;
                MoveToPosition(approachPosition);
                _lastRepositionTime = Time.time;
            }
            else
            {
                // Good distance, attack
                ChangeToState(HelperNPCStateName.Attack);
            }
        }
        
        private void HandleLostTarget()
        {
            _lostTargetTimer += Time.deltaTime;
            
            if (_lostTargetTimer < _lostTargetTimeout)
            {
                // Search at last known position
                string currentStateName = parent.GetCurrentState()?.GetType().Name;
                
                if (currentStateName == "NPCIdleState")
                {
                    MoveToPosition(_lastTargetPosition);
                }
            }
            else
            {
                // Give up and request return to patrol
                parent.UnsetTarget();
                RequestControllerChange("patrol", "Lost ranged target for too long");
            }
        }
        
        public override bool ShouldRemainActiveDespiteGlobalConditions()
        {
            // Remain active as long as we have a target or are still searching
            return HasTarget() || _lostTargetTimer < _lostTargetTimeout;
        }
        
        public override void FixedUpdateLogic()
        {
            if (HasTarget())
            {
                FaceTarget();
                
                // Additional logic: If enemy gets too close while we're not moving, consider fleeing
                float distance = GetDistanceToTarget();
                if (distance < _minDistance * 0.5f)
                {
                    string currentStateName = parent.GetCurrentState()?.GetType().Name;
                    if (currentStateName != "NPCMoveState")
                    {
                        // Emergency retreat
                        RequestControllerChange("flee", "Enemy too close for ranged combat");
                    }
                }
            }
        }
    }
}