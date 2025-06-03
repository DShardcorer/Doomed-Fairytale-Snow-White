using Entity.NPC.AI;
using Entity.NPC.AI.SubControllers;
using Entity.NPC.State.Move;
using Helpers;

namespace Entity.NPC.StandardAI
{
    public class PeacefulNPCAIController : NPCAIController
    {
        public PeacefulNPCAIController(NPCAIConfiguration config) : base(config)
        {
            var moveState = new NPCMoveState(config);
            states.Add(HelperNPCStateName.Move, moveState);
            
            var wanderController = new WanderNPCSubAIController();
            var fleeController = new FleeNPCSubAIController();
            
            subAIControllers.Add("wander", wanderController);
            subAIControllers.Add("flee", fleeController);
        }
        
        protected override NPCState GetInitialState()
        {
            return GetState(HelperNPCStateName.Idle);
        }
        
        protected override NPCSubAIController GetInitialNPCSubAIController()
        {
            return subAIControllers["wander"];
        }
        
        protected override void OnTargetSpottedInFOV(object sender, Entity entity)
        {
            // Peaceful NPCs flee when they see enemies
            SetTarget(entity);
            ChangeNPCSubAIController("flee");
        }
        
        protected override void OnTargetSpottedInProximity(object sender, Entity e)
        {
            // Also flee from close enemies
            SetTarget(e);
            ChangeNPCSubAIController("flee");
        }
    }

}