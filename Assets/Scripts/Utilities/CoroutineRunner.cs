using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(-101)]
public class CoroutineRunner : MonoBehaviour
{
    public static CoroutineRunner Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Run(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}