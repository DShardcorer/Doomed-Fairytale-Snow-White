using UnityEngine;
using Helpers;

namespace Entity.NPC.AI.SubControllers
{
    public class GuardNPCSubAIController : NPCSubAIController
    {
        private Vector3 _guardPosition;
        private float _guardRadius = 3f;
        private float _maxChaseDistance = 8f;
        private float _returnTimer = 0f;
        private float _returnDelay = 2f;
        private bool _isReturningToPost = false;
        private GuardState _currentGuardState = GuardState.Guarding;
        
        private enum GuardState
        {
            Guarding,
            Chasing,
            Returning
        }
        
        public override void Initialize(NPCAIController parent)
        {
            base.Initialize(parent);
            _guardRadius = config.patrolRadius * 0.6f;
            _maxChaseDistance = config.chaseRange;
            _returnDelay = config.patrolWaitTime;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            _guardPosition = npc.View.transform.position;
            _currentGuardState = GuardState.Guarding;
            _isReturningToPost = false;
            _returnTimer = 0f;
            
            ChangeToState(HelperNPCStateName.Idle);
        }
        
        public override void UpdateLogic()
        {
            switch (_currentGuardState)
            {
                case GuardState.Guarding:
                    HandleGuardingLogic();
                    break;
                case GuardState.Chasing:
                    HandleChasingLogic();
                    break;
                case GuardState.Returning:
                    HandleReturningLogic();
                    break;
            }
        }
        
        private void HandleGuardingLogic()
        {
            // Check if we're too far from our guard position
            float distanceFromPost = Vector3.Distance(npc.View.transform.position, _guardPosition);
            
            if (distanceFromPost > _guardRadius)
            {
                // Return to guard position
                MoveToPosition(_guardPosition);
                return;
            }
            
            // If we have a target, start chasing
            if (HasTarget())
            {
                _currentGuardState = GuardState.Chasing;
                
                if (IsInAttackRange())
                {
                    ChangeToState(HelperNPCStateName.Attack);
                }
                else
                {
                    ChangeToState(HelperNPCStateName.Chase);
                }
            }
            else
            {
                // Just guard - stay idle and alert
                string currentStateName = parent.GetCurrentState()?.GetType().Name;
                if (currentStateName != "NPCIdleState")
                {
                    ChangeToState(HelperNPCStateName.Idle);
                }
            }
        }
        
        private void HandleChasingLogic()
        {
            if (!HasTarget())
            {
                // Lost target, start returning
                _currentGuardState = GuardState.Returning;
                _returnTimer = 0f;
                return;
            }
            
            // Check if target is too far from guard post
            float targetDistanceFromPost = Vector3.Distance(npc.NPCProperties.target.View.transform.position, _guardPosition);
            
            if (targetDistanceFromPost > _maxChaseDistance)
            {
                // Target is too far, stop chasing and return
                parent.UnsetTarget();
                _currentGuardState = GuardState.Returning;
                _returnTimer = 0f;
                return;
            }
            
            // Normal combat logic
            FaceTarget();
            
            string currentStateName = parent.GetCurrentState()?.GetType().Name;
            
            switch (currentStateName)
            {
                case "NPCIdleState":
                    if (IsInAttackRange())
                    {
                        ChangeToState(HelperNPCStateName.Attack);
                    }
                    else
                    {
                        ChangeToState(HelperNPCStateName.Chase);
                    }
                    break;
                    
                case "StandardNpcMeleeChaseState":
                    if (IsInAttackRange())
                    {
                        ChangeToState(HelperNPCStateName.Attack);
                    }
                    break;
                    
                case "StandardNpcMeleeAttackState":
                    if (!IsInAttackRange())
                    {
                        ChangeToState(HelperNPCStateName.Chase);
                    }
                    break;
            }
        }
        
        private void HandleReturningLogic()
        {
            _returnTimer += Time.deltaTime;
            
            // If we get a new target while returning and it's close to our post, re-engage
            if (HasTarget())
            {
                float targetDistanceFromPost = Vector3.Distance(npc.NPCProperties.target.View.transform.position, _guardPosition);
                
                if (targetDistanceFromPost <= _maxChaseDistance)
                {
                    _currentGuardState = GuardState.Chasing;
                    return;
                }
            }
            
            // Move back to guard position
            float distanceFromPost = Vector3.Distance(npc.View.transform.position, _guardPosition);
            
            if (distanceFromPost > _guardRadius * 0.5f)
            {
                string currentStateName = parent.GetCurrentState()?.GetType().Name;
                if (currentStateName == "NPCIdleState")
                {
                    MoveToPosition(_guardPosition);
                }
            }
            else
            {
                // We're back at our post
                if (_returnTimer >= _returnDelay)
                {
                    _currentGuardState = GuardState.Guarding;
                    ChangeToState(HelperNPCStateName.Idle);
                }
            }
        }
        
        public override bool ShouldRemainActive()
        {
            // Guards always remain active unless explicitly told to change
            return true;
        }
        
        public override void FixedUpdateLogic()
        {
            if (_currentGuardState == GuardState.Chasing && HasTarget())
            {
                FaceTarget();
            }
            else if (_currentGuardState == GuardState.Guarding)
            {
                // Guards should be alert, maybe look around occasionally
                // For now, just maintain last movement vector
            }
        }
        
        // Public method to set a new guard position (useful for repositioning guards)
        public void SetGuardPosition(Vector3 newPosition)
        {
            _guardPosition = newPosition;
            _currentGuardState = GuardState.Returning;
            _returnTimer = 0f;
        }
    }
}