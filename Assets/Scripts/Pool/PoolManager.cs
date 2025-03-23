using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private GameManager _parent;
    
    [SerializeField] private List<PoolSO> uiElementsPoolSOList = new List<PoolSO>();
    [SerializeField] private List<PoolSO> npcPoolSOList = new List<PoolSO>();
    [SerializeField] private List<PoolSO> environmentPoolSOList = new List<PoolSO>();

    private Dictionary<string, ObjectPool> pools = new Dictionary<string, ObjectPool>();

    public void Initialize(GameManager parent)
    {
        _parent = parent;
        CreatePoolsFromList(uiElementsPoolSOList);
        CreatePoolsFromList(npcPoolSOList);
        CreatePoolsFromList(environmentPoolSOList);
    }

    private void CreatePoolsFromList(List<PoolSO> poolList)
    {
        foreach (PoolSO pool in poolList)
        {
            CreatePool(pool.key, pool.prefab, pool.size);
        }
    }

    public void CreatePool(string key, GameObject prefab, int size)
    {
        if (!pools.ContainsKey(key))
        {
            GameObject poolObj = new GameObject($"{key}Pool");
            poolObj.transform.SetParent(transform);
            ObjectPool pool = poolObj.AddComponent<ObjectPool>();
            pool.Initialize(prefab, size);
            pools.Add(key, pool);
        }
    }

    public GameObject GetObject(string key)
    {
        if (pools.ContainsKey(key))
            return pools[key].GetObject();

        Debug.LogWarning($"Pool with key '{key}' not found!");
        return null;
    }

    public void ReturnObject(string key, GameObject obj)
    {
        if (pools.ContainsKey(key))
            pools[key].ReturnObject(obj);
        else
            Debug.LogWarning($"No pool found for '{key}' to return object.");
    }
}
