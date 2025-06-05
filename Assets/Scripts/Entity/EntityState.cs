using System;
using EntitySystems.Skill;
using UnityEngine;

namespace Entity
{
    public abstract class EntityState
    {
        protected EntityStateProperties _entityStateProperties;
        protected Entity _entity;
        public Entity Entity => _entity;
        protected EntityStateMachine _stateMachine;
        protected ActiveSkillSystem ActiveSkillSystem;
        protected EntityProperties _properties;
        protected EntityView _view;
        protected Rigidbody2D _rigidbody;
        protected Animator _animator;
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
            ActiveSkillSystem = _entity.ActiveSkillSystem;
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
            Debug.LogWarning("Taking effect triggered from animation frame");
        }

        public virtual void UpdateState()
        {
            _view.SetAnimationDirection(_properties.lastMovementVector);
        }
        public virtual void FixedUpdateState()
        {

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
}
