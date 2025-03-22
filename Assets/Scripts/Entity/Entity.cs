

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
    private LevelSystem _levelSystem;
    public LevelSystem LevelSystem => _levelSystem;
    private HealthSystem _healthSystem;
    public HealthSystem HealthSystem => _healthSystem;
    private ManaSystem _manaSystem;
    public ManaSystem ManaSystem => _manaSystem;
    private StaminaSystem _staminaSystem;
    public StaminaSystem StaminaSystem => _staminaSystem;


    public EntityStateMachine StateMachine => _stateMachine;
    public EntityView View => _view;
    public EntityProperties Properties => _properties;
    protected AttackHitbox _attackHitbox;
    public AttackHitbox AttackHitbox => _attackHitbox;
    protected AnimationTriggers _animationTriggers;

    public AnimationTriggers AnimationTriggers => _animationTriggers;


    public Entity(EntityView view, EntityProperties properties, 
    StatSystem statSystem, SkillSystem skillSystem, LevelSystem levelSystem,
    HealthSystem healthSystem, ManaSystem manaSystem, StaminaSystem staminaSystem,
    EntityStateMachine stateMachine, Inventory inventory)
    {
        _view = view;
        _attackHitbox = view.GetComponentInChildren<AttackHitbox>();
        _animationTriggers = view.GetComponentInChildren<AnimationTriggers>();
        _properties = properties;
        _statSystem = statSystem;
        _skillSystem = skillSystem;
        _levelSystem = levelSystem;
        _healthSystem = healthSystem;
        _manaSystem = manaSystem;
        _staminaSystem = staminaSystem;
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
        _healthSystem.Initialize(this);
        _manaSystem.Initialize(this);
        _staminaSystem.Initialize(this);

    }
    public void TakeDamage(float damage)
    {
        Debug.Log($"Entity {this} took {damage} damage");
        _view.PlayDamagedAnimation();
        _healthSystem.TakeDamage((int)damage);
    }

    public abstract void Die();
}
