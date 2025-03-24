using System;
using UnityEngine;

public class NPC : Entity, ILifecycle<NPCManager>, IUpdatable, IFixedUpdatable
{
    protected NPCManager _parent;
    protected NPCView _npcView;
    public NPCView NPCView => _npcView;
    protected NPCProperties _npcProperties;
    public NPCProperties NPCProperties => _npcProperties;
    protected FOVDetector _fovDetector;
    public FOVDetector FOVDetector => _fovDetector;
    protected ProximityDetector _proximityDetector;
    public ProximityDetector ProximityDetector => _proximityDetector;



    public bool IsBusy = false;

    #region StateDefinitions
    // Idling
    protected NPCIdlingState _npcIdlingState;
    public NPCIdlingState NPCIdlingState => _npcIdlingState;

    protected NPCIdlingProperties _npcIdlingProperties;
    public NPCIdlingProperties NPCIdlingProperties => _npcIdlingProperties;

    // Moving
    protected NPCMovingState _npcMovingState;
    public NPCMovingState NPCMovingState => _npcMovingState;

    // Attacking
    protected NPCAttackingState _npcAttackingState;
    public NPCAttackingState NPCAttackingState => _npcAttackingState;

    // Chasing
    private NPCChasingState _npcChasingState;


    public NPCChasingState NPCChasingState => _npcChasingState;

    #endregion StateDefinitions

    public NPC(NPCView view, NPCProperties properties, 
    NPCIdlingState npcIdlingState, NPCMovingState npcMovingState,
    StatSystem statSystem, EquipmentSystem equipmentSystem, SkillSystem skillSystem, LevelSystem levelSystem, HealthSystem healthSystem, ManaSystem manaSystem, StaminaSystem staminaSystem,
    EntityStateMachine stateMachine, InventorySystem inventory
    ) : base(view, properties, statSystem, equipmentSystem, skillSystem, levelSystem, healthSystem, manaSystem, staminaSystem , stateMachine, inventory)
    {
        _npcView = view;
        _npcProperties = properties;
        _npcIdlingState = npcIdlingState;
        _npcMovingState = npcMovingState;
    }

    public virtual void Initialize(NPCManager parent)
    {
        // Getting references
        _parent = parent;

        // Add to update managers
        _parent.GameManager.UpdateManager.AddUpdatable(this);
        _parent.GameManager.FixedUpdateManager.AddFixedUpdatable(this);

        // View initialization
        _view.Initialize(this);

        // Entity detector initialization
        _fovDetector = _view.GetComponentInChildren<FOVDetector>();
        _fovDetector.Initialize(this);
        _proximityDetector = _view.GetComponentInChildren<ProximityDetector>();
        _proximityDetector.Initialize(this);

        _proximityDetector.OnEntityFromDifferentFactionSpottedInProximity += OnEntityFromDifferentFactionSpottedInProximity;
        _fovDetector.OnClosestEntityFromDifferentFactionSpottedInFOV += OnClosestEntityFromDifferentFactionSpottedInFOV;



        // State initialization
        _npcIdlingState.Initialize(this);
        _npcMovingState.Initialize(this);

        base.Initialize();

    }

    protected virtual void OnClosestEntityFromDifferentFactionSpottedInFOV(object sender, Entity e)
    {
        
    }

    protected virtual void OnEntityFromDifferentFactionSpottedInProximity(object sender, Entity e)
    {

        _properties.lastMovementVector = (e.View.transform.position - _view.transform.position).normalized;
    }


    public void UpdateLogic()
    {
        if (IsBusy) return;
        _stateMachine.UpdateLogic();
    }

    public override void FixedUpdateLogic()
    {
        base.FixedUpdateLogic();
        if (IsBusy) return;
        _stateMachine.FixedUpdateLogic();
    }

    public void Dispose()
    {
        _parent.GameManager.UpdateManager.RemoveUpdatable(this);
        _parent.GameManager.FixedUpdateManager.RemoveFixedUpdatable(this);

        _proximityDetector.OnEntityFromDifferentFactionSpottedInProximity -= OnEntityFromDifferentFactionSpottedInProximity;
        _fovDetector.OnClosestEntityFromDifferentFactionSpottedInFOV -= OnClosestEntityFromDifferentFactionSpottedInFOV;

        //null all references
        _parent = null;
        _npcView = null;
        _npcProperties = null;
        _fovDetector = null;
        _proximityDetector = null;
        _npcIdlingState = null;
        _npcMovingState = null;
        
    }


}
