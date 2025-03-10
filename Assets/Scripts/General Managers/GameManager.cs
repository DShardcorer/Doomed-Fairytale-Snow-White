using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private UpdateManager _updateManager;
    [SerializeField] private FixedUpdateManager _fixedUpdateManager;

    public InputManager InputManager => _inputManager;
    public PlayerManager PlayerManager => _playerManager;
    public UpdateManager UpdateManager => _updateManager;
    public FixedUpdateManager FixedUpdateManager => _fixedUpdateManager;



    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Initialize();
    }

    private void Initialize()
    {
        _inputManager.Initialize(this);
        _playerManager.Initialize(this);
        _updateManager.Initialize(this);
        _fixedUpdateManager.Initialize(this);
        
    }


}
