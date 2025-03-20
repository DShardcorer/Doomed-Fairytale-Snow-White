using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    // Container where UI screens will be placed (usually a panel on a Canvas)
    public Transform uiContainer;


    // Stack to track active UI screens.
    private Stack<GameObject> uiStack = new Stack<GameObject>();

    // Dictionary to store instantiated UI screens for reuse.
    private Dictionary<GameObject, GameObject> uiInstances = new Dictionary<GameObject, GameObject>();

    private void Awake()
    {
        // Singleton pattern implementation.
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        
    }

    private void Update()
    {
        // Listen for the Backspace key to navigate back.
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            PopUI();
        }
    }

    /// <summary>
    /// Enables (or instantiates) and pushes a new UI screen.
    /// </summary>
    public void PushUI(GameObject uiPrefab)
    {
        if (uiPrefab == null)
        {
            Debug.LogWarning("UI prefab is null!");
            return;
        }

        // Check if the UI has already been instantiated
        if (!uiInstances.TryGetValue(uiPrefab, out GameObject uiInstance))
        {
            uiInstance = Instantiate(uiPrefab, uiContainer);
            uiInstances[uiPrefab] = uiInstance; // Store instance for reuse
        }

        uiInstance.SetActive(true);
        uiStack.Push(uiInstance);
    }

    /// <summary>
    /// Disables the top UI screen instead of destroying it.
    /// </summary>
    public void PopUI()
    {
        if (uiStack.Count > 0)
        {
            GameObject topUI = uiStack.Pop();
            topUI.SetActive(false);
        }
        else
        {
            Debug.Log("UI Stack is empty.");
        }
    }

    /// <summary>
    /// Returns the currently active UI screen (if any).
    /// </summary>
    public GameObject GetCurrentUI()
    {
        return uiStack.Count > 0 ? uiStack.Peek() : null;
    }
}
