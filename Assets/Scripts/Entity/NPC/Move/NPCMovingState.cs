using UnityEngine;

public class NPCMovingState : NPCState
{
    private NPCMovingProperties _npcMovingProperties;
    public NPCMovingState(NPCMovingProperties npcMovingProperties, string animationBoolName) : base(animationBoolName, npcMovingProperties)
    {
        _npcMovingProperties = npcMovingProperties;
    }


    public override void EnterState()
    {
        base.EnterState();
        _stateTimer = _npcMovingProperties.MovingTime;
        _properties.lastMovementVector = GetRandomDirection();

    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
        _stateTimer -= Time.fixedDeltaTime;
        _npc.FOVDetector.SetColliderRotation(_properties.lastMovementVector);
        _npc.AttackHitbox.SetAttackHitBoxRotation(_properties.lastMovementVector);
        _rigidbody.linearVelocity = _properties.lastMovementVector * _npcMovingProperties.MoveSpeed;
        if (_stateTimer <= 0)
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
