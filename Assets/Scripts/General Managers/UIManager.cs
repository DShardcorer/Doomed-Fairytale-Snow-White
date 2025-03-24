using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public Transform uiContainer;

    [SerializeField]
    private IngameMenuUI _ingameMenuUI;
    public IngameMenuUI IngameMenuUI => _ingameMenuUI;


    [SerializeField]
    private HealthUI _healthUI;
    public HealthUI HealthUI => _healthUI;

    [SerializeField]
    private ManaUI _manaUI;
    public ManaUI ManaUI => _manaUI;

    [SerializeField]
    private StaminaUI _staminaUI;
    public StaminaUI StaminaUI => _staminaUI;





    // Stack to track active UI screens.
    private Stack<GameObject> uiStack = new Stack<GameObject>();

    // Dictionary to store instantiated UI screens for reuse.
    private Dictionary<GameObject, GameObject> uiInstances = new Dictionary<GameObject, GameObject>();

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }


    public void Initialize()
    {
        GameManager.Instance.InputManager.openMenuInputted += InputManager_openMenuInputted;
        _ingameMenuUI.Initialize(this);
        _ingameMenuUI.gameObject.SetActive(false);
        _healthUI.Initialize(this);
        _manaUI.Initialize(this);
        _staminaUI.Initialize(this);

        InvokeInitialEvents();
    }

    private void InvokeInitialEvents()
    {
        GameManager.Instance.PlayerManager.GetPlayer().Inventory.InvokeInitialEvents();
        GameManager.Instance.PlayerManager.GetPlayer().HealthSystem.InvokeInitialEvents();
        GameManager.Instance.PlayerManager.GetPlayer().ManaSystem.InvokeInitialEvents();
        GameManager.Instance.PlayerManager.GetPlayer().StaminaSystem.InvokeInitialEvents();
        GameManager.Instance.PlayerManager.GetPlayer().StatSystem.InvokeInitialEvents();
        GameManager.Instance.PlayerManager.GetPlayer().EquipmentSystem.InvokeInitialEvents();
        GameManager.Instance.PlayerManager.GetPlayer().LevelSystem.InvokeInitialEvents();
    }



    private void InputManager_openMenuInputted(object sender, EventArgs e)
    {
        _ingameMenuUI.gameObject.SetActive(!_ingameMenuUI.gameObject.activeSelf);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            //add 50 xp
            GameManager.Instance.PlayerManager.GetPlayer().LevelSystem.AddExperience(50);
        }

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
