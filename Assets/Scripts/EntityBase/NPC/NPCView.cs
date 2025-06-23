namespace EntityBase.NPC
{
    public class NPCView : EntityView
    {
        private NPC _npc;
        public NPC NPC => _npc;

        public void Initialize(NPC npc)
        {
            base.Initialize(npc);
            gameObject.SetActive(true);
        }

    }
}
