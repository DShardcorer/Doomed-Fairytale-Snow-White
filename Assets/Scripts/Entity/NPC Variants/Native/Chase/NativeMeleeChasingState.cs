using UnityEngine;

public class NativeMeleeChasingState : NativeChasingState
{
    public NativeMeleeChasingState(NativeChasingProperties nativeChasingProperties, string animationBoolName) : base(nativeChasingProperties, animationBoolName)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
        if(_properties.target == null){
            _stateMachine.ChangeState(_native.NativeIdlingState);
        }

        _properties.lastMovementVector = (_properties.target.View.transform.position - _native.View.transform.position).normalized;
        _native.View.transform.position += _properties.MoveSpeed * 1.5f * Time.fixedDeltaTime * new Vector3(_properties.lastMovementVector.x, _properties.lastMovementVector.y, 0f);

        // Check if the target is in attack range
        if(Vector3.Distance(_native.View.transform.position, _properties.target.View.transform.position) <= _properties.AttackRange){
            _stateMachine.ChangeState(_native.NativeAttackingState);
        }

        // Check if the target is out of chase range
        if(Vector3.Distance(_native.View.transform.position, _properties.target.View.transform.position) > _native.NativeProperties.ChaseRange){
            _stateMachine.ChangeState(_native.NativeIdlingState);
        }
        


    }


}
