using System;
using UnityEngine;
public class HealthChangedEventArgs : EventArgs
{
    public float LastCurrentHealth { get; }
    public float CurrentHealth { get; }
    public float MaxHealth { get; }

    public HealthChangedEventArgs(float lastCurrentHealth, float currentHealth, float maxHealth)
    {
        LastCurrentHealth = lastCurrentHealth;
        CurrentHealth = currentHealth;
        MaxHealth = maxHealth;
    }
}

public class ManaChangedEventArgs : EventArgs
{
    public float CurrentMana { get; }
    public float MaxMana { get; }

    public ManaChangedEventArgs(float currentMana, float maxMana)
    {
        CurrentMana = currentMana;
        MaxMana = maxMana;
    }
}

public class StaminaChangedEventArgs : EventArgs
{
    public float CurrentStamina { get; }
    public float MaxStamina { get; }

    public StaminaChangedEventArgs(float currentStamina, float maxStamina)
    {
        CurrentStamina = currentStamina;
        MaxStamina = maxStamina;
    }
}
public static class PlayerVitalStatsEventSystem
{
    public static event EventHandler<HealthChangedEventArgs> OnHealthChanged;
    public static event EventHandler<ManaChangedEventArgs> OnManaChanged;
    public static event EventHandler<StaminaChangedEventArgs> OnStaminaChanged;

    public static void InvokeHealthChanged(object sender, HealthChangedEventArgs e)
    {
        OnHealthChanged?.Invoke(sender, e);
    }

    public static void InvokeManaChanged(object sender, ManaChangedEventArgs e)
    {
        OnManaChanged?.Invoke(sender, e);
    }

    public static void InvokeStaminaChanged(object sender, StaminaChangedEventArgs e)
    {
        OnStaminaChanged?.Invoke(sender, e);
    }
}
