namespace Entity.NPC.AI.SubAI
{
    public class PatrolNPCSubAIController: NPCSubAIController
    {
        public override void Initialize(NPCAIController parent)
        {
            base.Initialize(parent);
            // Additional initialization for patrol behavior
            //GetPatrolProperties from configuration
            
        }

        public override void OnEnter()
        {
            // Logic for entering patrol state
        }

        public override void OnExit()
        {
            // Logic for exiting patrol state
        }

        public override void UpdateLogic()
        {
            // Update logic for patrol behavior
        }

        public override void FixedUpdateLogic()
        {
            // Fixed update logic for patrol behavior
        }
        
    }
}