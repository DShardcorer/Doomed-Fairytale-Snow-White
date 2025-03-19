using UnityEngine;

public class NativeState : NPCState
{
    protected Native _native;

    public NativeState(string animationBoolName, EntityStateProperties entityStateProperties) : base(animationBoolName, entityStateProperties)
    {
    }

    public virtual void Initialize(Native controller)
    {
        _native = controller;
        base.Initialize(controller);
    }
}
