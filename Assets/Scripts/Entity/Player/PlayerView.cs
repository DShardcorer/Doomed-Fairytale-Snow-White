using UnityEngine;

public class PlayerView : EntityView
{
    private new Player _controller;
    public override void Initialize(Entity controller)
    {
        base.Initialize(controller);
        _controller = (Player)controller;
    }

}
