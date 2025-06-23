using EntityBase.NPC.AI;
using EntityBase.NPC.AI.SubAI;
using EntityBase.NPC.AI.SubControllers;
using EntityBase.NPC.State.Move;
using Helpers;

namespace EntityBase.NPC.StandardAI
{
    public class GuardAgressiveNPCAIController : NPCAIController
    {
        public GuardAgressiveNPCAIController(NPCAIConfiguration config) : base(config)
        {
            // Add basic states
            var moveState = new NPCMoveState(config);
            states.Add(HelperNPCStateName.Move, moveState);
            
            // Add guard-specific sub controllers
            var guardController = new GuardNPCSubAIController();
            var combatController = new CombatNPCSubAIController();
            
            subAIControllers.Add(HelperNPCSubAIControllerName.Guard, guardController);
            subAIControllers.Add(HelperNPCSubAIControllerName.Combat, combatController);
        }
        
        protected override NPCState GetInitialState()
        {
            return GetState(HelperNPCStateName.Idle);
        }
        
        protected override NPCSubAIController GetInitialNPCSubAIController()
        {
            return subAIControllers[HelperNPCSubAIControllerName.Guard];
        }

        protected override string GetInitialStateId()
        {
            return HelperNPCStateName.Idle;
        }

        protected override string GetInitialSubAIControllerId()
        {
            return HelperNPCSubAIControllerName.Guard;
        }
        
        

    }


}