using UnityEngine;

public class NPCState : EntityState
{
    public NPCState(string animationBoolName) : base(animationBoolName)
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

