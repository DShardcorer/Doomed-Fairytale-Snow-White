

public class Player: ILifecycle<PlayerManager>
{
    private PlayerManager _playerManager;
    private PlayerMovement _playerMovement;
    private PlayerInteract _playerInteract;

    public PlayerManager PlayerManager => _playerManager;

    public Player(PlayerMovement playerMovement, PlayerInteract playerInteract)
    {
        _playerMovement = playerMovement;
        _playerInteract = playerInteract;
    }

    public void Initialize(PlayerManager playerManager)
    {
        _playerManager = playerManager;
        _playerMovement.Initialize(this);
        _playerInteract.Initialize(this);
    }
    public void Dispose()
    {
        _playerMovement.Dispose();
        _playerInteract.Dispose();
    }






}
