// WanderNPCSubAIController.cs
using UnityEngine;
using Helpers;

namespace EntityBase.NPC.AI.SubControllers
{
    public class WanderNPCSubAIController : NPCSubAIController
    {
        private float _wanderTimer = 0f;
        private float _wanderInterval;
        private float _wanderRadius = 10f;
        private Vector3 _wanderCenter;
        private bool _isWandering = false;
        private float _pauseTimer = 0f;
        private float _pauseDuration;
        private string _subcontrollerIdToChangeToWhenTargetSpotted;

        public WanderNPCSubAIController(string subcontrollerIdToChangeToWhenTargetSpotted)
        {
            _subcontrollerIdToChangeToWhenTargetSpotted = subcontrollerIdToChangeToWhenTargetSpotted;
        }
        
        public override void Initialize(NPCAIController parent)
        {
            base.Initialize(parent);
            _wanderRadius = config.patrolRadius * 1.5f;
            _wanderInterval = Random.Range(config.patrolWaitTime * 0.5f, config.patrolWaitTime * 2f);
            _pauseDuration = Random.Range(1f, config.patrolWaitTime);
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            _wanderCenter = npc.View.transform.position;
            _wanderTimer = 0f;
            _pauseTimer = 0f;
            _isWandering = false;
            
            ChangeToState(HelperNPCStateName.Idle);
        }
        
        public override void UpdateLogic()
        {
            // If we spot a target, switch to appropriate behavior
            if (HasTarget())
            {
                RequestControllerChange(_subcontrollerIdToChangeToWhenTargetSpotted, "Target spotted while wandering");
                return;
            }
            
            string currentStateId = parent.GetCurrentStateId();
            
            if (_isWandering)
            {
                // We're currently moving to a wander destination
                if (currentStateId == HelperNPCStateName.Idle)
                {
                    // We've reached our destination, start pausing
                    _isWandering = false;
                    _pauseTimer = 0f;
                    _pauseDuration = Random.Range(1f, config.patrolWaitTime);
                }
            }
            else
            {
                // We're pausing between wander movements
                _pauseTimer += Time.deltaTime;
                
                if (_pauseTimer >= _pauseDuration)
                {
                    // Time to wander to a new location
                    StartWandering();
                }
            }
            
            // Occasionally change wander intervals for more natural behavior
            _wanderTimer += Time.deltaTime;
            if (_wanderTimer >= _wanderInterval)
            {
                _wanderTimer = 0f;
                _wanderInterval = Random.Range(config.patrolWaitTime * 0.5f, config.patrolWaitTime * 3f);
                
                // Maybe change direction even if we're already wandering
                if (Random.value < 0.3f && !_isWandering)
                {
                    StartWandering();
                }
            }
        }
        
        private void StartWandering()
        {
            Vector3 randomDirection = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                0
            ).normalized;
            
            float randomDistance = Random.Range(_wanderRadius * 0.3f, _wanderRadius);
            Vector3 wanderDestination = _wanderCenter + randomDirection * randomDistance;
            
            // Make sure we don't wander too far from our center
            float distanceFromCenter = Vector3.Distance(wanderDestination, _wanderCenter);
            if (distanceFromCenter > _wanderRadius)
            {
                wanderDestination = _wanderCenter + (wanderDestination - _wanderCenter).normalized * _wanderRadius;
            }
            
            MoveToPosition(wanderDestination);
            _isWandering = true;
        }
        
        public override bool ShouldRemainActiveDespiteGlobalConditions()
        {
            return false;
        }
        
        public override void FixedUpdateLogic()
        {
            // Wandering NPCs might look around while moving
            if (_isWandering && parent.AstarAI != null)
            {
                Vector3 velocity = parent.AstarAI.velocity;
                if (velocity.magnitude > 0.1f)
                {
                    npc.NPCProperties.lastMovementVector = velocity.normalized;
                }
            }
        }
        
        // Public method to set a new wander center (useful for area-based wandering)
        public void SetWanderCenter(Vector3 newCenter, float newRadius = -1f)
        {
            _wanderCenter = newCenter;
            if (newRadius > 0f)
            {
                _wanderRadius = newRadius;
            }
        }
    }
}