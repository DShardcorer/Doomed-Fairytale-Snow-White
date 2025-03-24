using System;
using UnityEngine;

public class PlayerHealthSystem : HealthSystem
{
    public PlayerHealthSystem(float maxHealth) : base(maxHealth)
    {
    }

    public override void Initialize(Entity parent)
    {
        base.Initialize(parent);
        // Optionally, subscribe to additional player-specific events here if needed.
    }

    public override void InvokeInitialEvents()
    {
        PlayerVitalStatsEventSystem.InvokeHealthChanged(this, new HealthChangedEventArgs(currentHealth, maxHealth));
    }

    // Override the hook to invoke player-specific health changed events.
    protected override void OnHealthChanged()
    {
        PlayerVitalStatsEventSystem.InvokeHealthChanged(this, new HealthChangedEventArgs(currentHealth, maxHealth));
    }
}
