using UnityEngine;

public class NativeState : NPCState
{
    protected Native _native;
    public NativeState(string animationBoolName) : base(animationBoolName)
    {
    }

    public virtual void Initialize(Native controller)
    {
        _native = controller;
        base.Initialize(controller);
    }
}
