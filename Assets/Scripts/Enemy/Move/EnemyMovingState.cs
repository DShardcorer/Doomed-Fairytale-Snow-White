using UnityEngine;

public class EnemyMovingState : EnemyState
{
    private EnemyMovingProperties _enemyMovingProperties;
    public EnemyMovingState(EnemyMovingProperties enemyMovingProperties, string animationBoolName) : base(animationBoolName)
    {
        _enemyMovingProperties = enemyMovingProperties;
    }

    private float _moveTimer = 0;


    public override void EnterState()
    {
        base.EnterState();
        _moveTimer = _enemyMovingProperties.MovingTime;
        _enemyMovingProperties.lastMovementVector = GetRandomDirection();
        _enemy.EntityDetector.SetColliderRotation(Vector2.SignedAngle(Vector2.down, _enemyMovingProperties.lastMovementVector));
    }

    public override void FixedUpdateState()
    {
        _moveTimer -= Time.fixedDeltaTime;
        _rigidbody.linearVelocity = _enemyMovingProperties.lastMovementVector * _enemyMovingProperties.MoveSpeed;
        _view.SetAnimationDirection(_enemyMovingProperties.lastMovementVector);
        if (_moveTimer <= 0)
        {
            _stateMachine.ChangeState(_enemy.EnemyIdlingState);
        }
        base.FixedUpdateState();
    }

    public Vector2 GetRandomDirection()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }






}
