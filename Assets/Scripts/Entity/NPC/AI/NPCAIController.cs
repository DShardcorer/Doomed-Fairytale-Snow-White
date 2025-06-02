using System;
using System.Collections.Generic;
using Entity.NPC.AI.SubAI;
using Entity.NPC.Attack;
using Entity.NPC.BeingInteractedWith;
using Entity.NPC.Chase;
using Entity.NPC.Idle;
using Entity.NPC.Move;
using GeneralManagers;
using Helpers;
using Pathfinding;
using UnityEngine;

namespace Entity.NPC.AI
{
    public abstract class NPCAIController : ILifecycle<NPC>
    {
        protected NPC npc;
        public NPC NPC => npc; // Property to access the NPC instance
        protected NPCAIConfiguration _config;

        protected Seeker seeker; // Reference to the Seeker component
        public Seeker Seeker => seeker;
        protected IAstarAI astarAI; // Reference to AIPath or other IAstarAI implementation
        public IAstarAI AstarAI => astarAI;
        
        protected EntityStateMachine _npcStateStateMachine;
        protected Dictionary<string, NPCState> states = new Dictionary<string, NPCState>();

        protected NPCSubAIStateMachine _npcSubAIStateMachine;
        protected Dictionary<string, NPCSubAIController> subAIControllers = new Dictionary<string, NPCSubAIController>();

        public NPCAIController(NPCAIConfiguration config)
        {            
            //Override this to create specific states in derived classes constructors
            _config = config;
            NPCIdleState npcIdleState = new NPCIdleState(config);
            NPCBeingInteractedWithState npcBeingInteractedWithState = new NPCBeingInteractedWithState(config);
            states.Add(HelperNPCStateName.Idle, npcIdleState);
            states.Add(HelperNPCStateName.BeingInteractedWith, npcBeingInteractedWithState);
            
            // Create sub AI state machine
            _npcSubAIStateMachine = new NPCSubAIStateMachine();
        }

        public virtual void Initialize(NPC npc)
        {
            this.npc = npc;
            _npcStateStateMachine = npc.StateMachine;

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

            foreach (var subAIController in subAIControllers.Values)
            {
                subAIController.Initialize(this);
            }

            foreach (var state in states)
            {
                state.Value.Initialize(npc);
            }

            // Set initial state
            _npcSubAIStateMachine.Initialize(GetInitialNPCSubAIController());
            _npcStateStateMachine.Initialize(GetInitialState());
            
        }

        #region State Management

        public void AddStateAndInitialize(string stateId, NPCState state)
        {
            if (!states.ContainsKey(stateId))
            {
                states.Add(stateId, state);
                state.Initialize(npc);
            }
            else
            {
                Debug.LogWarning($"State with ID {stateId} already exists.");
            }
        }

        public void AddState(string stateId, NPCState state)
        {
            if (!states.ContainsKey(stateId))
            {
                states.Add(stateId, state);
            }
            else
            {
                Debug.LogWarning($"State with ID {stateId} already exists.");
            }
        }

        public void ChangeState(string stateId)
        {
            if (states.TryGetValue(stateId, out var state))
            {
                _npcStateStateMachine.ChangeState(state);
            }
            else
            {
                Debug.LogWarning($"State with ID {stateId} does not exist.");
            }
        }

        public NPCState GetState(string stateId)
        {
            if (states.TryGetValue(stateId, out var state))
            {
                return state;
            }
            else
            {
                Debug.LogWarning($"State with ID {stateId} does not exist.");
                return null;
            }
        }
        public NPCState GetCurrentState()
        {
            return _npcStateStateMachine.CurrentState as NPCState;
        }

        #endregion

        #region NPCSubAI Management

        public void AddNPCSubAIControllerAndInitialize(string controllerId, NPCSubAIController controller)
        {
            if (!subAIControllers.ContainsKey(controllerId))
            {
                subAIControllers.Add(controllerId, controller);
                controller.Initialize(this);
            }
            else
            {
                Debug.LogWarning($"Sub AI Controller with ID {controllerId} already exists.");
            }
        }
        
        public void AddNPCSubAIController(string controllerId, NPCSubAIController controller)
        {
            if (!subAIControllers.ContainsKey(controllerId))
            {
                subAIControllers.Add(controllerId, controller);
            }
            else
            {
                Debug.LogWarning($"Sub AI Controller with ID {controllerId} already exists.");
            }
        }
        
        public void ChangeNPCSubAIController(string controllerId)
        {
            if (subAIControllers.TryGetValue(controllerId, out var controller))
            {
                _npcSubAIStateMachine.ChangeNPCSubAIController(controller);
            }
            else
            {
                Debug.LogWarning($"Sub AI Controller with ID {controllerId} does not exist.");
            }
        }
        
        public NPCSubAIController GetInitialNPCSubAIController(string controllerId)
        {
            if (subAIControllers.TryGetValue(controllerId, out var controller))
            {
                return controller;
            }
            else
            {
                Debug.LogWarning($"Sub AI Controller with ID {controllerId} does not exist.");
                return null;
            }
        }
        public NPCSubAIController GetCurrentNPCSubAIController()
        {
            return _npcSubAIStateMachine.CurrentNpcSubAIController;
        }
        

        #endregion
        
        
        public NPCAIConfiguration GetConfiguration()
        {
            return _config;
        }


        // Abstract methods that must be implemented by derived classes
        protected abstract void OnTargetSpottedInFOV(object sender, Entity entity);
        protected abstract void OnTargetSpottedInProximity(object sender, Entity e);

        protected abstract NPCState GetInitialState();
        protected abstract NPCSubAIController GetInitialNPCSubAIController();


        public virtual void UpdateLogic()
        {
            _npcSubAIStateMachine.UpdateLogic();
            _npcStateStateMachine.UpdateLogic();
        }

        public virtual void FixedUpdateLogic()
        {
            npc.FOVDetector.SetColliderRotation(npc.NPCProperties.lastMovementVector);
            npc.AttackHitbox.SetAttackHitBoxRotation(npc.NPCProperties.lastMovementVector);
            _npcSubAIStateMachine.FixedUpdateLogic();
            _npcStateStateMachine.FixedUpdateLogic();
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
            _npcStateStateMachine = null;
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