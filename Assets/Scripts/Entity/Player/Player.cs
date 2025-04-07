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

    private PlayerInteraction _playerInteraction;
    public PlayerInteraction PlayerInteraction => _playerInteraction;



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
 PlayerStatSystem statSystem, PlayerEquipmentSystem equipmentSystem,
  SkillSystem skillSystem, PlayerLevelSystem levelSystem, PlayerHealthSystem healthSystem, PlayerManaSystem manaSystem, PlayerStaminaSystem staminaSystem,
 EntityStateMachine stateMachine, PlayerInventorySystem inventory)
 : base(view, properties, statSystem, equipmentSystem,
 skillSystem, levelSystem, healthSystem, manaSystem, staminaSystem, stateMachine, inventory)
    {
        _playerView = view;
        _playerProperties = properties;
        _playerIdlingState = playerIdlingState;
        _playerMovingState = playerMovingState;
        _playerAttackingState = playerAttackingState;
        _playerInteraction = view.GetComponentInChildren<PlayerInteraction>();
    }
    public void Initialize(PlayerManager parent)
    {
        base.Initialize();
        //Getting references
        _parent = parent;
        _inputManager = _parent.GameManager.InputManager;
        _inputManager.dashInputted += OnDashInputted;
        _inputManager.attackInputted += OnAttackInputted;
        _inputManager.skill1Inputted += OnSkill1Inputted;
        _parent.GameManager.CameraManager.SetFollowTarget(_playerView.transform);


        //Add to update managers
        _parent.GameManager.UpdateManager.AddUpdatable(this);
        _parent.GameManager.FixedUpdateManager.AddFixedUpdatable(this);


        //View initialization
        _playerView.Initialize(this);
        _playerInteraction.Initialize(this);


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
        _skillSystem.GetSkill(HelperSkillName.DashSkill).TryUseSkill();

    }
    private void OnSkill1Inputted(object sender, EventArgs e)
    {
        if (IsBusy) return;
        _skillSystem.GetSkill(HelperSkillName.ShootSkill).TryUseSkill();
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


    public override void Dispose()
    {
        base.Dispose();
        _inputManager.dashInputted -= OnDashInputted;
        _inputManager.attackInputted -= OnAttackInputted;
        _inputManager.skill1Inputted -= OnSkill1Inputted;
        _parent.GameManager.UpdateManager.RemoveUpdatable(this);
        _parent.GameManager.FixedUpdateManager.RemoveFixedUpdatable(this);
        _playerView = null;
        _playerProperties = null;
        _playerIdlingState = null;
        _playerMovingState = null;
        _playerAttackingState = null;
        _playerInteraction = null;
        _inputManager = null;
        _parent = null;
    }

    public override void Die()
    {
        Debug.Log("Player died");
    }
}
