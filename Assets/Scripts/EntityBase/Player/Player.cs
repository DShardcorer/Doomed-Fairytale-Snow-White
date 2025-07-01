using System;
using System.Collections.Generic;
using EntityBase.Player.State;
using EntityBase.Player.Idle;
using EntityBase.Player.Interaction;
using EntityBase.Player.Move;
using EntitySystems.PlayerSystems;
using EntitySystems.Skill;
using EntitySystems.Skill.ActiveSkills.Player.Attack;
using EntitySystems.WeaponSystem;
using GeneralManagers;
using Helpers;
using Input;
using UnityEngine;

namespace EntityBase.Player
{
    // Define player state enum
    public enum PlayerStateType
    {
        Idle,
        Move,

        Attack
        // Add additional states as needed
    }

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

        // Dictionary to store states with enum identifiers
        private Dictionary<PlayerStateType, EntityState> _states = new Dictionary<PlayerStateType, EntityState>();

        // Track current state type
        private PlayerStateType _currentStateType;
        public PlayerStateType CurrentStateType => _currentStateType;

        private PlayerEquippedSkillSystem _equippedSkillSystem;
        public PlayerEquippedSkillSystem EquippedSkillSystem => _equippedSkillSystem;


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
            PlayerBuffSystem buffSystem, PlayerEquippedSkillSystem equippedSkillSystem, 
            WeaponSystem weaponSystem)
            : base(
                profile,
                view, properties, statSystem, equipmentSystem,
                activeSkillSystem, passiveSkillSystem, levelSystem,
                healthSystem, manaSystem, staminaSystem,
                stateMachine, inventory, buffSystem, weaponSystem)
        {
            _equippedSkillSystem = equippedSkillSystem;
            _playerView = view;
            _playerProperties = properties;
            IdleState = playerIdleState;
            _playerInteraction = view.GetComponentInChildren<PlayerInteraction>();

            // Store states in dictionary using enum keys
            _states[PlayerStateType.Idle] = playerIdleState;
            _states[PlayerStateType.Move] = playerMoveState;
            _states[PlayerStateType.Attack] = playerAttackState;
        }

        // State access methods
        public void AddState(PlayerStateType stateType, EntityState state)
        {
            if (!_states.ContainsKey(stateType))
            {
                _states.Add(stateType, state);
            }
            else
            {
                Debug.LogWarning($"State with type {stateType} already exists.");
            }
        }

        public EntityState GetState(PlayerStateType stateType)
        {
            if (_states.TryGetValue(stateType, out var state))
            {
                return state;
            }

            Debug.LogWarning($"State with type {stateType} does not exist.");
            return null;
        }

        public void ChangeState(PlayerStateType stateType)
        {
            if (_states.TryGetValue(stateType, out var state))
            {
                stateMachine.ChangeState(state);
                _currentStateType = stateType;
            }
            else
            {
                Debug.LogError($"Cannot change to state with type {stateType} as it does not exist.");
            }
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

            //Add to update managers
            _parent.GameManager.UpdateManager.AddUpdatable(this);
            _parent.GameManager.FixedUpdateManager.AddFixedUpdatable(this);

            //View initialization
            _playerView.Initialize(this);
            _playerInteraction.Initialize(this);

            //State initialization
            foreach (var state in _states.Values)
            {
                if (state is PlayerState playerState)
                {
                    playerState.Initialize(this);
                }
            }

            // Initialize with the idle state
            _currentStateType = PlayerStateType.Idle;
            stateMachine.Initialize(_states[PlayerStateType.Idle]);

            //SkillSystem initialization
            activeSkillSystem.Initialize(this);
        }

        private void OnAttackInputted(object sender, EventArgs e)
        {
            if (IsBusy) return;
            ChangeState(PlayerStateType.Attack);
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

        public override bool IsAttacking()
        {
            return _currentStateType == PlayerStateType.Attack;
        }

        public override int CurrentAttackCounter()
        {
            if (_states.TryGetValue(PlayerStateType.Attack, out var state) && state is PlayerAttackState attackState)
            {
                return attackState.CurrentAttackCounter;
            }

            return 0;
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
            _playerInteraction = null;
            _inputManager = null;
            _parent = null;
            _states.Clear();
        }

        public override void Die()
        {
            Debug.Log("Player died");
        }
    }
}