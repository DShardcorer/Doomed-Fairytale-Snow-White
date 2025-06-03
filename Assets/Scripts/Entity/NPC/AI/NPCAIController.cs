using System.Collections.Generic;
using Entity.NPC.BeingInteractedWith;
using Entity.NPC.Idle;
using GeneralManagers;
using Helpers;
using Pathfinding;
using UnityEngine;

namespace Entity.NPC.AI
{
    public abstract class NPCAIController : ILifecycle<NPC>
    {
        // Existing fields...
        protected NPC npc;
        public NPC NPC => npc; // Add public property
        protected NPCAIConfiguration _config;
        protected Seeker seeker;
        public Seeker Seeker => seeker;
        protected IAstarAI astarAI;
        public IAstarAI AstarAI => astarAI;

        protected EntityStateMachine _npcStateStateMachine;
        protected Dictionary<string, NPCState> states = new Dictionary<string, NPCState>();

        protected NPCSubAIStateMachine _npcSubAIStateMachine;

        protected Dictionary<string, NPCSubAIController>
            subAIControllers = new Dictionary<string, NPCSubAIController>();

        // NEW: Queue for controller change requests
        private Queue<ControllerChangeRequest> _changeRequests = new Queue<ControllerChangeRequest>();

        private struct ControllerChangeRequest
        {
            public string controllerId;
            public string reason;
            public float requestTime;
        }

        public NPCAIController(NPCAIConfiguration config)
        {
            _config = config;
            NPCIdleState npcIdleState = new NPCIdleState(config);
            NPCBeingInteractedWithState npcBeingInteractedWithState = new NPCBeingInteractedWithState(config);
            states.Add(HelperNPCStateName.Idle, npcIdleState);
            states.Add(HelperNPCStateName.BeingInteractedWith, npcBeingInteractedWithState);

            _npcSubAIStateMachine = new NPCSubAIStateMachine();
        }

        public virtual void Initialize(NPC npc)
        {
            this.npc = npc;
            _npcStateStateMachine = npc.StateMachine;

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

            _npcStateStateMachine.Initialize(GetInitialState());
            _npcSubAIStateMachine.Initialize(GetInitialNPCSubAIController());
        }

        // Existing state management methods...

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

        // NEW: Method for sub-controllers to request changes
        public void RequestSubAIControllerChange(string controllerId, string reason = "")
        {
            _changeRequests.Enqueue(new ControllerChangeRequest
            {
                controllerId = controllerId,
                reason = reason,
                requestTime = Time.time
            });
        }

        #endregion

        public NPCAIConfiguration GetConfiguration()
        {
            return _config;
        }

        protected abstract void OnTargetSpottedInFOV(object sender, Entity entity);
        protected abstract void OnTargetSpottedInProximity(object sender, Entity e);
        protected abstract NPCState GetInitialState();
        protected abstract NPCSubAIController GetInitialNPCSubAIController();

        public virtual void UpdateLogic()
        {
            // Process controller change requests first
            ProcessControllerChangeRequests();

            // Check for external conditions that override sub-controller decisions
            CheckGlobalConditions();

            _npcSubAIStateMachine.UpdateLogic();
            _npcStateStateMachine.UpdateLogic();
        }

        // NEW: Process queued controller change requests
        private void ProcessControllerChangeRequests()
        {
            while (_changeRequests.Count > 0)
            {
                var request = _changeRequests.Dequeue();

                // Validate the request (you can add more logic here)
                if (subAIControllers.ContainsKey(request.controllerId))
                {
                    Debug.Log(
                        $"Processing controller change request: {request.controllerId} (Reason: {request.reason})");
                    ChangeNPCSubAIController(request.controllerId);
                    break; // Process one request per frame
                }
                else
                {
                    Debug.LogWarning($"Requested controller {request.controllerId} does not exist");
                }
            }
        }

        // NEW: Check global conditions that can override sub-controller behavior
        protected virtual void CheckGlobalConditions()
        {
            var currentController = GetCurrentNPCSubAIController();
            if (currentController == null) return;

            // Check if current controller wants to remain active
            if (!currentController.ShouldRemainActive())
            {
                // Let the controller decide what to do
                return;
            }

            // Global condition: Health-based fleeing
            if (npc.NPCProperties.target != null)
            {
                float healthPercent = (float)npc.HealthSystem.GetHealthPercentage();
                if (healthPercent <= _config.healthFleeThreshold &&
                    currentController.GetType().Name != "FleeNPCSubAIController")
                {
                    ChangeNPCSubAIController("flee");
                    return;
                }
            }

            // Add more global conditions here as needed
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
            _changeRequests.Clear();
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