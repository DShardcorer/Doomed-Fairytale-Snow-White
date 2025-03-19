using System;
using UnityEngine;

public abstract class EntityStateProperties : ILifecycle<EntityState>
{
    private EntityState _parent;
    private CombatStatBoard _combatStatBoard;
    public CombatStatBoard CombatStatBoard => _combatStatBoard;
    private AbilityStatBoard _abilityStatBoard;
    public AbilityStatBoard AbilityStatBoard => _abilityStatBoard;
    public EntityStateProperties()
    {
    }
    public void Initialize(EntityState parent)
    {
        Debug.Log("Initializing EntityStateProperties");
        _parent = parent;
        _combatStatBoard = parent.Entity.StatSystem.CombatStatBoard;
        _abilityStatBoard = parent.Entity.StatSystem.AbilityStatBoard;
        _parent.Entity.StatSystem.OnStatsChanged += UpdateDerivedProperties;
        UpdateDerivedProperties(this, EventArgs.Empty);
    }

    protected abstract void UpdateDerivedProperties(object sender, EventArgs e);

    public void Dispose()
    {
        _combatStatBoard = null;
        _parent = null;
        _parent.Entity.StatSystem.OnStatsChanged -= UpdateDerivedProperties;
    }


}
