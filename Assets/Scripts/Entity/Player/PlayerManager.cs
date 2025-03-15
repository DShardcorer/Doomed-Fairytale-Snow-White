using UnityEngine;

public class PlayerManager : MonoBehaviour, ILifecycle<GameManager>
{
    private GameManager _gameManager;
    private Player _player;

    public GameManager GameManager => _gameManager;

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private float _playerSpeed = 5;
    [SerializeField] private float _playerHealth = 100;
    [SerializeField] private float _playerInteractionRange = 3;
    [SerializeField] private float _dashSpeed = 20;
    [SerializeField] private float _dashDuration = 0.667f;
    [SerializeField] private float _dashCooldown = 1f;

    [SerializeField] private float _attackDamage = 10;
    [SerializeField] private float _attackRange = 1;
    [SerializeField] private float _attackCooldown = 1;
    [SerializeField] private float _attackDashVelocity = 5;

    public float AttackDamage => _attackDamage;
    public float AttackRange => _attackRange;
    public float AttackCooldown => _attackCooldown;
    public float AttackDashVelocity => _attackDashVelocity;



    public float PlayerSpeed => _playerSpeed;
    public float PlayerHealth => _playerHealth;
    public float PlayerInteractionRange => _playerInteractionRange;
    public float DashSpeed => _dashSpeed;
    public float DashDuration => _dashDuration;
    public float DashCooldown => _dashCooldown;

    public void Initialize(GameManager gameManager)
    {
        _gameManager = gameManager;
        CreatePlayer();
    }

    public void CreatePlayer()
    {
        GameObject playerGameObject = Instantiate(_playerPrefab);

        //PlayerView creation
        PlayerView playerView = playerGameObject.GetComponent<PlayerView>();
        Debug.Log(playerView.Rigidbody2D);

        //PlayerProperties creation
        PlayerProperties playerProperties = new PlayerProperties();

        PlayerIdlingState playerIdlingState = new PlayerIdlingState(AnimationStateHelper.IS_IDLING);


        PlayerMovingProperties playerMovingProperties = new PlayerMovingProperties();
        PlayerMovingState playerMovingState = new PlayerMovingState(playerMovingProperties, AnimationStateHelper.IS_MOVING);


        PlayerDashingProperties playerDashingProperties = new PlayerDashingProperties();
        PlayerDashingState playerDashingState = new PlayerDashingState(playerDashingProperties, AnimationStateHelper.IS_DASHING);


        PlayerAttackingProperties playerAttackingProperties = new PlayerAttackingProperties();
        PlayerAttackingState playerAttackingState = new PlayerAttackingState(playerAttackingProperties, AnimationStateHelper.IS_ATTACKING);


        _player = new Player(playerView, playerProperties, playerIdlingState, playerMovingState, playerDashingState, playerAttackingState);
        _player.Initialize(this);
    }
    public Player GetPlayer()
    {
        return _player;
    }

    public void Dispose()
    {
        _player = null;
    }


}
