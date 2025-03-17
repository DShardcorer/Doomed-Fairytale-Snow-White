using UnityEngine;

public class AnimationTriggers : MonoBehaviour, ILifecycle<Entity>
{
    private Entity _entity;
    private AttackHitbox _attackHitbox;
    private EntityStateMachine _stateMachine;

    public void Dispose()
    {
        _entity = null;
    }

    public void Initialize(Entity parent)
    {
        _entity = parent;
        _attackHitbox = _entity.AttackHitbox;
        _stateMachine = _entity.StateMachine;
        //if null, throw error
        if (_attackHitbox == null)
        {
            Debug.LogError("AttackHitbox is not assigned!");
        }
        if (_stateMachine == null)
        {
            Debug.LogError("StateMachine is not assigned!");
        }

    }

    public void OnAnimationEnd()
    {
        _stateMachine.OnAnimationEnd();
    }

    public void PerformAttack()
    {
        _attackHitbox.PerformAttack(_entity.Properties.AttackType);
    }







}
