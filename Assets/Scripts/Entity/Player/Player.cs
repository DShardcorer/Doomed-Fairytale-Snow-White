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



    //Attacking
    private PlayerAttackingState _playerAttackingState;
    public PlayerAttackingState PlayerAttackingState => _playerAttackingState;

    public Player(PlayerView view, PlayerProperties properties,
 PlayerIdlingState playerIdlingState, PlayerMovingState playerMovingState, PlayerAttackingState playerAttackingState,
 SkillSystem skillSystem, EntityStateMachine stateMachine) : base(view, properties, skillSystem, stateMachine)
    {
        _playerView = view;
        _playerProperties = properties;
        _playerIdlingState = playerIdlingState;
        _playerMovingState = playerMovingState;
        _playerAttackingState = playerAttackingState;
    }
    public void Initialize(PlayerManager parent)
    {
        base.Initialize();
        //Getting references
        _parent = parent;
        _inputManager = _parent.GameManager.InputManager;
        _inputManager.dashInputted += OnDashInputted;
        _inputManager.attackInputted += OnAttackInputted;
        _parent.GameManager.CameraManager.SetFollowTarget(_playerView.transform);


        //Add to update managers
        _parent.GameManager.UpdateManager.AddUpdatable(this);
        _parent.GameManager.FixedUpdateManager.AddFixedUpdatable(this);


        //View initialization
        _view.Initialize(this);


        //State creation

        _playerIdlingState.Initialize(this);
        _playerMovingState.Initialize(this);
        _playerAttackingState.Initialize(this);


        _stateMachine.Initialize(_playerIdlingState);



        //SkillSystem initialization
        _skillSystem.Initialize(this);

    }

    private void OnAttackInputted(object sender, EventArgs e)
    {
        if (IsBusy) return;
        _stateMachine.ChangeState(_playerAttackingState);
    }

    private void OnDashInputted(object sender, EventArgs e)
    {
        if (IsBusy) return;
        if (_skillSystem.GetSkill(SkillNameHelper.DashSkill).TryUseSkill())
        {
        }


    }

    public override void FixedUpdateLogic()
    {
        base.FixedUpdateLogic();
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
