using UnityEngine;

public class EnemyState : NPCState
{
    protected Enemy _enemy;
    public EnemyState(string animationBoolName) : base(animationBoolName)
    {
    }

    public virtual void Initialize(Enemy controller)
    {
        _enemy = controller;
        base.Initialize(controller);
    }
}
