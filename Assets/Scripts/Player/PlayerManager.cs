using UnityEngine;

public class PlayerManager : MonoBehaviour, ILifecycle<GameManager>
{
    private GameManager _gameManager;
    private Player _player;

    public GameManager GameManager => _gameManager;

    [SerializeField] private GameObject _playerPrefab;

    public void Initialize(GameManager gameManager)
    {
        _gameManager = gameManager;
        CreatePlayer();
    }

    public void CreatePlayer()
    {
        GameObject playerGameObject = Instantiate(_playerPrefab);
        
        //PlayerMovement creation
        PlayerMovementView playerMovementView = playerGameObject.GetComponent<PlayerMovementView>();
        PlayerMovementProperties playerMovementProperties = new PlayerMovementProperties();
        PlayerMovement playerMovement = new PlayerMovement(playerMovementProperties, playerMovementView);


        //PlayerInteract creation
        PlayerInteractView playerInteractView = playerGameObject.GetComponent<PlayerInteractView>();
        PlayerInteractProperties playerInteractProperties = new PlayerInteractProperties();
        PlayerInteract playerInteract = new PlayerInteract(playerInteractProperties, playerInteractView);



        //Player creation
        _player = new Player(playerMovement, playerInteract);
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
