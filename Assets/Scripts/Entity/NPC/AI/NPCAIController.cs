using System;
using System.Collections.Generic;
using Entity.NPC.Attack;
using Entity.NPC.BeingInteractedWith;
using Entity.NPC.Chase;
using Entity.NPC.Idle;
using Entity.NPC.Move;
using GeneralManagers;

namespace Entity.NPC.AI
{
    public abstract class NPCAIController : ILifecycle<NPC>
    {
        protected NPC npc;
        protected NPCAIConfiguration _config;
        protected EntityStateMachine _stateMachine;

        // Dictionary of states if you want to reuse them
        // Idling state
        protected NPCIdlingState _npcIdlingState;
        public NPCIdlingState NPCIdlingState => _npcIdlingState;

        // Moving state
        protected NPCMovingState _npcMovingState;
        public NPCMovingState NPCMovingState => _npcMovingState;

        // Attacking state
        protected NPCAttackingState _npcAttackingState;
        public NPCAttackingState NPCAttackingState => _npcAttackingState;

        // Chasing state
        protected NPCChasingState _npcChasingState;
        public NPCChasingState NPCChasingState => _npcChasingState;

        // Being Interacted With state
        protected NPCBeingInteractedWithState _npcBeingInteractedWithState;
        public NPCBeingInteractedWithState NPCBeingInteractedWithState => _npcBeingInteractedWithState;

        public NPCAIController(NPCAIConfiguration config)
        {
            _config = config;
            //Override this to create specific states in derived classes constructors
        }

        public virtual void Initialize(NPC npc)
        {
            this.npc = npc;
            _stateMachine = npc.StateMachine;

            // Set up event listeners
            this.npc.FOVDetector.OnClosestEntityFromDifferentFactionSpottedInFOV += OnTargetSpottedInFOV;
            this.npc.ProximityDetector.OnEntityFromDifferentFactionSpottedInProximity += OnTargetSpottedInProximity;


            // Initialize states
            _npcIdlingState.Initialize(this.npc);
            _npcMovingState.Initialize(this.npc);
            _npcChasingState.Initialize(this.npc);
            _npcAttackingState.Initialize(this.npc);
            _npcBeingInteractedWithState.Initialize(this.npc);

            // Set initial state
            _stateMachine.Initialize(GetInitialState());
        }

        public void ChangeState(NPCState newState)
        {
            _stateMachine.ChangeState(newState);
        }


        // Abstract methods that must be implemented by derived classes
        protected abstract void OnTargetSpottedInFOV(object sender, Entity entity);
        protected abstract void OnTargetSpottedInProximity(object sender, Entity e);

        protected abstract NPCState GetInitialState();


        public virtual void UpdateLogic()
        {
            _stateMachine.UpdateLogic();
        }

        public virtual void FixedUpdateLogic()
        {
            npc.FOVDetector.SetColliderRotation(npc.NPCProperties.lastMovementVector);
            npc.AttackHitbox.SetAttackHitBoxRotation(npc.NPCProperties.lastMovementVector);
            _stateMachine.FixedUpdateLogic();
        }

        public virtual void Dispose()
        {
            if (npc != null)
            {
                npc.FOVDetector.OnClosestEntityFromDifferentFactionSpottedInFOV -= OnTargetSpottedInFOV;
                npc.ProximityDetector.OnEntityFromDifferentFactionSpottedInProximity -= OnTargetSpottedInProximity;
            }

            npc = null;
            _config = null;
            _stateMachine = null;
        }

        public void SetTarget(Entity target)
        {
            npc.NPCProperties.target = target;
        }

        public void UnsetTarget()
        {
            npc.NPCProperties.target = null;
        }
    }
}