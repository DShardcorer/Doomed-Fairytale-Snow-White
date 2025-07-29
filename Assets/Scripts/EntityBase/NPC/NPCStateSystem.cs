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

        private string currentStateId = string.Empty;

        public string CurrentStateId => currentStateId;
        public NPCState CurrentState => GetCurrentState();

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
                if (npc != null)
                {
                    state.Initialize(npc);
                }
            }
        }

        public void AddState(string stateId, NPCState state)
        {
            if (!states.ContainsKey(stateId))
            {
                states.Add(stateId, state);
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

        // Direct methods for strategies to use
        public void MoveToPosition(Vector3 position, string returnStateId = null)
        {
            if (states.TryGetValue(HelperNPCStateName.Move, out var stateObj) &&
                stateObj is NPCMoveState moveState)
            {
                moveState.Setup(returnStateId ?? HelperNPCStateName.Idle, position);
                ChangeState(HelperNPCStateName.Move);
            }
        }

        public NPCState GetState(string stateId)
        {
            if (states.TryGetValue(stateId, out var state))
            {
                return state;
            }
            return null;
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
            // Update state machine
            stateMachine.UpdateLogic();
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