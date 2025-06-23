using EntityBase.NPC.State.Move;
using GeneralManagers;
using Helpers;
using UnityEngine;

namespace EntityBase.NPC.AI
{
    public abstract class NPCSubAIController : ILifecycle<NPCAIController>
    {
        protected NPCAIController parent;
        protected NPCAIConfiguration config;
        protected NPC npc;
        
        public NPCAIController Parent => parent;
        public NPCAIConfiguration Config => config;
        public NPC NPC => npc;
        
        public virtual void Initialize(NPCAIController parent)
        {
            this.parent = parent;
            this.config = parent.GetConfiguration();
            this.npc = parent.NPC;
        }

        public virtual void Dispose()
        {
            parent = null;
            config = null;
            npc = null;
        }

        public virtual void OnEnter() 
        {
            Debug.Log($"{GetType().Name} entered");
        }
        
        public virtual void OnExit() 
        {
            Debug.Log($"{GetType().Name} exited");
        }
        
        private bool _isBusy = false;

        public void SetBusy(bool isBusy)
        {
            _isBusy = isBusy;
            Debug.Log($"{GetType().Name} is now busy: {_isBusy}");
        }

        public virtual void UpdateLogic()
        {
            if(_isBusy) return;
        }

        public virtual void FixedUpdateLogic()
        {
            if(_isBusy) return;
        }
        
        // Helper methods for common operations
        protected void ChangeToState(string stateId)
        {
            parent.ChangeState(stateId);
        }
        
        protected void MoveToPosition(Vector3 position, string returnState = null)
        {
            var moveState = parent.GetState(HelperNPCStateName.Move) as NPCMoveState;
            if (moveState != null)
            {
                moveState.Setup(returnState ?? GetIdleStateAfterMove(), position);
                parent.ChangeState(HelperNPCStateName.Move);
            }
        }
        
        protected virtual string GetIdleStateAfterMove()
        {
            return HelperNPCStateName.Idle;
        }
        
        protected float GetDistanceToTarget()
        {
            if (npc.NPCProperties.target == null) return float.MaxValue;
            
            return Vector3.Distance(
                npc.View.transform.position,
                npc.NPCProperties.target.View.transform.position
            );
        }
        
        protected bool HasTarget()
        {
            return parent.HasTarget();
        }
        
        protected bool IsInAttackRange()
        {
            return HasTarget() && GetDistanceToTarget() <= config.attackRange;
        }
        
        protected void FaceTarget()
        {
            if (HasTarget())
            {
                npc.NPCProperties.lastMovementVector =
                    (npc.NPCProperties.target.View.transform.position - npc.View.transform.position).normalized;
            }
        }
        
        protected void RequestControllerChange(string controllerId, string reason = "")
        {
            parent.RequestSubAIControllerChange(controllerId, reason);
        }
        
        //Virtual method to check if this controller should remain active DESPITE global conditions
        public abstract bool ShouldRemainActiveDespiteGlobalConditions();
    }
}