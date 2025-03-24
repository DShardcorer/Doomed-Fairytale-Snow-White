using System;
using UnityEngine;

public class PlayerStaminaSystem : StaminaSystem
{
    public PlayerStaminaSystem(int maxStamina) : base(maxStamina)
    {
    }
    
    public override void Initialize(Entity parent)
    {
        base.Initialize(parent);
        // Additional player-specific initialization can be performed here if needed.
    }
    
    public override void InvokeInitialEvents()
    {
        PlayerVitalStatsEventSystem.InvokeStaminaChanged(this, 
            new StaminaChangedEventArgs(currentStamina, maxStamina));
    }
    
    // Override the hook to invoke player-specific stamina changed events.
    protected override void OnStaminaChanged()
    {
        PlayerVitalStatsEventSystem.InvokeStaminaChanged(this, 
            new StaminaChangedEventArgs(currentStamina, maxStamina));
    }
}
