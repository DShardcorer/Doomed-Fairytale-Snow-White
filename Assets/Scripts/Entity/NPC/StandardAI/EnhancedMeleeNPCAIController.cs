using Entity.NPC.AI;
using Entity.NPC.AI.SubControllers;
using Entity.NPC.BeingInteractedWith;
using Entity.NPC.Move;
using Entity.NPC.StandardAI.Attack;
using Entity.NPC.StandardAI.Chase;
using Helpers;
using UnityEngine;

namespace Entity.NPC.StandardAI
{
    public class EnhancedMeleeNPCAIController : NPCAIController
    {
        public EnhancedMeleeNPCAIController(NPCAIConfiguration config) : base(config)
        {
            // Add all required states
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
            return GetState(HelperNPCStateName.Idle);
        }
        
        protected override NPCSubAIController GetInitialNPCSubAIController()
        {
            return subAIControllers["patrol"];
        }
        
        public override void FixedUpdateLogic()
        {
            base.FixedUpdateLogic();
            
            // Check for health-based fleeing
            if (npc.NPCProperties.target != null)
            {
                float healthPercent = npc.HealthSystem.GetHealthPercentage();
                if (healthPercent <= _config.healthFleeThreshold && 
                    GetCurrentNPCSubAIController().GetType() != typeof(FleeNPCSubAIController))
                {
                    ChangeNPCSubAIController("flee");
                }
            }
        }
        
        protected override void OnTargetSpottedInFOV(object sender, Entity entity)
        {
            if (npc.IsBusy || !_config.shouldChaseTargets)
                return;
                
            SetTarget(entity);
            
            // Switch to combat mode if not already
            if (GetCurrentNPCSubAIController().GetType() != typeof(CombatNPCSubAIController))
            {
                ChangeNPCSubAIController("combat");
            }
        }
        
        protected override void OnTargetSpottedInProximity(object sender, Entity e)
        {
            if (npc.IsBusy)
                return;
                
            npc.NPCProperties.lastMovementVector =
                (e.View.transform.position - npc.View.transform.position).normalized;
        }
    }
}