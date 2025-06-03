using Entity.NPC.AI;
using Entity.NPC.AI.SubControllers;
using Entity.NPC.StandardAI.Attack;
using Entity.NPC.StandardAI.Chase;
using Entity.NPC.State.Move;
using Helpers;
using UnityEngine;

namespace Entity.NPC.StandardAI
{
    public class EnhancedMeleeNPCAIController : NPCAIController
    {
        public EnhancedMeleeNPCAIController(NPCAIConfiguration config) : base(config)
        {
            // Add states
            NPCMoveState npcMoveState = new NPCMoveState(config);
            StandardNpcMeleeChaseState npcChaseState = new StandardNpcMeleeChaseState(config);
            StandardNpcMeleeAttackState npcAttackState = new StandardNpcMeleeAttackState(config);
            
            states.Add(HelperNPCStateName.Move, npcMoveState);
            states.Add(HelperNPCStateName.Chase, npcChaseState);
            states.Add(HelperNPCStateName.Attack, npcAttackState);
            
            // Add sub AI controllers
            PatrolNPCSubAIController patrolController = new PatrolNPCSubAIController();
            CombatNPCSubAIController combatController = new CombatNPCSubAIController();
            FleeNPCSubAIController fleeController = new FleeNPCSubAIController();
            
            subAIControllers.Add("patrol", patrolController);
            subAIControllers.Add("combat", combatController);
            subAIControllers.Add("flee", fleeController);
        }
        
        protected override NPCState GetInitialState()
        {
            NPCState state = GetState(HelperNPCStateName.Idle);
            if (state == null)
            {
                Debug.LogError("Initial state 'Idle' not found in EnhancedMeleeNPCAIController.");
                return null;
            }
            return state;
        }
        
        protected override NPCSubAIController GetInitialNPCSubAIController()
        {
            return subAIControllers["patrol"];
        }
        
        // ONLY handle external events - no internal state management
        protected override void OnTargetSpottedInFOV(object sender, Entity entity)
        {
            if (npc.IsBusy || !_config.shouldChaseTargets)
                return;
                
            SetTarget(entity);
            
            // Always switch to combat when target spotted
            ChangeNPCSubAIController("combat");
        }
        
        protected override void OnTargetSpottedInProximity(object sender, Entity e)
        {
            if (npc.IsBusy)
                return;
                
            npc.NPCProperties.lastMovementVector =
                (e.View.transform.position - npc.View.transform.position).normalized;
        }
        
        // ONLY handle global conditions
        protected override void CheckGlobalConditions()
        {
            base.CheckGlobalConditions(); // Handles health-based fleeing
            
            // Add any other global conditions specific to melee NPCs here
        }
    }
}