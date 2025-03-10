
using UnityEngine;

public class PlayerMovement : IFixedUpdatable, ILifecycle<Player>
{
    private Player _player;
    private PlayerMovementProperties _playerMovementProperties;
    private PlayerMovementView _playerMovementView;

    public PlayerMovementProperties PlayerMovementProperties => _playerMovementProperties;
    public PlayerMovementView PlayerMovementView => _playerMovementView;

    private InputManager _inputManager;

    public PlayerMovement(PlayerMovementProperties playerMovementProperties, PlayerMovementView playerMovementView)
    {
        _playerMovementProperties = playerMovementProperties;
        _playerMovementView = playerMovementView;
    }

    public void Initialize(Player player)
    {
        _player = player;
        _playerMovementView.Initialize(this);
        _inputManager = _player.PlayerManager.GameManager.InputManager;
        _player.PlayerManager.GameManager.FixedUpdateManager.AddFixedUpdatable(this);
    }



    public void FixedUpdateLogic()
    {
        Vector2 movementVector = _inputManager.GetMovementVector();
        Vector2 movement = movementVector * _playerMovementProperties.Speed * Time.deltaTime;
        _playerMovementView.Move(movement);
    }

    public void Dispose()
    {
        _player.PlayerManager.GameManager.FixedUpdateManager.RemoveFixedUpdatable(this);
        _playerMovementView.Dispose();

    }


}
