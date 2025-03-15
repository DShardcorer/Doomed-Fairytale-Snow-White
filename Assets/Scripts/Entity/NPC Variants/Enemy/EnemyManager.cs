using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : NPCManager
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private NPCPropertiesSO _enemyPropertiesSO;
    private List<Enemy> _enemies = new List<Enemy>();


    public override void SpawnNPC(Vector3 position)
    {
        // EnemyProperties enemyProperties = new EnemyProperties(_enemyPropertiesSO);
        EnemyView enemyView = _poolManager.GetObject(_enemyPrefab.name).GetComponent<EnemyView>();
        EnemyProperties enemyProperties = new EnemyProperties(_enemyPropertiesSO);


        //State creation
        EnemyIdlingProperties enemyIdlingProperties = new EnemyIdlingProperties(2);
        EnemyIdlingState enemyIdlingState = new EnemyIdlingState(enemyIdlingProperties, AnimationStateHelper.IS_IDLING);

        EnemyMovingProperties enemyMovingProperties = new EnemyMovingProperties(2);
        EnemyMovingState enemyMovingState = new EnemyMovingState(enemyMovingProperties, AnimationStateHelper.IS_MOVING);

        Enemy enemy = new Enemy(enemyView, enemyProperties, enemyIdlingState, enemyMovingState);
        
        enemy.Initialize(this);

        enemy.EnemyView.transform.position = position;
        _enemies.Add(enemy);
        Debug.Log("Enemy spawned");
    }


    public override void Dispose()
    {
        base.Dispose();
        _enemies.Clear();
    }



}
