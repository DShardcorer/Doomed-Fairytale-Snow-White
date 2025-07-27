using System.Collections.Generic;
using EntityBase.NPC.AI;
using EntityBase.NPC.State.BeingInteractedWith;
using EntityBase.NPC.State.Idle;
using EntityBase.NPC.State.Move;
using GeneralManagers;
using Helpers;
using Pathfinding;
using UnityEngine;

namespace EntityBase.NPC
{
    public class NPCStateSystem : ILifecycle<NPC>
    {
        protected NPC npc;
        protected NPCAIConfiguration config;
        protected Seeker seeker;
        protected IAstarAI astarAI;

        protected EntityStateMachine stateMachine;
        protected Dictionary<string, NPCState> states = new Dictionary<string, NPCState>();

        // Track current state ID
        private string currentStateId = string.Empty;
        
        public string CurrentStateId => currentStateId;

        public NPCStateSystem(NPCAIConfiguration config)
        {
            this.config = config;
            
            // Add basic states all NPCs need
            NPCIdleState npcIdleState = new NPCIdleState(config);
            NPCBeingInteractedWithState npcBeingInteractedWithState = new NPCBeingInteractedWithState(config);
            NPCMoveState npcMoveState = new NPCMoveState(config);
            
            states.Add(HelperNPCStateName.Idle, npcIdleState);
            states.Add(HelperNPCStateName.BeingInteractedWith, npcBeingInteractedWithState);
            states.Add(HelperNPCStateName.Move, npcMoveState);
        }

        public virtual void Initialize(NPC npc)
        {
            this.npc = npc;
            stateMachine = npc.StateMachine;

            if (seeker == null)
                seeker = npc.View.GetComponent<Seeker>();

            if (astarAI == null)
                astarAI = npc.View.GetComponent<IAstarAI>();

            if (seeker == null || astarAI == null)
            {
                Debug.LogError("Missing Seeker or AIPath component on NPC GameObject");
                return;
            }

            // Initialize all states
            foreach (var state in states)
            {
                state.Value.Initialize(npc);
            }

            // Initialize state machine with initial value
            string initialStateId = HelperNPCStateName.Idle;
            currentStateId = initialStateId;
            stateMachine.Initialize(GetState(initialStateId));
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
                stateMachine.ChangeState(state);
                currentStateId = stateId;
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
            return stateMachine.CurrentState as NPCState;
        }

        #endregion

        public NPCAIConfiguration GetConfiguration()
        {
            return config;
        }

        public virtual void UpdateLogic()
        {
            // Handle objective-based state changes
            ProcessObjective();
            
            // Update state machine
            stateMachine.UpdateLogic();
        }
        
        // Translate objectives from behavior tree into state changes
        private void ProcessObjective()
        {
            switch (npc.NPCProperties.CurrentObjective)
            {
                case NPCObjective.Idle:
                    if (currentStateId != HelperNPCStateName.Idle)
                        ChangeState(HelperNPCStateName.Idle);
                    break;
                
                
                case NPCObjective.Move:
                    var moveState = GetState(HelperNPCStateName.Move) as NPCMoveState;
                    if (moveState != null)
                    {
                        moveState.Setup(npc.NPCProperties.ReturnState, npc.NPCProperties.TargetPosition);
                        ChangeState(HelperNPCStateName.Move);
                    }
                    break;
                
                case NPCObjective.Attack:
                    if (currentStateId != HelperNPCStateName.Attack)
                        ChangeState(HelperNPCStateName.Attack);
                    break;
            }
        }

        public virtual void FixedUpdateLogic()
        {
            // Update basic character components
            npc.FOVDetector.SetColliderRotation(npc.NPCProperties.lastMovementVector);
            npc.AttackHitbox.SetAttackHitBoxRotation(npc.NPCProperties.lastMovementVector);
            
            // Update state machine
            stateMachine.FixedUpdateLogic();
        }

        public virtual void Dispose()
        {
            npc = null;
            config = null;
            stateMachine = null;
            states.Clear();
            seeker = null;
            astarAI = null;
        }
    }
}