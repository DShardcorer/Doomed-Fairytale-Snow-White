using UnityEngine;

public class NativeChasingState : NativeState
{

    protected NativeChasingProperties _nativeChasingProperties;
    public NativeChasingState(NativeChasingProperties nativeChasingProperties, string animationBoolName) : base(animationBoolName)
    {
        _nativeChasingProperties = nativeChasingProperties;
    }
    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
        Vector2 roundedDirection = new Vector2(Mathf.Round(_properties.lastMovementVector.x), Mathf.Round(_properties.lastMovementVector.y));
        _npc.FOVDetector.SetColliderRotation(_properties.lastMovementVector);
        _npc.AttackHitbox.SetAttackHitBoxRotation(_properties.lastMovementVector);
        
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

