using GeneralManagers;

namespace Entity.NPC.AI
{
    public abstract class NPCSubAIController: ILifecycle<NPCAIController>
    {
        protected NPCAIController parent;
        private NPCAIConfiguration config;
        
        public virtual void Initialize(NPCAIController parent)
        {
            this.parent = parent;
            config = parent.GetConfiguration();
        }

        public void Dispose()
        {
            // Cleanup if necessary
            parent = null;
            config = null;
        }

        public virtual void OnEnter() {}
        public virtual void OnExit() {}
        public virtual void UpdateLogic() {}
        public virtual void FixedUpdateLogic() {}

    }
}