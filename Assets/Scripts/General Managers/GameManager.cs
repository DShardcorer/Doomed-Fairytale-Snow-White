using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private PoolManager _poolManager;
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private UpdateManager _updateManager;
    [SerializeField] private FixedUpdateManager _fixedUpdateManager;
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private CameraManager _cameraManager;

    public InputManager InputManager => _inputManager;
    public PoolManager PoolManager => _poolManager;
    public EnemyManager EnemyManager => _enemyManager;
    public PlayerManager PlayerManager => _playerManager;
    public UpdateManager UpdateManager => _updateManager;
    public FixedUpdateManager FixedUpdateManager => _fixedUpdateManager;
    public CameraManager CameraManager => _cameraManager;




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

        Initialize();
    }

    private void Initialize()
    {
        _inputManager.Initialize(this);
        _cameraManager.Initialize(this);
        _poolManager.Initialize(this);
        _playerManager.Initialize(this);
        _updateManager.Initialize(this);
        _fixedUpdateManager.Initialize(this);
        _enemyManager.Initialize(this);


    }


}
