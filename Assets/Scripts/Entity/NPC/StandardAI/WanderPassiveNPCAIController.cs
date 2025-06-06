using Entity.NPC.AI;
using Entity.NPC.AI.SubControllers;
using Entity.NPC.State.Move;
using Helpers;

namespace Entity.NPC.StandardAI
{
    public class WanderPassiveNPCAIController : NPCAIController
    {
        public WanderPassiveNPCAIController(NPCAIConfiguration config) : base(config)
        {
            var moveState = new NPCMoveState(config);
            states.Add(HelperNPCStateName.Move, moveState);
            
            var wanderController = new WanderNPCSubAIController();
            var fleeController = new FleeNPCSubAIController(HelperNPCSubAIControllerName.Wander);
            
            subAIControllers.Add(HelperNPCSubAIControllerName.Wander, wanderController);
            subAIControllers.Add(HelperNPCSubAIControllerName.Flee, fleeController);
        }
        
        protected override NPCState GetInitialState()
        {
            return GetState(HelperNPCStateName.Idle);
        }
        
        protected override NPCSubAIController GetInitialNPCSubAIController()
        {
            return subAIControllers[HelperNPCSubAIControllerName.Wander];
        }

        protected override string GetInitialStateId()
        {
            return HelperNPCStateName.Idle;
        }

        protected override string GetInitialSubAIControllerId()
        {
            return HelperNPCSubAIControllerName.Wander;
        }

        protected override void OnTargetSpottedInFOV(object sender, Entity entity)
        {
            // Peaceful NPCs flee when they see enemies
            SetTarget(entity);
            ChangeNPCSubAIController(HelperNPCSubAIControllerName.Flee);
        }
        
        protected override void OnTargetSpottedInProximity(object sender, Entity e)
        {
            // Also flee from close enemies
            SetTarget(e);
            ChangeNPCSubAIController(HelperNPCSubAIControllerName.Flee);
        }
    }

}