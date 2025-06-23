using System.Collections.Generic;
using EntityBase.Faction;
using EntityBase.Player.Idle;
using EntityBase.Player.Move;
using EntitySystems.PlayerSystems;
using EntitySystems.Skill;
using EntitySystems.Skill.ActiveSkills.Player.Attack;
using EntitySystems.Skill.ActiveSkills.Player.Dash;
using EntitySystems.Skill.ActiveSkills.Player.Shoot;
using EntitySystems.Skill.PassiveSkills;
using EntitySystems.Skill.SkillRegistry;
using EntitySystems.States.Movement;
using EntitySystems.Stats;
using EventBus.Player;
using GeneralManagers;
using Helpers;
using Item;
using Item.Inventory;
using SceneSwitch;
using UnityEngine;

namespace EntityBase.Player
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
            //Find object by name
            GameObject spawnPortal = GameObject.Find("Portal_Spawn");
            if (spawnPortal != null)
            {
                Vector3 spawnPosition = spawnPortal.transform.position;
                CreatePlayer(spawnPosition);
            }
            else
            {
                Debug.LogWarning("Spawn portal not found, creating player at default position.");
                CreatePlayer(new Vector3(0, 0, 0));
            }
        }

        public void CreatePlayer(Vector3 position)
        {
            GameObject playerGameObject = Instantiate(_playerPrefab, position, Quaternion.identity);

            //PlayerView creation
            PlayerView playerView = playerGameObject.GetComponent<PlayerView>();
            PlayerProfile playerProfile = new PlayerProfile("Player", "The main player character");
            DontDestroyOnLoad(playerView);
            //ActiveSkillSystem creation
            DashActiveSkill dashActiveSkill =
                SkillRegistry.CreateActiveSkill(HelperSkillName.DashSkill) as DashActiveSkill;
            ShootActiveSkill shootActiveSkill =
                SkillRegistry.CreateActiveSkill(HelperSkillName.ShootSkill) as ShootActiveSkill;


            PlayerActiveSkillSystem activeSkillSystem = new PlayerActiveSkillSystem(new List<ActiveSkill>
                {
                    dashActiveSkill,
                    shootActiveSkill
                }
            );

            PlayerPassiveSkillSystem passiveSkillSystem = new PlayerPassiveSkillSystem(new List<PassiveSkill>
            {
                // SkillRegistry.CreatePassiveSkill(HelperSkillName.NaturalStrength),
                // SkillRegistry.CreatePassiveSkill(HelperSkillName.Flirt),
                // SkillRegistry.CreatePassiveSkill(HelperSkillName.PerceptiveEye)
            });


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
            PlayerBuffSystem buffSystem = new PlayerBuffSystem();

            PlayerEquippedSkillSystem playerEquippedSkillSystem = new PlayerEquippedSkillSystem();

            _player = new Player(playerProfile,
                playerView, playerProperties,
                playerIdleState, playerMoveState, playerAttackState,
                statSystem, equipmentSystem,
                activeSkillSystem, passiveSkillSystem,
                levelSystem, healthSystem, manaSystem, staminaSystem,
                stateMachine, inventory, buffSystem, playerEquippedSkillSystem);
            _player.Initialize(this);
        }

        public void Dispose()
        {
            _player = null;
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.P))
            {
                PassiveSkill skill = SkillRegistry.CreatePassiveSkill(HelperSkillName.NaturalStrength);
                AddPassiveSkill(skill);
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.O))
            {
                _player.LevelSystem.AddExperience(50);
            }
        }

        #region Player Utility API

        public void DisablePlayer()
        {
            _player.PlayerView.gameObject.SetActive(false);
        }

        public void EnablePlayer()
        {
            _player.PlayerView.gameObject.SetActive(true);
        }

        public void SetPlayerStat(StatType statType, int value)
        {
            _player.StatSystem.SetAbilityStatPoints(statType, value);
        }

        public void AddPlayerStat(StatType statType, int amount)
        {
            PlayerStatsEventSystem.InvokeStatPointGained(statType, amount);
            _player.StatSystem.AddAbilityStatPoints(statType, amount);
        }

        public void AddItemToInventory(ItemDataSO itemDataSo, int amount)
        {
            _player.InventorySystem.AddItem(itemDataSo, amount);
        }

        public void RemoveItemFromInventory(ItemDataSO itemDataSo, int amount)
        {
            _player.InventorySystem.RemoveItem(itemDataSo, amount);
        }

        public void AddActiveSkill(ActiveSkill skill)
        {
            _player.ActiveSkillSystem.AddSkill(skill);
        }

        public void AddPassiveSkill(PassiveSkill skill)
        {
            _player.PassiveSkillSystem.AddSkill(skill);
        }

        public void SetPlayerName(string name)
        {
            _player.Profile.SetName(name);
        }

        public void SetPlayerPosition(Vector3 position)
        {
            _player.PlayerView.transform.position = position;
        }

        #endregion
    }
}