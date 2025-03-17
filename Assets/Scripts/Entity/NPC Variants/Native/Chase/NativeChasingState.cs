using UnityEngine;

public class NativeChasingState : NativeState
{
    
    protected NativeChasingProperties _nativeChasingProperties;
    public NativeChasingState(NativeChasingProperties nativeChasingProperties, string animationBoolName) : base(animationBoolName)
    {
        _nativeChasingProperties = nativeChasingProperties;
    }

    public void SetTarget(Entity target)
    {
       _properties.target = target;
    }

    public void UnsetTarget()
    {
        _properties.target = null;
    }







}

