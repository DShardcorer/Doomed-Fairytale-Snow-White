using System;
using UnityEngine;

public class HealthSystem : ILifecycle<Entity>
{
    protected Entity _entity;
    public Entity Entity => _entity;
    
    protected float maxHealth;
    public float MaxHealth => maxHealth;

    protected float currentHealth;
    public float CurrentHealth => currentHealth;

    public HealthSystem(float maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }

    public virtual void Initialize(Entity parent)
    {
        _entity = parent;
    }

    // Virtual method for invoking initial events; base implementation does nothing.
    public virtual void InvokeInitialEvents() { }

    public virtual void Dispose()
    {
        _entity = null;
    }

    // Apply damage and trigger the OnHealthChanged hook.
    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
            currentHealth = 0;

        OnHealthChanged();

        if (currentHealth <= 0)
        {
            _entity.Die();
        }
    }

    // Virtual hook for derived classes to override when health changes.
    protected virtual void OnHealthChanged() { }
}
