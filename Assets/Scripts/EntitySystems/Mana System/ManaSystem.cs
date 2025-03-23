using System;

public class ManaSystem: ILifecycle<Entity>
{
    private Entity _entity;
    public Entity Entity => _entity;
    private float maxMana;
    public float MaxMana => maxMana;

    private float currentMana;
    public float CurrentMana => currentMana;


    public ManaSystem(float maxMana)
    {
        this.maxMana = maxMana;
        currentMana = maxMana;
    }

    public void Initialize(Entity parent)
    {
        _entity = parent;
    }
    public void InvokeInitialEvents()
    {
        PlayerVitalStatsEventSystem.InvokeManaChanged(this, new ManaChangedEventArgs(currentMana, maxMana));
    }

    public void Dispose()
    {
        _entity = null;
    }

    public bool TryUseMana(float mana)
    {
        if (currentMana >= mana)
        {
            UseMana(mana);
            PlayerVitalStatsEventSystem.InvokeManaChanged(this, new ManaChangedEventArgs(currentMana, maxMana));
            return true;
        }
        return false;
    }

    private void UseMana(float mana)
    {
        currentMana -= mana;
        if (currentMana <= 0)
        {
            currentMana = 0;
        }
    }

    public void RestoreMana(float mana)
    {
        currentMana += mana;
        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }
    }
}
