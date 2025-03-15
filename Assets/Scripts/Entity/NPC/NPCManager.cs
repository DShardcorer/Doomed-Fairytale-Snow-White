using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour, ILifecycle<GameManager>
{
    [SerializeField] private GameObject _npcPrefab;
    [SerializeField] private NPCPropertiesSO _npcPropertiesSO;

    protected GameManager _gameManager;

    public GameManager GameManager => _gameManager;
    protected PoolManager _poolManager;

    public void Initialize(GameManager gameManager)
    {
        _gameManager = gameManager;
        _poolManager = _gameManager.PoolManager;
    }

    public virtual void SpawnNPC(Vector3 position)
    {
    }

    public virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnNPC(Vector3.zero);
        }
    }

    public virtual void Dispose()
    {

    }
}
