using System.Collections.Generic;
using UnityEngine;

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




        //SkillSystem creation
        PlayerDashingProperties playerDashingProperties = new PlayerDashingProperties();
        PlayerDashingState playerDashingState = new PlayerDashingState(HelperAnimationStateName.IS_DASHING, playerDashingProperties);
        DashSkill dashSkill = new DashSkill(HelperSkillName.DashSkill, 2f, 0, 0, 30, playerDashingState);
        

        PlayerShootingProperties playerShootingProperties = new PlayerShootingProperties();
        PlayerShootingState playerShootingState = new PlayerShootingState(HelperAnimationStateName.IS_SHOOTING, playerShootingProperties);
        ShootSkill shootSkill = new ShootSkill(HelperSkillName.ShootSkill, 2f, 0, 10, 30, playerShootingState);
        
        SkillSystem skillSystem = new SkillSystem(new List<Skill> { dashSkill, shootSkill });



        //States creation
        PlayerIdlingProperties playerIdlingProperties = new PlayerIdlingProperties();
        PlayerIdlingState playerIdlingState = new PlayerIdlingState(HelperAnimationStateName.IS_IDLING, playerIdlingProperties);


        PlayerMovingProperties playerMovingProperties = new PlayerMovingProperties();
        PlayerMovingState playerMovingState = new PlayerMovingState(HelperAnimationStateName.IS_MOVING, playerMovingProperties);

        PlayerAttackingProperties playerAttackingProperties = new PlayerAttackingProperties();
        PlayerAttackingState playerAttackingState = new PlayerAttackingState(HelperAnimationStateName.IS_ATTACKING, playerAttackingProperties);

        EntityStateMachine stateMachine = new EntityStateMachine();

        //Stat system creation
        AbilityStatBoard abilityStatBoard = new AbilityStatBoard(_abilityStatboardSO);
        PlayerStatSystem statSystem = new PlayerStatSystem(abilityStatBoard, AttackStatType.Strength);

        //Equipment system creation
        PlayerEquipmentSystem equipmentSystem = new PlayerEquipmentSystem();


        //PlayerProperties creation
        PlayerProperties playerProperties = new PlayerProperties(EntityFaction.Player, statSystem.CombatStatBoard.Health.BaseValue);

        //LevelSystem creation
        PlayerLevelSystem levelSystem = new PlayerLevelSystem();

        //HealthSystem creation (convert health to int)
        PlayerHealthSystem healthSystem = new PlayerHealthSystem((int)statSystem.CombatStatBoard.Health.ModifiedValue);

        //ManaSystem creation
        PlayerManaSystem manaSystem = new PlayerManaSystem((int)statSystem.CombatStatBoard.Mana.ModifiedValue);

        //StaminaSystem creation
        PlayerStaminaSystem staminaSystem = new PlayerStaminaSystem((int)statSystem.CombatStatBoard.Stamina.ModifiedValue);

        PlayerInventorySystem inventory = new PlayerInventorySystem();

        _player = new Player(playerView, playerProperties, playerIdlingState, playerMovingState, playerAttackingState,
         statSystem,equipmentSystem,
          skillSystem, levelSystem, healthSystem, manaSystem, staminaSystem, stateMachine, inventory);
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
