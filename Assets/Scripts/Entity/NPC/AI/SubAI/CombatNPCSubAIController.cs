using UnityEngine;
using Helpers;

namespace Entity.NPC.AI.SubControllers
{
    public class CombatNPCSubAIController : NPCSubAIController
    {
        private float _lostTargetTimer = 0f;
        private float _lostTargetTimeout = 3f;
        private Vector3 _lastTargetPosition;
        
        public override void OnEnter()
        {
            base.OnEnter();
            _lostTargetTimer = 0f;
            
            if (HasTarget())
            {
                _lastTargetPosition = npc.NPCProperties.target.View.transform.position;
                
                // Immediately start appropriate combat behavior
                if (IsInAttackRange())
                {
                    ChangeToState(HelperNPCStateName.Attack);
                }
                else
                {
                    ChangeToState(HelperNPCStateName.Chase);
                }
            }
        }
        
        public override void UpdateLogic()
        {
            if (!HasTarget())
            {
                HandleLostTarget();
                return;
            }
            
            // Reset lost target timer since we have a target
            _lostTargetTimer = 0f;
            _lastTargetPosition = npc.NPCProperties.target.View.transform.position;
            
            // Update facing direction
            FaceTarget();
            
            // Combat state machine logic
            string currentStateName = parent.GetCurrentState()?.GetType().Name;
            
            switch (currentStateName)
            {
                case "NPCIdleState":
                    // We're idle but have a target, start chasing
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
                    // We're chasing, check if we can attack
                    if (IsInAttackRange())
                    {
                        ChangeToState(HelperNPCStateName.Attack);
                    }
                    break;
                    
                case "StandardNpcMeleeAttackState":
                    // We're attacking, check if target moved away
                    if (!IsInAttackRange())
                    {
                        ChangeToState(HelperNPCStateName.Chase);
                    }
                    break;
            }
        }
        
        private void HandleLostTarget()
        {
            _lostTargetTimer += Time.deltaTime;
            
            if (_lostTargetTimer < _lostTargetTimeout)
            {
                // Search for target at last known position
                string currentStateName = parent.GetCurrentState()?.GetType().Name;
                
                if (currentStateName == "NPCIdleState" || currentStateName == "StandardNpcMeleeChaseState")
                {
                    MoveToPosition(_lastTargetPosition, HelperNPCStateName.Idle);
                }
            }
            else
            {
                // Give up and return to patrol
                parent.UnsetTarget();
                parent.ChangeNPCSubAIController("patrol");
            }
        }
        
        public override void FixedUpdateLogic()
        {
            // Additional combat logic that needs to run in FixedUpdate
            if (HasTarget())
            {
                FaceTarget();
            }
        }
    }
}