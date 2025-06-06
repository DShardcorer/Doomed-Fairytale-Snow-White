using Entity.NPC.AI;
using Entity.NPC.AI.SubControllers;
using Entity.NPC.State.Move;
using Helpers;

namespace Entity.NPC.StandardAI
{
    public class IdlePassiveNPCAIController: NPCAIController
    {
        public IdlePassiveNPCAIController(NPCAIConfiguration config) : base(config)
        {
            var moveState = new NPCMoveState(config);
            states.Add(HelperNPCStateName.Move, moveState);
            
            var fleeController = new FleeNPCSubAIController(HelperNPCSubAIControllerName.Idle);
            subAIControllers.Add(HelperNPCSubAIControllerName.Flee, fleeController);
        }

        protected override string GetInitialStateId()
        {
            return HelperNPCStateName.Idle;
        }

        protected override string GetInitialSubAIControllerId()
        {
            return HelperNPCSubAIControllerName.Idle;
        }

        protected override void OnTargetSpottedInFOV(object sender, Entity entity)
        {
            
        }

        protected override void OnTargetSpottedInProximity(object sender, Entity e)
        {
           
        }

        protected override NPCState GetInitialState()
        {
            return states[HelperNPCStateName.Idle];
        }

        protected override NPCSubAIController GetInitialNPCSubAIController()
        {
            throw new System.NotImplementedException();
        }
    }
}