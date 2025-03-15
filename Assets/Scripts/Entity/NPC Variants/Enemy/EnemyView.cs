using UnityEngine;

public class EnemyView : NPCView
{

    private Enemy _enemy;

    public override void Initialize(Entity controller)
    {
        base.Initialize(controller);
        _enemy = (Enemy)controller;
        gameObject.SetActive(true);
    }


}
