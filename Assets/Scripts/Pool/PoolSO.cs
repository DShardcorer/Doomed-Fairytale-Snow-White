
using UnityEngine;

[CreateAssetMenu(fileName = "Pool", menuName = "Pool")]
public class PoolSO : ScriptableObject
{
    public string key;
    public GameObject prefab;
    public int size;
}
