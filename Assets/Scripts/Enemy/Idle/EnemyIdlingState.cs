using UnityEngine;

public class EnemyIdlingState : EnemyState
{
    private EnemyIdlingProperties _enemyIdlingProperties;
    public EnemyIdlingState(EnemyIdlingProperties enemyIdlingProperties, string animationBoolName) : base(animationBoolName)
    {
        _enemyIdlingProperties = enemyIdlingProperties;
    }
    private float idleTimer = 0;
    public override void EnterState()
    {
        base.EnterState();
        idleTimer = _enemyIdlingProperties.IdleTime;
    }

    public override void FixedUpdateState()
    {
        idleTimer -= Time.fixedDeltaTime;
        if (idleTimer <= 0)
        {
            _stateMachine.ChangeState(_enemy.EnemyMovingState);
        }
        base.FixedUpdateState();
    }
}
