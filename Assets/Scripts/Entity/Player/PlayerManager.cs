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

        public GameManager GameManager => _gameManager;

        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private AbilityStatboardSO _abilityStatboardSO;


        public void Initialize(GameManager gameManager)
        {
            _gameManager = gameManager;
            CreatePlayer();
        }

        public void CreatePlayer()
        {
            GameObject playerGameObject = Instantiate(_playerPrefab);

            //PlayerView creation
            PlayerView playerView = playerGameObject.GetComponent<PlayerView>();
            DontDestroyOnLoad(playerView);


            //ActiveSkillSystem creation
            PlayerDashingProperties playerDashingProperties = new PlayerDashingProperties();
            PlayerDashingState playerDashingState =
                new PlayerDashingState(HelperAnimationStateName.IS_DASHING, playerDashingProperties);
            DashActiveSkill dashActiveSkill =
                new DashActiveSkill(HelperSkillName.DashSkill, 2f, 0, 0, 30, playerDashingState);


            PlayerShootingProperties playerShootingProperties = new PlayerShootingProperties();
            PlayerShootingState playerShootingState =
                new PlayerShootingState(HelperAnimationStateName.IS_SHOOTING, playerShootingProperties);
            ShootActiveSkill shootActiveSkill =
                new ShootActiveSkill(HelperSkillName.ShootSkill, 2f, 0, 10, 30, playerShootingState);

            ActiveSkillSystem activeSkillSystem =
                new ActiveSkillSystem(new List<ActiveSkill> { dashActiveSkill, shootActiveSkill });

            SkillInfoSO naturalStrengthSO =
                UnityEngine.Resources.Load<SkillInfoSO>(HelperResourcePath.PassiveSkillPath + "/NaturalStrength");
            NaturalStrengthPassiveSkill naturalStrengthPassiveSkill =
                new NaturalStrengthPassiveSkill(naturalStrengthSO);

            List<PassiveSkill> passiveSkills = new List<PassiveSkill> { naturalStrengthPassiveSkill };
            PassiveSkillSystem passiveSkillSystem = new PassiveSkillSystem(passiveSkills);

            //States creation
            PlayerIdlingProperties playerIdlingProperties = new PlayerIdlingProperties();
            PlayerIdlingState playerIdlingState =
                new PlayerIdlingState(HelperAnimationStateName.IS_IDLING, playerIdlingProperties);


            PlayerMovingProperties playerMovingProperties = new PlayerMovingProperties();
            PlayerMovingState playerMovingState =
                new PlayerMovingState(HelperAnimationStateName.IS_MOVING, playerMovingProperties);

            PlayerAttackingProperties playerAttackingProperties = new PlayerAttackingProperties();
            PlayerAttackingState playerAttackingState =
                new PlayerAttackingState(HelperAnimationStateName.IS_ATTACKING, playerAttackingProperties);

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
                playerView, playerProperties, playerIdlingState, playerMovingState, playerAttackingState,
                statSystem, equipmentSystem,
                activeSkillSystem, passiveSkillSystem,
                levelSystem, healthSystem, manaSystem, staminaSystem,
                stateMachine, inventory);
            _player.Initialize(this);
        }

        public Player GetPlayer()
        {
            return _player;
        }

        public void Dispose()
        {
            _player = null;
        }
    }
}