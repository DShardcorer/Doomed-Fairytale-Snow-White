using UnityEngine;

public class NPCMovingState : NPCState
{
    private NPCMovingProperties _npcMovingProperties;
    public NPCMovingState(NPCMovingProperties npcMovingProperties, string animationBoolName) : base(animationBoolName)
    {
        _npcMovingProperties = npcMovingProperties;
    }

    private float _moveTimer = 0;

    public override void EnterState()
    {
        base.EnterState();
        _moveTimer = _npcMovingProperties.MovingTime;
        _npcMovingProperties.lastMovementVector = GetRandomDirection();
        Vector2 roundedDirection = new Vector2(Mathf.Round(_npcMovingProperties.lastMovementVector.x), Mathf.Round(_npcMovingProperties.lastMovementVector.y));
        _npc.EntityDetector.SetColliderRotation(Vector2.SignedAngle(Vector2.down, roundedDirection));
    }

    public override void FixedUpdateState()
    {
        _moveTimer -= Time.fixedDeltaTime;
        _rigidbody.linearVelocity = _npcMovingProperties.lastMovementVector * _npcMovingProperties.MoveSpeed;
        _view.SetAnimationDirection(_npcMovingProperties.lastMovementVector);
        if (_moveTimer <= 0)
        {
            _stateMachine.ChangeState(_npc.NPCIdlingState);
        }
        base.FixedUpdateState();
    }

    public Vector2 GetRandomDirection()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }

}
