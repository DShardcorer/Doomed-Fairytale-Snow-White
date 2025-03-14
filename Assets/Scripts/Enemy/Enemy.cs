using UnityEngine;

public class Enemy : Entity, ILifecycle<EnemyManager>, IUpdatable, IFixedUpdatable
{
    private EnemyManager _parent;
    private EnemyView _enemyView;
    public EnemyView EnemyView => _enemyView;
    private EntityDetector _entityDetector;
    public EntityDetector EntityDetector => _entityDetector;

    public bool IsBusy = false;


    #region StateDefinitions
    //Idling
    private EnemyIdlingState _enemyIdlingState;
    public EnemyIdlingState EnemyIdlingState => _enemyIdlingState;

    private EnemyIdlingProperties _enemyIdlingProperties;
    public EnemyIdlingProperties EnemyIdlingProperties => _enemyIdlingProperties;


    //Moving
    private EnemyMovingState _enemyMovingState;
    public EnemyMovingState EnemyMovingState => _enemyMovingState;
    private EnemyMovingProperties _enemyMovingProperties;
    public EnemyMovingProperties EnemyMovingProperties => _enemyMovingProperties;


    //Attacking
    private EnemyAttackingState _enemyAttackingState;
    public EnemyAttackingState EnemyAttackingState => _enemyAttackingState;
    private EnemyAttackingProperties _enemyAttackingProperties;
    public EnemyAttackingProperties EnemyAttackingProperties => _enemyAttackingProperties;



    //Chasing
    private EnemyChasingState _enemyChasingState;
    public EnemyChasingState EnemyChasingState => _enemyChasingState;
    private EnemyChasingProperties _enemyChasingProperties;
    public EnemyChasingProperties EnemyChasingProperties => _enemyChasingProperties;

    #endregion StateDefinitions

    public Enemy(EnemyView view) : base(view)
    {
        _enemyView = view;
    }

    public void Initialize(EnemyManager parent)
    {
        //Getting references
        _parent = parent;

        //Add to update managers
        _parent.GameManager.UpdateManager.AddUpdatable(this);
        _parent.GameManager.FixedUpdateManager.AddFixedUpdatable(this);

        //View initialization
        _view.Initialize(this);
        //Entity detector initialization
        _entityDetector = _view.GetComponentInChildren<EntityDetector>();


        //State initialization
        _enemyIdlingProperties = new EnemyIdlingProperties(2);
        _enemyIdlingState = new EnemyIdlingState(_enemyIdlingProperties, AnimationStateHelper.IS_IDLING);
        _enemyIdlingState.Initialize(this);

        _enemyMovingProperties = new EnemyMovingProperties(2);
        _enemyMovingState = new EnemyMovingState(_enemyMovingProperties, AnimationStateHelper.IS_MOVING);
        _enemyMovingState.Initialize(this);

        // _enemyAttackingProperties = new EnemyAttackingProperties(2);
        // _enemyAttackingState = new EnemyAttackingState(_enemyAttackingProperties, "isAttacking");
        // _enemyAttackingState.Initialize(this);

        // _enemyChasingProperties = new EnemyChasingProperties(2);
        // _enemyChasingState = new EnemyChasingState(_enemyChasingProperties, "isChasing");
        // _enemyChasingState.Initialize(this);

        _stateMachine.Initialize(_enemyIdlingState);
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
