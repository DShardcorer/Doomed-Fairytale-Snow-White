using UnityEngine;

public class NativeChasingState : NativeState
{

    protected NativeChasingProperties _nativeChasingProperties;

    public NativeChasingState(string animationBoolName, NativeChasingProperties entityStateProperties) : base(animationBoolName, entityStateProperties)
    {
        _nativeChasingProperties = entityStateProperties;
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

