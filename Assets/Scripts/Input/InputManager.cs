using System;
using UnityEngine;

public class InputManager : MonoBehaviour, ILifecycle<GameManager>
{
    private GameManager _gameManager;

    private PlayerInputActions _playerInputActions;
    public event EventHandler attackInputted;
    public event EventHandler dashInputted;
    public event EventHandler skill1Inputted;
    public event EventHandler interactInputted;
    public event EventHandler openMenuInputted;


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
        _playerInputActions.Player.Skill1.performed += ctx => skill1Inputted?.Invoke(this, EventArgs.Empty);
        _playerInputActions.Player.OpenMenu.performed += ctx => openMenuInputted?.Invoke(this, EventArgs.Empty);
        _playerInputActions.Player.Interact.performed += ctx => interactInputted?.Invoke(this, EventArgs.Empty);
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
