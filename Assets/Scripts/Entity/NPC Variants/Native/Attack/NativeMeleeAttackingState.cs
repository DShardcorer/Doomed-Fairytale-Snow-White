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
    }
    public override void FixedUpdateState()
    {

        //If target is out of attack range, chase the target
        if (Vector3.Distance(_native.View.transform.position, _properties.target.View.transform.position) > _properties.AttackRange)
        {
            _stateMachine.ChangeState(_native.NativeChasingState);
        }

        if (_properties.target == null)
        {
            _stateMachine.ChangeState(_native.NativeIdlingState);
        }
        base.FixedUpdateState();






    }

    private void Attack()
    {
        Debug.Log("Attacking");
    }
}
