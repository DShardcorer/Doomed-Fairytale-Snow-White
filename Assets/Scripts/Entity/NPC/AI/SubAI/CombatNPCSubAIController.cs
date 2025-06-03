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
            
            _lostTargetTimer = 0f;
            _lastTargetPosition = npc.NPCProperties.target.View.transform.position;
            
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
        
        private void HandleLostTarget()
        {
            _lostTargetTimer += Time.deltaTime;
            
            if (_lostTargetTimer < _lostTargetTimeout)
            {
                string currentStateName = parent.GetCurrentState()?.GetType().Name;
                
                if (currentStateName == "NPCIdleState" || currentStateName == "StandardNpcMeleeChaseState")
                {
                    MoveToPosition(_lastTargetPosition);
                }
            }
            else
            {
                // Give up and request return to patrol
                parent.UnsetTarget();
                RequestControllerChange(HelperNPCSubAIControllerName.Patrol, "Lost target for too long");
            }
        }
        
        public override bool ShouldRemainActive()
        {
            // Remain active as long as we have a target or are still searching
            return HasTarget() || _lostTargetTimer < _lostTargetTimeout;
        }
        
        public override void FixedUpdateLogic()
        {
            if (HasTarget())
            {
                FaceTarget();
            }
        }
    }
}