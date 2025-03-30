using System;
using UnityEngine;

public abstract class EntityStateProperties : ILifecycle<EntityState>
{
    protected EntityState _parent;
    private CombatStatBoard _combatStatBoard;
    public CombatStatBoard CombatStatBoard => _combatStatBoard;
    private AbilityStatBoard _abilityStatBoard;
    public AbilityStatBoard AbilityStatBoard => _abilityStatBoard;
    public EntityStateProperties()
    {
    }
    public void Initialize(EntityState parent)
    {
        _parent = parent;
        _combatStatBoard = parent.Entity.StatSystem.CombatStatBoard;
        _abilityStatBoard = parent.Entity.StatSystem.AbilityStatBoard;
        EntityStatsEventSystem.CombatStatsChanged += UpdateDerivedProperties;
        UpdateDerivedProperties(_parent.Entity, _combatStatBoard);
    }

    protected void UpdateDerivedProperties(Entity sender, CombatStatBoard e){
        if(sender != _parent.Entity)
            return;
        UpdateDerivedProperties(sender, EventArgs.Empty);
    }
    protected abstract void UpdateDerivedProperties(object sender, EventArgs e);

    public void Dispose()
    {
        _combatStatBoard = null;
        _parent = null;
        EntityStatsEventSystem.CombatStatsChanged -= UpdateDerivedProperties;
    }


}
