using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, ILifecycle<GameManager>
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private EnemyPropertiesSO _enemyPropertiesSO;
    private List<Enemy> _enemies = new List<Enemy>();
    private GameManager _gameManager;

    public GameManager GameManager => _gameManager;
    private PoolManager _poolManager;

    public void Initialize(GameManager gameManager)
    {
        _gameManager = gameManager;
        _poolManager = _gameManager.PoolManager;
    }

    public void SpawnEnemy(Vector3 position)
    {
        // EnemyProperties enemyProperties = new EnemyProperties(_enemyPropertiesSO);
        EnemyView enemyView = _poolManager.GetObject(_enemyPrefab.name).GetComponent<EnemyView>();
        Enemy enemy = new Enemy(enemyView);
        
        enemy.Initialize(this);

        enemy.EnemyView.transform.position = position;
        _enemies.Add(enemy);
        Debug.Log("Enemy spawned");
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SpawnEnemy(Vector3.zero);
        }
    }



    public void Dispose()
    {
        _enemies.Clear();
    }



}
