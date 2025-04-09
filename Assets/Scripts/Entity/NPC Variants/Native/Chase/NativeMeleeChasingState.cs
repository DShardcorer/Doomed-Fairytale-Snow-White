using UnityEngine;

public class NativeMeleeChasingState : NativeChasingState
{
    private float _attackRange;
    private float _attackCooldown;
    public NativeMeleeChasingState(NativeChasingProperties nativeChasingProperties, string animationBoolName) : base(animationBoolName, nativeChasingProperties)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
        _stateTimer -= Time.fixedDeltaTime;
        if (_properties.target == null)
        {
            _stateMachine.ChangeState(_npc.NPCIdlingState);
        }

        _properties.lastMovementVector = (_properties.target.View.transform.position - _npc.View.transform.position).normalized;
        _rigidbody.linearVelocity = _properties.MoveSpeed * 1.5f * _properties.lastMovementVector;


        // Check if the target is in attack range
        if (Vector3.Distance(_npc.View.transform.position, _properties.target.View.transform.position) <= _nativeChasingProperties.AttackRange)
        {
            if (_stateTimer <= 0)
            {
                _stateTimer = _nativeChasingProperties.AttackCooldown;
                _stateMachine.ChangeState(_npc.NPCAttackingState);
            }
        }

        // Check if the target is out of chase range
        if (Vector3.Distance(_npc.View.transform.position, _properties.target.View.transform.position) > _npc.NPCProperties.ChaseRange)
        {
            _stateMachine.ChangeState(_npc.NPCIdlingState);
        }
    }


}
