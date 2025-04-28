using System;
using System.Collections.Generic;
using Entity.NPC.Attack;
using Entity.NPC.BeingInteractedWith;
using Entity.NPC.Chase;
using Entity.NPC.Idle;
using Entity.NPC.Move;
using GeneralManagers;
using Pathfinding;
using UnityEngine;

namespace Entity.NPC.AI
{
    public abstract class NPCAIController : ILifecycle<NPC>
    {
        protected NPC npc;
        protected NPCAIConfiguration _config;
        protected EntityStateMachine _stateMachine;
        protected Seeker seeker; // Reference to the Seeker component
        public Seeker Seeker => seeker;
        protected IAstarAI astarAI; // Reference to AIPath or other IAstarAI implementation
        public IAstarAI AstarAI => astarAI;
        
        
        // Idling state
        protected NPCIdleState _npcIdleState;
        public NPCIdleState NpcIdleState => _npcIdleState;

        // Moving state
        protected NPCMoveState _npcMoveState;
        public NPCMoveState NpcMoveState => _npcMoveState;

        // Attacking state
        protected NPCAttackState _npcAttackState;
        public NPCAttackState NpcAttackState => _npcAttackState;

        // Chasing state
        protected NPCChaseState _npcChaseState;
        public NPCChaseState NpcChaseState => _npcChaseState;

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
            
            if (seeker == null)
                seeker = npc.View.GetComponent<Seeker>();

            if (astarAI == null)
                astarAI = npc.View.GetComponent<IAstarAI>();

            if (seeker == null || astarAI == null)
            {
                Debug.LogError("Missing Seeker or AIPath component on NPC GameObject");
                return;
            }

            // Initialize states
            _npcIdleState.Initialize(this.npc);
            _npcMoveState.Initialize(this.npc);
            _npcChaseState.Initialize(this.npc);
            _npcAttackState.Initialize(this.npc);
            _npcBeingInteractedWithState.Initialize(this.npc);

            // Set initial state
            _stateMachine.Initialize(GetInitialState());
        }

        public void ChangeState(NPCState newState)
        {
            _stateMachine.ChangeState(newState);
        }
        public void ChangeStateIdle()
        {
            _stateMachine.ChangeState(_npcIdleState);
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