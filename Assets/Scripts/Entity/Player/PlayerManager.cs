using UnityEngine;

public class PlayerManager : MonoBehaviour, ILifecycle<GameManager>
{
    private GameManager _gameManager;
    private Player _player;

    public GameManager GameManager => _gameManager;

    [SerializeField] private GameObject _playerPrefab;

    [SerializeField] private PlayerPropertiesSO _playerPropertiesSO;



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

        PlayerProperties playerProperties = new PlayerProperties(_playerPropertiesSO);

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
