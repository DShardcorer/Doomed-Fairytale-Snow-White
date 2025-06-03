using Entity.NPC.AI;
using Entity.NPC.AI.SubControllers;
using Entity.NPC.State.Move;
using Helpers;

namespace Entity.NPC.StandardAI
{
    public class GuardNPCAIController : NPCAIController
    {
        public GuardNPCAIController(NPCAIConfiguration config) : base(config)
        {
            // Add basic states
            var moveState = new NPCMoveState(config);
            states.Add(HelperNPCStateName.Move, moveState);
            
            // Add guard-specific sub controllers
            var guardController = new GuardNPCSubAIController();
            var combatController = new CombatNPCSubAIController();
            
            subAIControllers.Add("guard", guardController);
            subAIControllers.Add("combat", combatController);
        }
        
        protected override NPCState GetInitialState()
        {
            return GetState(HelperNPCStateName.Idle);
        }
        
        protected override NPCSubAIController GetInitialNPCSubAIController()
        {
            return subAIControllers["guard"];
        }
        
        protected override void OnTargetSpottedInFOV(object sender, Entity entity)
        {
            if (npc.IsBusy || !_config.shouldChaseTargets) return;
            
            SetTarget(entity);
            ChangeNPCSubAIController("combat");
        }
        
        protected override void OnTargetSpottedInProximity(object sender, Entity e)
        {
            if (npc.IsBusy) return;
            
            npc.NPCProperties.lastMovementVector =
                (e.View.transform.position - npc.View.transform.position).normalized;
        }
    }


}