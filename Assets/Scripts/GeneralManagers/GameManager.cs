using AudioSystem;
using DateDayNightSystem;
using DateTimeDayNightSystem;
using DefaultNamespace.BarterSystem;
using DefaultNamespace.LightingSystem;
using DialogueSystem;
using Entity.NPC.Spawning;
using Entity.NPC_Variants.Native;
using Entity.Player;
using Input;
using Pool;
using QuestSystem;
using UnityEngine;

namespace GeneralManagers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Cameras")]
        [SerializeField] public Camera MainCamera;
        [SerializeField] public Camera UIMainCamera;

        [Header("Managers")]
        [SerializeField] private BarterManager _barterManager;
        [SerializeField] private NPCSpawnManager _npcSpawnManager;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private PoolManager _poolManager;
        [SerializeField] private PlayerManager _playerManager;
        [SerializeField] private PlayerViewManager _playerViewManager;
        [SerializeField] private UpdateManager _updateManager;
        [SerializeField] private FixedUpdateManager _fixedUpdateManager;
        [SerializeField] private GameTimeManager _gameTimeManager;
        [SerializeField] private DayCycleLightingManager _dayCycleLightingManager;
        [SerializeField] private NativeManager _enemyManager;
        [SerializeField] private CameraManager _cameraManager;
        [SerializeField] private QuestManager _questManager;
        [SerializeField] private DialogueManager _dialogueManager;

        #region Public Accessors
        public BarterManager BarterManager => _barterManager;
        public NPCSpawnManager NPCSpawnManager => _npcSpawnManager;
        public AudioManager AudioManager => _audioManager;
        public InputManager InputManager => _inputManager;
        public PoolManager PoolManager => _poolManager;
        public PlayerManager PlayerManager => _playerManager;
        public PlayerViewManager PlayerViewManager => _playerViewManager;
        public UpdateManager UpdateManager => _updateManager;
        public FixedUpdateManager FixedUpdateManager => _fixedUpdateManager;
        public DayCycleLightingManager DayCycleLightingManager => _dayCycleLightingManager;
        public GameTimeManager GameTimeManager => _gameTimeManager;
        public NativeManager EnemyManager => _enemyManager;
        public CameraManager CameraManager => _cameraManager;
        public QuestManager QuestManager => _questManager;
        public DialogueManager DialogueManager => _dialogueManager;

        #endregion

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
            _barterManager.Initialize(this);
            _npcSpawnManager.Initialize(this);
            _audioManager.Initialize(this);
            _inputManager.Initialize(this);
            _poolManager.Initialize(this);
            _playerManager.Initialize(this);
            _playerViewManager.Initialize(this);
            _updateManager.Initialize(this);
            _fixedUpdateManager.Initialize(this);
            _gameTimeManager.Initialize(this);
            _dayCycleLightingManager.Initialize(this);
            _enemyManager.Initialize(this);
            _questManager.Initialize(this);
            _dialogueManager.Initialize(this);
            _cameraManager.Initialize(this);
        }

        public void Dispose()
        {
            _npcSpawnManager.Dispose();
            _gameTimeManager.Dispose();
            _inputManager.Dispose();
            _poolManager.Dispose();
            _playerManager.Dispose();
            _updateManager.Dispose();
            _fixedUpdateManager.Dispose();
            _cameraManager.Dispose();
            _enemyManager.Dispose();
            _questManager.Dispose();
            _dialogueManager.Dispose();

            Instance = null;
            Destroy(gameObject);
        }
    }
}
