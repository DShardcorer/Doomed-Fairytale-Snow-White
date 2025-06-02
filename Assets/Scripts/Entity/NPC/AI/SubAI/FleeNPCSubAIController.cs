using UnityEngine;
using Helpers;

namespace Entity.NPC.AI.SubControllers
{
    public class FleeNPCSubAIController : NPCSubAIController
    {
        private float _fleeTimer = 0f;
        private float _fleeTimeout = 5f;
        private float _fleeDistance = 10f;
        private bool _hasSetFleeDestination = false;
        
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
            SetFleeDestination();
        }
        
        private void SetFleeDestination()
        {
            Vector3 fleeDirection;
            
            if (HasTarget())
            {
                // Flee away from target
                fleeDirection = (npc.View.transform.position - npc.NPCProperties.target.View.transform.position).normalized;
            }
            else
            {
                // Flee in random direction
                fleeDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
            }
            
            Vector3 fleePosition = npc.View.transform.position + fleeDirection * _fleeDistance;
            MoveToPosition(fleePosition);
            _hasSetFleeDestination = true;
        }
        
        public override void UpdateLogic()
        {
            _fleeTimer += Time.deltaTime;
            
            // If we've been fleeing long enough, return to patrol
            if (_fleeTimer >= _fleeTimeout)
            {
                parent.UnsetTarget();
                parent.ChangeNPCSubAIController("patrol");
                return;
            }
            
            // If we've reached our flee destination and still need to flee, set a new one
            string currentStateName = parent.GetCurrentState()?.GetType().Name;
            if (currentStateName == "NPCIdleState" && _hasSetFleeDestination)
            {
                if (_fleeTimer < _fleeTimeout * 0.7f) // Still need to flee
                {
                    _hasSetFleeDestination = false;
                    SetFleeDestination();
                }
            }
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
    }
}