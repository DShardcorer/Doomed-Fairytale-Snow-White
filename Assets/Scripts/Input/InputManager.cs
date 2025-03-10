using System;
using UnityEngine;

public class InputManager : MonoBehaviour, ILifecycle<GameManager>
{
    private GameManager _gameManager;

    private PlayerInputActions _playerInputActions;


    public void Initialize(GameManager gameManager)
    {
        _gameManager = gameManager;
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
        InitializeEvents();
    }

    private void InitializeEvents()
    {

    }

    public GameManager GetGameManager(){
        return _gameManager;
    }


    public void Dispose()
    {
        _playerInputActions.Disable();
    }
    public Vector2 GetMovementVector(){
        return _playerInputActions.Player.Move.ReadValue<Vector2>();
    }


}
