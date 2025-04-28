using System.Collections.Generic;
using Entity.Faction;
using Entity.Player.Idle;
using Entity.Player.Move;
using EntitySystems.PlayerSystems;
using EntitySystems.Skill;
using EntitySystems.Skill.ActiveSkills.Player.Attack;
using EntitySystems.Skill.ActiveSkills.Player.Dash;
using EntitySystems.Skill.ActiveSkills.Player.Shoot;
using EntitySystems.Skill.PassiveSkills;
using EntitySystems.Skill.SkillFactory;
using EntitySystems.States.Movement;
using EntitySystems.Stats;
using GeneralManagers;
using Helpers;
using UnityEngine;

namespace Entity.Player
{
    public class PlayerManager : MonoBehaviour, ILifecycle<GameManager>
    {
        private GameManager _gameManager;
        private Player _player;
        public Player Player => _player;

        public GameManager GameManager => _gameManager;

        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private AbilityStatboardSO _abilityStatboardSO;


        public void Initialize(GameManager gameManager)
        {
            _gameManager = gameManager;
            CreatePlayer(new Vector3(0,0,0));
        }

        public void CreatePlayer(Vector3 position)
        {
            GameObject playerGameObject = Instantiate(_playerPrefab, position, Quaternion.identity);

            //PlayerView creation
            PlayerView playerView = playerGameObject.GetComponent<PlayerView>();
            DontDestroyOnLoad(playerView);


            //ActiveSkillSystem creation
            DashActiveSkill dashActiveSkill = SkillRegistry.CreateActiveSkill("Dash") as DashActiveSkill;
            ShootActiveSkill shootActiveSkill = SkillRegistry.CreateActiveSkill("Shoot") as ShootActiveSkill;

            PlayerActiveSkillSystem activeSkillSystem =
                new PlayerActiveSkillSystem(new List<ActiveSkill> { dashActiveSkill, shootActiveSkill });


            NaturalStrengthPassiveSkill naturalStrengthPassiveSkill =
                SkillRegistry.CreatePassiveSkill("Natural Strength") as NaturalStrengthPassiveSkill;
            BodyControlPassiveSkill bodyControlPassiveSkill =
                SkillRegistry.CreatePassiveSkill("Body Control") as BodyControlPassiveSkill;
            List<PassiveSkill> passiveSkills = new List<PassiveSkill>
                { naturalStrengthPassiveSkill, bodyControlPassiveSkill };
            PlayerPassiveSkillSystem passiveSkillSystem = new PlayerPassiveSkillSystem(passiveSkills);

            //States creation
            PlayerIdlingProperties playerIdlingProperties = new PlayerIdlingProperties();
            PlayerIdleState playerIdleState =
                new PlayerIdleState(HelperAnimationStateName.IS_IDLING, playerIdlingProperties);


            PlayerMovingProperties playerMovingProperties = new PlayerMovingProperties();
            PlayerMoveState playerMoveState =
                new PlayerMoveState(HelperAnimationStateName.IS_MOVING, playerMovingProperties);

            PlayerAttackingProperties playerAttackingProperties = new PlayerAttackingProperties();
            PlayerAttackState playerAttackState =
                new PlayerAttackState(HelperAnimationStateName.IS_ATTACKING, playerAttackingProperties);

            EntityStateMachine stateMachine = new EntityStateMachine();

            //Stat system creation
            AbilityStatBoard abilityStatBoard = new AbilityStatBoard(_abilityStatboardSO);
            PlayerStatSystem statSystem = new PlayerStatSystem(abilityStatBoard, AttackStatType.Strength);

            //Equipment system creation
            PlayerEquipmentSystem equipmentSystem = new PlayerEquipmentSystem();


            //PlayerProperties creation
            PlayerProperties playerProperties =
                new PlayerProperties(EntityFaction.Player, statSystem.CombatStatBoard.Health.BaseValue);

            //LevelSystem creation
            PlayerLevelSystem levelSystem = new PlayerLevelSystem();

            //HealthSystem creation (convert health to int)
            PlayerHealthSystem healthSystem =
                new PlayerHealthSystem((int)statSystem.CombatStatBoard.Health.ModifiedValue);

            //ManaSystem creation
            PlayerManaSystem manaSystem = new PlayerManaSystem((int)statSystem.CombatStatBoard.Mana.ModifiedValue);

            //StaminaSystem creation
            PlayerStaminaSystem staminaSystem =
                new PlayerStaminaSystem((int)statSystem.CombatStatBoard.Stamina.ModifiedValue);

            PlayerInventorySystem inventory = new PlayerInventorySystem();

            _player = new Player(
                playerView, playerProperties, playerIdleState, playerMoveState, playerAttackState,
                statSystem, equipmentSystem,
                activeSkillSystem, passiveSkillSystem,
                levelSystem, healthSystem, manaSystem, staminaSystem,
                stateMachine, inventory);
            _player.Initialize(this);
        }


        public void Dispose()
        {
            _player = null;
        }
    }
}