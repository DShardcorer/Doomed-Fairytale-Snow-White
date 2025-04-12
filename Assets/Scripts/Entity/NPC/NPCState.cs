namespace Entity.NPC
{
    public class NPCState : EntityState
    {
        public NPCState(string animationBoolName, EntityStateProperties entityStateProperties) : base(animationBoolName, entityStateProperties)
        {
        }

        protected NPC _npc;
        public NPC NPC => _npc;

        public virtual void Initialize(NPC controller)
        {
            _npc = controller;
            base.Initialize(controller);
        }
    }
}

