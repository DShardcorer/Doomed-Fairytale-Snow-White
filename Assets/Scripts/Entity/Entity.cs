

using System;

public abstract class Entity
{
    protected EntityStateMachine _stateMachine;
    protected EntityView _view;
    protected EntityProperties _properties;
    protected SkillSystem _skillSystem;
    public SkillSystem SkillSystem => _skillSystem;

    public EntityStateMachine StateMachine => _stateMachine;
    public EntityView View => _view;
    public EntityProperties Properties => _properties;
    protected AttackHitbox _attackHitbox;
    public AttackHitbox AttackHitbox => _attackHitbox;
    protected AnimationTriggers _animationTriggers;

    public AnimationTriggers AnimationTriggers => _animationTriggers;


    public Entity(EntityView view, EntityProperties properties, SkillSystem skillSystem, EntityStateMachine stateMachine)
    {
        _view = view;
        _attackHitbox = view.GetComponentInChildren<AttackHitbox>();
        _animationTriggers = view.GetComponentInChildren<AnimationTriggers>();
        _properties = properties;
        _skillSystem = skillSystem;
        _stateMachine = stateMachine;
    }

    public virtual void FixedUpdateLogic()
    {
        _properties.currentPosition = _view.transform.position;
    }
    public virtual void Initialize()
    {
        _attackHitbox.Initialize(this);
        _animationTriggers.Initialize(this);
    }
    public void TakeDamage(float damage)
    {
        _view.PlayDamagedAnimation();
        _properties.CurrentHealth -= damage;
        if (_properties.CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _view.gameObject.SetActive(false);
    }
}
