
using System;
using UnityEngine;
public class HealthChangedEventArgs : EventArgs
{
    public float CurrentHealth { get; }
    public float MaxHealth { get; }

    public HealthChangedEventArgs(float currentHealth, float maxHealth)
    {
        CurrentHealth = currentHealth;
        MaxHealth = maxHealth;
    }
}

public class HealthSystem : ILifecycle<Entity>
{
    private Entity _entity;
    public Entity Entity => _entity;
    private float maxHealth;
    public float MaxHealth => maxHealth;

    private float currentHealth;
    public float CurrentHealth => currentHealth;

    public event EventHandler<HealthChangedEventArgs> OnHealthChanged;



    public HealthSystem(float maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }


    public void Initialize(Entity parent)
    {
        _entity = parent;
    }
    public void InvokeInitialEvents()
    {
        OnHealthChanged?.Invoke(this, new HealthChangedEventArgs(currentHealth, maxHealth));
    }

    public void Dispose()
    {
        _entity = null;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            _entity.Die();
        }

    }
}

