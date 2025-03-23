using System;
using UnityEngine;

public abstract class EntityState
{
    protected EntityStateProperties _entityStateProperties;
    protected Entity _entity;
    public Entity Entity => _entity;
    protected EntityStateMachine _stateMachine;
    protected SkillSystem _skillSystem;
    protected EntityProperties _properties;
    protected EntityView _view;
    protected Rigidbody2D _rigidbody;
    protected Animator _animator;
    protected float _stateTimer = 1f;
    protected bool _isAnimationEnded;
    protected string _animationBoolName;

    public EntityState(string animationBoolName, EntityStateProperties entityStateProperties)
    {
        _entityStateProperties = entityStateProperties;
        _animationBoolName = animationBoolName;
    }
    public virtual void Initialize(Entity controller)
    {
        _entity = controller;
        _stateMachine = _entity.StateMachine;
        _skillSystem = _entity.SkillSystem;
        _properties = _entity.Properties;
        _view = _entity.View;
        _rigidbody = _entity.View.Rigidbody2D;
        _animator = _entity.View.Animator;
        _entityStateProperties.Initialize(this);
    }
    public virtual void EnterState()
    {
        _entity.AnimationTriggers.OnTakingEffect += OnTakingEffect;
        _isAnimationEnded = false;
        _view.StartStateAnimation(_animationBoolName);
    }

    protected virtual void OnTakingEffect(object sender, EventArgs e){
        Debug.Log("Taking effect");
    }

    public virtual void UpdateState()
    {
    }
    public virtual void FixedUpdateState()
    {
        _view.SetAnimationDirection(_properties.lastMovementVector);
    }

    public virtual void OnAnimationEnd()
    {
        _isAnimationEnded = true;
    }

    public virtual void ExitState()
    {
        _view.StopStateAnimation(_animationBoolName);
        _entity.AnimationTriggers.OnTakingEffect -= OnTakingEffect;
    }
    public bool IsAnimationEnded()
    {
        return _isAnimationEnded;
    }

}
