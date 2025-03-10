using UnityEngine;

public class PlayerInteract : IUpdatable, ILifecycle<Player>
{
    private Player _player;
    private PlayerInteractProperties _playerInteractProperties;
    private PlayerInteractView _playerInteractView;

    public PlayerInteract(PlayerInteractProperties playerInteractProperties, PlayerInteractView playerInteractView)
    {
        _playerInteractProperties = playerInteractProperties;
        _playerInteractView = playerInteractView;
    }


    public void Initialize(Player player)
    {
        _player = player;
        _playerInteractView.Initialize(this);
        _player.PlayerManager.GameManager.UpdateManager.AddUpdatable(this);
    }

    public void UpdateLogic()
    {

    }


    public void Dispose()
    {
        _player.PlayerManager.GameManager.UpdateManager.RemoveUpdatable(this);
        _playerInteractView.Dispose();
    }
}
