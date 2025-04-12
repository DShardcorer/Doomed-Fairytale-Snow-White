using DialogueSystem;
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
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private PoolManager _poolManager;
        [SerializeField] private PlayerManager _playerManager;
        [SerializeField] private UpdateManager _updateManager;
        [SerializeField] private FixedUpdateManager _fixedUpdateManager;
        [SerializeField] private NativeManager _enemyManager;
        [SerializeField] private CameraManager _cameraManager;
        [SerializeField] private QuestManager _questManager;
        [SerializeField] private DialogueManager _dialogueManager;

        public InputManager InputManager => _inputManager;
        public PoolManager PoolManager => _poolManager;
        public NativeManager EnemyManager => _enemyManager;
        public PlayerManager PlayerManager => _playerManager;
        public UpdateManager UpdateManager => _updateManager;
        public FixedUpdateManager FixedUpdateManager => _fixedUpdateManager;
        public CameraManager CameraManager => _cameraManager;
        public QuestManager QuestManager => _questManager;
        public DialogueManager DialogueManager => _dialogueManager;




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
            _inputManager.Initialize(this);
            _cameraManager.Initialize(this);
            _poolManager.Initialize(this);
            _playerManager.Initialize(this);
            _updateManager.Initialize(this);
            _fixedUpdateManager.Initialize(this);
            _enemyManager.Initialize(this);
            _questManager.Initialize(this); 
            _dialogueManager.Initialize(this);


        }


    }
}
