using UnityEngine;

public class NativeAttackingState : NativeState
{
    protected NativeAttackingProperties _nativeAttackingProperties;
    public NativeAttackingState(NativeAttackingProperties nativeAttackingProperties, string animationBoolName) : base(animationBoolName)
    {
        _nativeAttackingProperties = nativeAttackingProperties;
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
        if (!_isAnimationEnded)
        {
            _rigidbody.linearVelocity = 0.5f * _native.NativeProperties.lastMovementVector * _native.NativeProperties.MoveSpeed;
        }
        else
        {
            _stateMachine.ChangeState(_native.NativeChasingState);
        }
    }


}
