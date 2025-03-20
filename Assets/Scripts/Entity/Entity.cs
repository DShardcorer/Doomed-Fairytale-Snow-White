

using System;
using UnityEngine;


public abstract class Entity
{
    protected EntityStateMachine _stateMachine;
    protected EntityView _view;
    protected EntityProperties _properties;
    protected SkillSystem _skillSystem;
    public SkillSystem SkillSystem => _skillSystem;
    protected StatSystem _statSystem;
    public StatSystem StatSystem => _statSystem;
    protected Inventory _inventory;
    public Inventory Inventory => _inventory;


    public EntityStateMachine StateMachine => _stateMachine;
    public EntityView View => _view;
    public EntityProperties Properties => _properties;
    protected AttackHitbox _attackHitbox;
    public AttackHitbox AttackHitbox => _attackHitbox;
    protected AnimationTriggers _animationTriggers;

    public AnimationTriggers AnimationTriggers => _animationTriggers;


    public Entity(EntityView view, EntityProperties properties, StatSystem statSystem, SkillSystem skillSystem, EntityStateMachine stateMachine, Inventory inventory)
    {
        _view = view;
        _attackHitbox = view.GetComponentInChildren<AttackHitbox>();
        _animationTriggers = view.GetComponentInChildren<AnimationTriggers>();
        _properties = properties;
        _statSystem = statSystem;
        _skillSystem = skillSystem;
        _stateMachine = stateMachine;
        _inventory = inventory;
    }


    public virtual void FixedUpdateLogic()
    {
        _properties.currentPosition = _view.transform.position;
    }
    public virtual void Initialize()
    {
        _inventory.Initialize(this);
        _attackHitbox.Initialize(this);
        _animationTriggers.Initialize(this);
        _statSystem.Initialize(this);
        //debuglog inventory
        Debug.Log($"Entity {this} initialized with inventory {_inventory}");

    }
    public void TakeDamage(float damage)
    {
        Debug.Log($"Entity {this} took {damage} damage");
        _view.PlayDamagedAnimation();
        _properties.currentHealth -= damage;
        if (_properties.currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _view.gameObject.SetActive(false);
    }
}
