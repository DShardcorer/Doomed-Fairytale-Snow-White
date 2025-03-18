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

    protected override void OnTakingEffect(object sender, EventArgs e)
    {
        _entity.AttackHitbox.PerformAttack(_entity.Properties.AttackType, _entity.Properties.AttackDamage);
    }

}
