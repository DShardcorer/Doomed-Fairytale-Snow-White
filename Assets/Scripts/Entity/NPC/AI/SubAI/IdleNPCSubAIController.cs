using Helpers;

namespace Entity.NPC.AI.SubControllers
{
    public class IdleNPCSubAIController : NPCSubAIController
    {

        public override void UpdateLogic()
        {
            // Idle NPCs do nothing, but we can check for conditions to change state
            if (HasTarget())
            {
                RequestControllerChange(HelperNPCSubAIControllerName.Flee, "Target spotted, switching to Flee.");
            }
        }
    }
}