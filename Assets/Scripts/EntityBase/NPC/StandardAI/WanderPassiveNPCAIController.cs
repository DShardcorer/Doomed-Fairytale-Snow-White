using EntityBase.NPC.AI;
using EntityBase.NPC.AI.SubControllers;
using EntityBase.NPC.State.Move;
using Helpers;

namespace EntityBase.NPC.StandardAI
{
    public class WanderPassiveNPCAIController : NPCAIController
    {
        public WanderPassiveNPCAIController(NPCAIConfiguration config) : base(config)
        {
            var moveState = new NPCMoveState(config);
            states.Add(HelperNPCStateName.Move, moveState);
            
            var wanderController = new WanderNPCSubAIController(HelperNPCSubAIControllerName.Flee);
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
            base.OnTargetSpottedInFOV(sender, entity);
            //Request flee from the target
            RequestSubAIControllerChange(HelperNPCSubAIControllerName.Flee, "Flee from enemy in FOV");
        }
        
    }

}