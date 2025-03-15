using System;
using UnityEngine;

public class Player : Entity, ILifecycle<PlayerManager>, IFixedUpdatable, IUpdatable
{
    private PlayerManager _parent;
    private InputManager _inputManager;
    public InputManager InputManager => _inputManager;

    private PlayerView _playerView;
    public PlayerView PlayerView => _playerView;

    private PlayerProperties _playerProperties;
    public PlayerProperties PlayerProperties => _playerProperties;



    public bool IsBusy = false;


    //Idling
    private PlayerIdlingState _playerIdlingState;
    public PlayerIdlingState PlayerIdlingState => _playerIdlingState;

    //Moving
    private PlayerMovingState _playerMovingState;
    public PlayerMovingState PlayerMovingState => _playerMovingState;
    private PlayerMovingProperties _playerMovingProperties;
    public PlayerMovingProperties PlayerMovingProperties => _playerMovingProperties;


    //Dashing
    private PlayerDashingState _playerDashingState;
    public PlayerDashingState PlayerDashingState => _playerDashingState;
    private PlayerDashingProperties _playerDashingProperties;
    public PlayerDashingProperties PlayerDashingProperties => _playerDashingProperties;


    //Attacking
    private PlayerAttackingState _playerAttackingState;
    public PlayerAttackingState PlayerAttackingState => _playerAttackingState;
    private PlayerAttackingProperties _playerAttackingProperties;
    public PlayerAttackingProperties PlayerAttackingProperties => _playerAttackingProperties;


    public Player(PlayerView view, PlayerProperties properties,
     PlayerIdlingState playerIdlingState, PlayerMovingState playerMovingState,
     PlayerDashingState playerDashingState, PlayerAttackingState playerAttackingState ) : base(view, properties)
    {
        _playerView = view;
        _playerProperties = properties;
        _playerIdlingState = playerIdlingState;
        _playerMovingState = playerMovingState;
        _playerDashingState = playerDashingState;
        _playerAttackingState = playerAttackingState;
    }
    public void Initialize(PlayerManager parent)
    {
        //Getting references
        _parent = parent;
        _inputManager = _parent.GameManager.InputManager;
        _inputManager.dashInputted += OnDashInputted;
        _inputManager.attackInputted += OnAttackInputted;


        //Add to update managers
        _parent.GameManager.UpdateManager.AddUpdatable(this);
        _parent.GameManager.FixedUpdateManager.AddFixedUpdatable(this);


        //View initialization
        _view.Initialize(this);


        //State creation
        
         _playerIdlingState.Initialize(this);
         _playerMovingState.Initialize(this);
         _playerDashingState.Initialize(this);
         _playerAttackingState.Initialize(this);

 
        _stateMachine.Initialize(_playerIdlingState);

    }

    private void OnAttackInputted(object sender, EventArgs e)
    {
        if (IsBusy) return;
        _stateMachine.ChangeState(_playerAttackingState);
    }

    private void OnDashInputted(object sender, EventArgs e)
    {
        if (IsBusy) return;
        _stateMachine.ChangeState(_playerDashingState);
    }

    public void FixedUpdateLogic()
    {

        _stateMachine.FixedUpdateLogic();
    }

    public void UpdateLogic()
    {
        _stateMachine.UpdateLogic();
    }


    public void Dispose()
    {
        _parent = null;
    }




}
