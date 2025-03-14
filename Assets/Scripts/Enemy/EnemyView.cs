using UnityEngine;

public class EnemyView : EntityView
{
    private new Enemy _controller;

    public override void Initialize(Entity controller)
    {
        base.Initialize(controller);
        _controller = (Enemy)controller;
        gameObject.SetActive(true);
    }



}
