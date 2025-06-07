using Entity.NPC.AI;
using Entity.NPC.AI.SubControllers;
using Entity.NPC.State.Move;
using Helpers;
using UnityEngine;

namespace Entity.NPC.StandardAI
{
    public class KeepPositionPassiveNPCAIController : NPCAIController
    {
        public KeepPositionPassiveNPCAIController(NPCAIConfiguration config) : base(config)
        {
            // Add Move state for movement
            var moveState = new NPCMoveState(config);
            states.Add(HelperNPCStateName.Move, moveState);
            
            // Add idle and flee sub-controllers
            var idleController = new KeepPositionSubAIController();
            var fleeController = new FleeNPCSubAIController(HelperNPCSubAIControllerName.KeepPosition);
            
            subAIControllers.Add(HelperNPCSubAIControllerName.KeepPosition, idleController);
            subAIControllers.Add(HelperNPCSubAIControllerName.Flee, fleeController);
        }
        
        protected override NPCState GetInitialState()
        {
            return GetState(HelperNPCStateName.Idle);
        }
        
        protected override NPCSubAIController GetInitialNPCSubAIController()
        {
            return subAIControllers[HelperNPCSubAIControllerName.KeepPosition];
        }

        protected override string GetInitialStateId()
        {
            return HelperNPCStateName.Idle;
        }

        protected override string GetInitialSubAIControllerId()
        {
            return HelperNPCSubAIControllerName.KeepPosition;
        }

        protected override void OnTargetSpottedInFOV(object sender, Entity entity)
        {
            base.OnTargetSpottedInFOV(sender, entity);
            // Request flee from the target
            RequestSubAIControllerChange(HelperNPCSubAIControllerName.Flee, "Flee from enemy in FOV");
        }
    }
}