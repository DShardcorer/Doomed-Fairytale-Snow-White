

public class Native : NPC
{
    protected NativeManager _nativeManager;
    protected NativeProperties _nativeProperties;
    protected NativeView _nativeView;

    public NativeProperties NativeProperties => _nativeProperties;
    public NativeView NativeView => _nativeView;

    protected NativeIdlingState _nativeIdlingState;
    public NativeIdlingState NativeIdlingState => _nativeIdlingState;

    protected NativeMovingState _nativeMovingState;
    public NativeMovingState NativeMovingState => _nativeMovingState;

    protected NativeChasingState _nativeChasingState;
    public NativeChasingState NativeChasingState => _nativeChasingState;

    protected NativeAttackingState _nativeAttackingState;
    public NativeAttackingState NativeAttackingState => _nativeAttackingState;



    public Native(NativeView view, NativeProperties properties,
     NativeIdlingState nativeIdlingState, NativeMovingState nativeMovingState,
      NativeChasingState nativeChasingState, NativeAttackingState nativeAttackingState,
        StatSystem statSystem, EquipmentSystem equipmentSystem,
         SkillSystem skillSystem, LevelSystem levelSystem, HealthSystem healthSystem, ManaSystem manaSystem, StaminaSystem staminaSystem,
        EntityStateMachine stateMachine, InventorySystem inventory
      ) : base(view, properties, nativeIdlingState, nativeMovingState, 
      statSystem, equipmentSystem,
      skillSystem, levelSystem, healthSystem, manaSystem, staminaSystem, stateMachine, inventory)
    {
        _nativeView = view;
        _nativeProperties = properties;
        _nativeIdlingState = nativeIdlingState;
        _nativeMovingState = nativeMovingState;
        _nativeChasingState = nativeChasingState;
        _nativeAttackingState = nativeAttackingState;
    }





    public void Initialize(NativeManager parent)
    {
        _nativeManager = parent;
        base.Initialize(parent); // Call base NPC initialization

        
        
        _nativeChasingState.Initialize(this);
        _nativeAttackingState.Initialize(this);


        _stateMachine.Initialize(_nativeIdlingState);

    }

    protected override void OnClosestEntityFromDifferentFactionSpottedInFOV(object sender, Entity e)
    {
        base.OnClosestEntityFromDifferentFactionSpottedInFOV(sender, e);
        _properties.target = e;
        _stateMachine.ChangeState(_nativeChasingState);
    }

    protected override void OnEntityFromDifferentFactionSpottedInProximity(object sender, Entity e)
    {
        base.OnEntityFromDifferentFactionSpottedInProximity(sender, e);
    }
    public override void Die()
    {
        base.Die();
        _nativeManager.DespawnMeleeNative(this);
    }
}
