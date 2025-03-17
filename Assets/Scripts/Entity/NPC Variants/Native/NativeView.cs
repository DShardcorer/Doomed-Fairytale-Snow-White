using UnityEngine;

public class NativeView : NPCView
{

    private Native _native;

    public override void Initialize(Entity controller)
    {
        base.Initialize(controller);
        _native = (Native)controller;
        gameObject.SetActive(true);
    }


}
