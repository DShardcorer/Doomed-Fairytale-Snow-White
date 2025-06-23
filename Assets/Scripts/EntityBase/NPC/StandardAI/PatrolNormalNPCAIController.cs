using EntityBase.NPC.AI;
using EntityBase.NPC.AI.SubAI;
using EntityBase.NPC.AI.SubControllers;
using EntityBase.NPC.StandardAI.Attack;
using EntityBase.NPC.State.Chase;
using EntityBase.NPC.State.Move;
using Helpers;
using UnityEngine;

namespace EntityBase.NPC.StandardAI
{
    public class PatrolNormalNPCAIController : NPCAIController
    {
        public PatrolNormalNPCAIController(NPCAIConfiguration config) : base(config)
        {
            // Add states
            NPCMoveState npcMoveState = new NPCMoveState(config);
            NPCChaseState npcChaseState = new NPCChaseState(config);
            NpcMeleeAttackState npcAttackState = new NpcMeleeAttackState(config);
            
            states.Add(HelperNPCStateName.Move, npcMoveState);
            states.Add(HelperNPCStateName.Chase, npcChaseState);
            states.Add(HelperNPCStateName.Attack, npcAttackState);
            
            // Add sub AI controllers
            PatrolNPCSubAIController patrolController = new PatrolNPCSubAIController();
            CombatNPCSubAIController combatController = new CombatNPCSubAIController();
            FleeNPCSubAIController fleeController = new FleeNPCSubAIController(HelperNPCSubAIControllerName.Patrol);
            
            subAIControllers.Add(HelperNPCSubAIControllerName.Patrol, patrolController);
            subAIControllers.Add(HelperNPCSubAIControllerName.Combat, combatController);
            subAIControllers.Add(HelperNPCSubAIControllerName.Flee, fleeController);
        }
        
        protected override NPCState GetInitialState()
        {
            NPCState state = GetState(HelperNPCStateName.Idle);
            if (state == null)
            {
                Debug.LogError("Initial state 'Idle' not found");
                return null;
            }
            return state;
        }
        
        protected override NPCSubAIController GetInitialNPCSubAIController()
        {
            return subAIControllers[HelperNPCSubAIControllerName.Patrol];
        }
        
        protected override string GetInitialStateId()
        {
            return HelperNPCStateName.Idle;
        }

        protected override string GetInitialSubAIControllerId()
        {
            return HelperNPCSubAIControllerName.Patrol;
        }
        
    }
}