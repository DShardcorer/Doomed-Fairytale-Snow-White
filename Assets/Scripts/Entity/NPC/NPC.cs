using UnityEngine;

public class NPC : Entity, ILifecycle<NPCManager>, IUpdatable, IFixedUpdatable
{
    protected NPCManager _parent;
    private NPCView _npcView;
    public NPCView NPCView => _npcView;
    private NPCProperties _npcProperties;
    public NPCProperties NPCProperties => _npcProperties;
    private EntityDetector _entityDetector;
    public EntityDetector EntityDetector => _entityDetector;

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
    protected NPCMovingProperties _npcMovingProperties;
    public NPCMovingProperties NPCMovingProperties => _npcMovingProperties;

    // Attacking
    protected NPCAttackingState _npcAttackingState;
    public NPCAttackingState NPCAttackingState => _npcAttackingState;
    protected NPCAttackingProperties _npcAttackingProperties;
    public NPCAttackingProperties NPCAttackingProperties => _npcAttackingProperties;

    // Chasing
    private NPCChasingState _npcChasingState;
    public NPCChasingState NPCChasingState => _npcChasingState;
    private NPCChasingProperties _npcChasingProperties;
    public NPCChasingProperties NPCChasingProperties => _npcChasingProperties;

    #endregion StateDefinitions

    public NPC(NPCView view, NPCProperties properties, NPCIdlingState npcIdlingState, NPCMovingState npcMovingState) : base(view, properties)
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
        _entityDetector = _view.GetComponentInChildren<EntityDetector>();

        // State initialization
        _npcIdlingState.Initialize(this);
        _npcMovingState.Initialize(this);

    }

    public virtual void InitializeStates()
    {

    }

    public void UpdateLogic()
    {
        if (IsBusy) return;
        _stateMachine.UpdateLogic();
    }

    public void FixedUpdateLogic()
    {
        if (IsBusy) return;
        _stateMachine.FixedUpdateLogic();
    }

    public void Dispose()
    {
        _parent.GameManager.UpdateManager.RemoveUpdatable(this);
        _parent.GameManager.FixedUpdateManager.RemoveFixedUpdatable(this);
    }
}
