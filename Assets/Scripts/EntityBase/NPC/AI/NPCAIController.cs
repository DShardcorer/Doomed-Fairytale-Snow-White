using System.Collections.Generic;
using EntityBase.NPC.State;
using EntityBase.NPC.State.BeingInteractedWith;
using EntityBase.NPC.State.Idle;
using GeneralManagers;
using Helpers;
using Pathfinding;
using UnityEngine;

namespace EntityBase.NPC.AI
{
    public abstract class NPCAIController : ILifecycle<NPC>
    {
        protected NPC npc;
        public NPC NPC => npc;
        protected NPCAIConfiguration _config;
        protected Seeker seeker;
        public Seeker Seeker => seeker;
        protected IAstarAI astarAI;
        public IAstarAI AstarAI => astarAI;

        protected EntityStateMachine _npcStateStateMachine;
        protected Dictionary<string, NPCState> states = new Dictionary<string, NPCState>();

        // Track current state and subcontroller IDs
        private string _currentStateId = string.Empty;
        private string _currentSubControllerId = string.Empty;

        // Public properties to access current IDs
        public string CurrentStateId => _currentStateId;
        public string CurrentSubControllerId => _currentSubControllerId;

        protected NPCSubAIStateMachine _npcSubAIStateMachine;

        protected Dictionary<string, NPCSubAIController>
            subAIControllers = new Dictionary<string, NPCSubAIController>();

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

            this.npc.FOVDetector.OnClosestEntityFromEnemyFactionSpottedInFOV += OnTargetSpottedInFOV;
            this.npc.ProximityDetector.OnEntityFromEnemyFactionSpottedInProximity += OnTargetSpottedInProximity;

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

            // Initialize state machines with initial values
            string initialStateId = GetInitialStateId();
            string initialSubControllerId = GetInitialSubAIControllerId();

            _currentStateId = initialStateId;
            _currentSubControllerId = initialSubControllerId;

            _npcStateStateMachine.Initialize(GetState(initialStateId));
            _npcSubAIStateMachine.Initialize(GetNPCSubAIController(initialSubControllerId));
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
                _currentStateId = stateId; // Update the current state ID directly
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

        // Get the current state ID directly
        public string GetCurrentStateId()
        {
            return _currentStateId;
        }

        // Method to get initial state ID
        protected abstract string GetInitialStateId();

        #endregion

        #region NPCSubAI Management

        public void SetCurrentSubControllerBusy(bool isBusy)
        {
            var currentController = GetCurrentNPCSubAIController();
            if (currentController != null)
            {
                currentController.SetBusy(isBusy);
            }
            else
            {
                Debug.LogWarning("No current NPC Sub AI Controller to set busy state.");
            }
        }

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

        private void ChangeNPCSubAIController(string controllerId)
        {
            if (subAIControllers.TryGetValue(controllerId, out var controller))
            {
                _npcSubAIStateMachine.ChangeNPCSubAIController(controller);
                _currentSubControllerId = controllerId; // Update the current controller ID directly
            }
            else
            {
                Debug.LogWarning($"Sub AI Controller with ID {controllerId} does not exist.");
            }
        }


        public NPCSubAIController GetCurrentNPCSubAIController()
        {
            return _npcSubAIStateMachine.CurrentNpcSubAIController;
        }

        public NPCSubAIController GetNPCSubAIController(string controllerId)
        {
            return subAIControllers.TryGetValue(controllerId, out var controller) ? controller : null;
        }

        public bool HasNPCSubAIController(string controllerId)
        {
            return subAIControllers.ContainsKey(controllerId);
        }

        // Get the current subcontroller ID directly
        public string GetCurrentSubControllerId()
        {
            return _currentSubControllerId;
        }

        // Method to get initial subcontroller ID
        protected abstract string GetInitialSubAIControllerId();

        public string InitialSubAIControllerId()
        {
            return GetInitialSubAIControllerId();
        }

        // Method for sub-controllers to request changes
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

        protected virtual void OnTargetSpottedInFOV(object sender, Entity entity)
        {
            SetTarget(entity);
        }

        protected virtual void OnTargetSpottedInProximity(object sender, Entity e)
        {
            if (npc.IsBusy)
                return;
            if (HasTarget())
            {
                return;
            }

            npc.NPCProperties.lastMovementVector =
                (e.View.transform.position - npc.View.transform.position).normalized;
        }

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

        // Process queued controller change requests
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

        // Check global conditions that can override sub-controller behavior
        protected virtual void CheckGlobalConditions()
        {
            var currentController = GetCurrentNPCSubAIController();
            if (currentController == null) return;

            // Check if current controller wants to remain active
            if (currentController.ShouldRemainActiveDespiteGlobalConditions())
            {
                // Let the controller decide what to do
                return;
            }

            // Global condition: Health-based fleeing
            if (HasTarget())
            {
                float healthPercent = npc.HealthSystem.GetHealthPercentage();
                //print out current health percent and stateid
                Debug.Log(
                    $"Current Health Percent: {healthPercent}, Current subcontroller ID: {_currentSubControllerId}");
                if (healthPercent <= _config.healthFleeThreshold &&
                    _currentSubControllerId != HelperNPCSubAIControllerName.Flee
                    && HasNPCSubAIController(HelperNPCSubAIControllerName.Flee))
                {
                    ChangeNPCSubAIController(HelperNPCSubAIControllerName.Flee);
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
                npc.FOVDetector.OnClosestEntityFromEnemyFactionSpottedInFOV -= OnTargetSpottedInFOV;
                npc.ProximityDetector.OnEntityFromEnemyFactionSpottedInProximity -= OnTargetSpottedInProximity;
            }

            npc = null;
            _config = null;
            _npcStateStateMachine = null;
            _changeRequests.Clear();
            states.Clear();
            subAIControllers.Clear();
            seeker = null;
            astarAI = null;
            _npcSubAIStateMachine.Dispose();
            _npcSubAIStateMachine = null;
        }

        public void SetTarget(Entity target)
        {
            npc.NPCProperties.target = target;
        }

        public void UnsetTarget()
        {
            npc.NPCProperties.target = null;
        }

        public bool HasTarget()
        {
            return npc.NPCProperties.target != null;
        }
    }
}