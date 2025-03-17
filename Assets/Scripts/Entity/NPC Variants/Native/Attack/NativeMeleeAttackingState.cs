using System;
using UnityEngine;

public class NativeMeleeAttackingState : NativeAttackingState
{
    public NativeMeleeAttackingState(NativeAttackingProperties nativeAttackingProperties, string animationBoolName) : base(nativeAttackingProperties, animationBoolName)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        _stateTimer = _properties.AttackCooldown;
    }
    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
        //If target is out of attack range, chase the target
        if (Vector3.Distance(_native.View.transform.position, _properties.target.View.transform.position) > _properties.AttackRange)
        {
            _stateMachine.ChangeState(_native.NativeChasingState);
        }

        if (_properties.target == null)
        {
            _stateMachine.ChangeState(_native.NativeIdlingState);
        }

        _stateTimer -= Time.fixedDeltaTime;
        if (_stateTimer <= 0)
        {
            Attack();
            _stateTimer = _properties.AttackCooldown;
        }



    }

    private void Attack()
    {
        Debug.Log("Attacking");
    }
}
