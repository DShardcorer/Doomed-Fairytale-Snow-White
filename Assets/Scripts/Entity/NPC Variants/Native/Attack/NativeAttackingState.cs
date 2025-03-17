using UnityEngine;

public class NativeAttackingState : NativeState
{
    protected NativeAttackingProperties _nativeAttackingProperties;
    public NativeAttackingState(NativeAttackingProperties nativeAttackingProperties, string animationBoolName) : base(animationBoolName)
    {
        _nativeAttackingProperties = nativeAttackingProperties;
    }

    


}
