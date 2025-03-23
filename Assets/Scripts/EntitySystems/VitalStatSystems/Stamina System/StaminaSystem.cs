using System;
using UnityEngine;

public class StaminaSystem : ILifecycle<Entity>, IUpdatable
{
    private Entity _entity;
    public Entity Entity => _entity;
    private float maxStamina;
    public float MaxStamina => maxStamina;

    private float currentStamina;
    public float CurrentStamina => currentStamina;

    public StaminaSystem(int maxStamina)
    {
        this.maxStamina = maxStamina;
        currentStamina = maxStamina;
    }
    public void Initialize(Entity parent)
    {
        _entity = parent;
        //add to update manager
        GameManager.Instance.UpdateManager.AddUpdatable(this);
    }
    public void InvokeInitialEvents()
    {
        PlayerVitalStatsEventSystem.InvokeStaminaChanged(this, new StaminaChangedEventArgs(currentStamina, maxStamina));
    }

    public void Dispose()
    {
        _entity = null;
        //remove from update manager
        GameManager.Instance.UpdateManager.RemoveUpdatable(this);
    }
    public void UpdateLogic()
    {
        //recover 5% of max stamina every second
        RestoreStamina(maxStamina / 20 * Time.deltaTime);
    }


    public bool TryUseStamina(float stamina)
    {
        if (currentStamina >= stamina)
        {
            UseStamina(stamina);
            PlayerVitalStatsEventSystem.InvokeStaminaChanged(this, new StaminaChangedEventArgs(currentStamina, maxStamina));
            return true;
        }
        return false;
    }
    private void UseStamina(float stamina)
    {
        currentStamina -= stamina;
        if (currentStamina <= 0)
        {
            currentStamina = 0;
        }
    }

    public void RestoreStamina(float stamina)
    {
        if (currentStamina >= maxStamina)
        {
            return;
        }
        currentStamina += stamina;

        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }
        PlayerVitalStatsEventSystem.InvokeStaminaChanged(this, new StaminaChangedEventArgs(currentStamina, maxStamina));
    }


}
