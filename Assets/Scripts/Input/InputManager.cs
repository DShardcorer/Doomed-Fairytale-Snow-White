using System;
using GeneralManagers;
using UnityEngine;

namespace Input
{
    public class InputManager : MonoBehaviour, ILifecycle<GameManager>
    {
        private GameManager _gameManager;
        public GameManager GameManager => _gameManager;

        public InputEventContext inputEventContext = InputEventContext.DEFAULT;

        private PlayerInputActions _playerInputActions;
        public event EventHandler attackInputted;
        public event EventHandler dashInputted;
        public event EventHandler skill1Inputted;
        public event Action<InputEventContext> interactInputted;
        public event EventHandler openMenuInputted;


        public event Action<InputEventContext> uiSubmitInputted;
        public event Action<InputEventContext> uiCancelInputted;


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
            _playerInputActions.Player.Interact.performed += ctx => interactInputted?.Invoke(inputEventContext);
            _playerInputActions.UI.Submit.performed += ctx => uiSubmitInputted?.Invoke(inputEventContext);
            _playerInputActions.UI.Cancel.performed += ctx => uiCancelInputted?.Invoke(inputEventContext);
            _playerInputActions.UI.Disable();
        }
        public void DisableOpenMenuInput()
        {
            _playerInputActions.Player.OpenMenu.Disable();
        }

        public void EnableOpenMenuInput()
        {
            _playerInputActions.Player.OpenMenu.Enable();
        }

        public void SetInputEventContext(InputEventContext context)
        {
            inputEventContext = context;
            switch (context)
            {
                case InputEventContext.DEFAULT:
                    SwitchToPlayerControls();
                    break;
                case InputEventContext.DIALOGUE:
                    SwitchToDialogueControls();
                    break;
            }
        }

        private void SwitchToPlayerControls()
        {
            _playerInputActions.UI.Disable();
            _playerInputActions.Player.Enable();
        }

        private void SwitchToDialogueControls()
        {
            _playerInputActions.Player.Disable();
            _playerInputActions.UI.Enable();
        }

        public void DisablePlayerControls()
        {
            _playerInputActions.Player.Disable();
        }

        public void EnablePlayerControls()
        {
            _playerInputActions.Player.Enable();
        }


        public void Dispose()
        {
            _playerInputActions.Disable();
        }

        public Vector2 GetMovementVector()
        {
            Vector2 movement = _playerInputActions.Player.Move.ReadValue<Vector2>();
            return movement;
        }
    }
}