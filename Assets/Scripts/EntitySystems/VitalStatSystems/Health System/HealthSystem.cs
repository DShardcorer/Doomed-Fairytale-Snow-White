using System;
using UnityEngine;

public class HealthSystem : ILifecycle<Entity>
{
    private Entity _entity;
    public Entity Entity => _entity;
    private float maxHealth;
    public float MaxHealth => maxHealth;

    private float currentHealth;
    public float CurrentHealth => currentHealth;

    public HealthSystem(float maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }

    public void Initialize(Entity parent)
    {
        _entity = parent;
    }

    // Helper method to check if the owning entity is the player.
    private bool IsPlayerEntity() => _entity is Player; // Or use _entity.CompareTag("Player")

    // Only invoke health events if the entity is a player.
    public void InvokeInitialEvents()
    {
        if (IsPlayerEntity())
        {
            PlayerVitalStatsEventSystem.InvokeHealthChanged(this, new HealthChangedEventArgs(currentHealth, maxHealth));
        }
    }

    public void Dispose()
    {
        _entity = null;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
            currentHealth = 0;
        
        if (IsPlayerEntity())
        {
            // Update UI or other player-specific systems.
            PlayerVitalStatsEventSystem.InvokeHealthChanged(this, new HealthChangedEventArgs(currentHealth, maxHealth));
        }
        
        if (currentHealth <= 0)
        {
            _entity.Die();
        }
    }
}
