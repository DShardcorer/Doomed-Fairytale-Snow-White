public class NPCView : EntityView
{
    public NPC NPC => _controller as NPC;

    public override void Initialize(Entity controller)
    {
        base.Initialize(controller);
        gameObject.SetActive(true);
    }
}
