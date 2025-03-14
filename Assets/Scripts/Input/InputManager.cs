using System;
using UnityEngine;

public class InputManager : MonoBehaviour, ILifecycle<GameManager>
{
    private GameManager _gameManager;

    private PlayerInputActions _playerInputActions;

    public event EventHandler attackInputted;
    public event EventHandler dashInputted;


    public void Initialize(GameManager gameManager)
    {
        _gameManager = gameManager;
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
        InitializeEvents();
    }

    private void InitializeEvents()
    {
        _playerInputActions.Player.Attack.performed += ctx => attackInputted?.Invoke(this, EventArgs.Empty);
        _playerInputActions.Player.Dash.performed += ctx => dashInputted?.Invoke(this, EventArgs.Empty);


    }

    public GameManager GetGameManager(){
        return _gameManager;
    }


    public void Dispose()
    {
        _playerInputActions.Disable();
    }
    public Vector2 GetMovementVector(){
        Vector2 movement = _playerInputActions.Player.Move.ReadValue<Vector2>();
        return movement;
    }


}
