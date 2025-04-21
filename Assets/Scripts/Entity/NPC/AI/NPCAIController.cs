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
        protected NPC _npc;
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
            _npc = npc;
            _stateMachine = npc.StateMachine;

            // Set up event listeners
            _npc.FOVDetector.OnClosestEntityFromDifferentFactionSpottedInFOV += OnTargetSpottedInFOV;
            _npc.ProximityDetector.OnEntityFromDifferentFactionSpottedInProximity += OnTargetSpottedInProximity;


            // Initialize states
            _npcIdlingState.Initialize(_npc);
            _npcMovingState.Initialize(_npc);
            _npcChasingState.Initialize(_npc);
            _npcAttackingState.Initialize(_npc);
            _npcBeingInteractedWithState.Initialize(_npc);

            // Set initial state
            ChangeState(GetInitialState());
        }

        public void ChangeState(NPCState newState)
        {
            _stateMachine.ChangeState(newState);
        }


        // Abstract methods that must be implemented by derived classes
        protected abstract void OnTargetSpottedInFOV(object sender, Entity e);
        protected abstract void OnTargetSpottedInProximity(object sender, Entity e);

        protected abstract NPCState GetInitialState();


        public void UpdateLogic()
        {
            _stateMachine.UpdateLogic();
        }

        public void FixedUpdateLogic()
        {
            _npc.FOVDetector.SetColliderRotation(_npc.NPCProperties.lastMovementVector);
            _npc.AttackHitbox.SetAttackHitBoxRotation(_npc.NPCProperties.lastMovementVector);
            _stateMachine.FixedUpdateLogic();
        }

        public virtual void Dispose()
        {
            if (_npc != null)
            {
                _npc.FOVDetector.OnClosestEntityFromDifferentFactionSpottedInFOV -= OnTargetSpottedInFOV;
                _npc.ProximityDetector.OnEntityFromDifferentFactionSpottedInProximity -= OnTargetSpottedInProximity;
            }

            _npc = null;
            _config = null;
            _stateMachine = null;
        }
    }
}