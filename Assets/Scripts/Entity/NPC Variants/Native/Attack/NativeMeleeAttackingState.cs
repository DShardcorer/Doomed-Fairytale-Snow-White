using System;
using UnityEngine;

public class NativeMeleeAttackingState : NativeAttackingState
{
    public NativeMeleeAttackingState(string animationBoolName, NativeAttackingProperties entityStateProperties) : base(animationBoolName, entityStateProperties)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
    }
    public override void FixedUpdateState()
    {

        //If target is out of attack range, chase the target
        if (Vector3.Distance(_npc.View.transform.position, _properties.target.View.transform.position) >_nativeAttackingProperties.AttackRange)
        {
            _stateMachine.ChangeState(_npc.NPCChasingState);
        }

        if (_properties.target == null)
        {
            _stateMachine.ChangeState(_npc.NPCIdlingState);
        }
        base.FixedUpdateState();
    }

    protected override void OnTakingEffect(object sender, EventArgs e)
    {
        _entity.AttackHitbox.PerformAttack( _nativeAttackingProperties.AttackType, _nativeAttackingProperties.AttackDamage);
    }

}
