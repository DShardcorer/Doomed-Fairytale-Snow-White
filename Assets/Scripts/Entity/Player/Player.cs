using System;
using Entity.Player.Idle;
using Entity.Player.Interaction;
using Entity.Player.Move;
using Entity.Player.State;
using EntitySystems.PlayerSystems;
using EntitySystems.Skill;
using EntitySystems.Skill.ActiveSkills.Player.Attack;
using GeneralManagers;
using Helpers;
using Input;
using UnityEngine;

namespace Entity.Player
{
    public class Player : Entity, ILifecycle<PlayerManager>, IFixedUpdatable, IUpdatable
    {
        private PlayerManager _parent;
        private InputManager _inputManager;

        public InputManager InputManager => _inputManager;

        //
        // private PlayerProfile _profile;
        // public PlayerProfile Profile => _profile;
        private PlayerView _playerView;
        public PlayerView PlayerView => _playerView;

        private PlayerProperties _playerProperties;
        public PlayerProperties PlayerProperties => _playerProperties;

        private PlayerInteraction _playerInteraction;
        public PlayerInteraction PlayerInteraction => _playerInteraction;


        //Idling
        private PlayerIdleState _playerIdleState;
        public PlayerIdleState PlayerIdleState => _playerIdleState;

        //Moving
        private PlayerMoveState _playerMoveState;
        public PlayerMoveState PlayerMoveState => _playerMoveState;


        //Attacking
        private PlayerAttackState _playerAttackState;
        public PlayerAttackState PlayerAttackState => _playerAttackState;

        public Player(
            PlayerProfile profile,
            PlayerView view, PlayerProperties properties,
            PlayerIdleState playerIdleState, PlayerMoveState playerMoveState,
            PlayerAttackState playerAttackState,
            PlayerStatSystem statSystem, PlayerEquipmentSystem equipmentSystem,
            ActiveSkillSystem activeSkillSystem, PassiveSkillSystem passiveSkillSystem,
            PlayerLevelSystem levelSystem, PlayerHealthSystem healthSystem,
            PlayerManaSystem manaSystem, PlayerStaminaSystem staminaSystem,
            EntityStateMachine stateMachine, PlayerInventorySystem inventory, 
            PlayerBuffSystem buffSystem)
            : base(
                profile,
                view, properties, statSystem, equipmentSystem,
                activeSkillSystem, passiveSkillSystem, levelSystem, 
                healthSystem, manaSystem, staminaSystem,
                stateMachine, inventory, buffSystem)
        {
            _playerView = view;
            _playerProperties = properties;
            _playerIdleState = playerIdleState;
            // _profile = profile;
            IdleState = playerIdleState;
            _playerMoveState = playerMoveState;
            _playerAttackState = playerAttackState;
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
            // _parent.GameManager.CameraManager.SetFollowTarget(_playerView.transform);


            //Add to update managers
            _parent.GameManager.UpdateManager.AddUpdatable(this);
            _parent.GameManager.FixedUpdateManager.AddFixedUpdatable(this);


            //View initialization
            _playerView.Initialize(this);
            _playerInteraction.Initialize(this);


            //State creation
            _playerIdleState.Initialize(this);
            _playerMoveState.Initialize(this);
            _playerAttackState.Initialize(this);


            stateMachine.Initialize(IdleState);


            //SkillSystem initialization
            activeSkillSystem.Initialize(this);
        }


        private void OnAttackInputted(object sender, EventArgs e)
        {
            if (IsBusy) return;
            stateMachine.ChangeState(_playerAttackState);
        }

        private void OnDashInputted(object sender, EventArgs e)
        {
            if (IsBusy) return;
            activeSkillSystem.GetSkill(HelperSkillName.DashSkill).TryUseSkill();
        }

        private void OnSkill1Inputted(object sender, EventArgs e)
        {
            if (IsBusy) return;
            activeSkillSystem.GetSkill(HelperSkillName.ShootSkill).TryUseSkill();
        }

        public override void FixedUpdateLogic()
        {
            base.FixedUpdateLogic();
            stateMachine.FixedUpdateLogic();
        }

        public void UpdateLogic()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                LevelSystem.AddExperience(50);
            }

            stateMachine.UpdateLogic();
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
            IdleState = null;
            _playerMoveState = null;
            _playerAttackState = null;
            _playerInteraction = null;
            _inputManager = null;
            _parent = null;

            //Garbage collector hoat dong
            // different flavor of wrong
        }

        public override void Die()
        {
            Debug.Log("Player died");
        }
    }
}