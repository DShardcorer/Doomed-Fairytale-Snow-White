using UnityEngine;

public class Enemy : NPC
{
    protected EnemyManager _enemyManager;
    protected EnemyProperties _enemyProperties;
    protected EnemyView _enemyView;

    public EnemyProperties EnemyProperties => _enemyProperties;
    public EnemyView EnemyView => _enemyView;

    private EnemyIdlingProperties _enemyIdlingProperties;
    public EnemyIdlingProperties EnemyIdlingProperties => _enemyIdlingProperties;

    private EnemyIdlingState _enemyIdlingState;
    public EnemyIdlingState EnemyIdlingState => _enemyIdlingState;

    private EnemyMovingProperties _enemyMovingProperties;
    public EnemyMovingProperties EnemyMovingProperties => _enemyMovingProperties;
    private EnemyMovingState _enemyMovingState;

    public Enemy(EnemyView view, EnemyProperties properties, EnemyIdlingState npcIdlingState, EnemyMovingState npcMovingState) : base(view, properties, npcIdlingState, npcMovingState)
    {
        _enemyView = view;
        _enemyProperties = properties;
        _enemyIdlingState = npcIdlingState;
        _enemyMovingState = npcMovingState;
    }

    public EnemyMovingState EnemyMovingState => _enemyMovingState;



    public void Initialize(EnemyManager parent)
    {
        _enemyManager = parent;
        base.Initialize(parent); // Call base NPC initialization
        _stateMachine.Initialize(_enemyIdlingState);

    }
}
