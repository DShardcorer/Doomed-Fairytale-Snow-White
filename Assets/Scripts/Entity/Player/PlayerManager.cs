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
        PlayerDashingState playerDashingState = new PlayerDashingState(AnimationStateHelper.IS_DASHING, playerDashingProperties);
        DashSkill dashSkill = new DashSkill(SkillNameHelper.DashSkill, 2f, playerDashingState);
        

        PlayerShootingProperties playerShootingProperties = new PlayerShootingProperties();
        PlayerShootingState playerShootingState = new PlayerShootingState(AnimationStateHelper.IS_SHOOTING, playerShootingProperties);
        ShootSkill shootSkill = new ShootSkill(SkillNameHelper.ShootSkill, 2f, playerShootingState);
        
        SkillSystem skillSystem = new SkillSystem(new List<Skill> { dashSkill, shootSkill });



        //States creation
        PlayerIdlingProperties playerIdlingProperties = new PlayerIdlingProperties();
        PlayerIdlingState playerIdlingState = new PlayerIdlingState(AnimationStateHelper.IS_IDLING, playerIdlingProperties);


        PlayerMovingProperties playerMovingProperties = new PlayerMovingProperties();
        PlayerMovingState playerMovingState = new PlayerMovingState(AnimationStateHelper.IS_MOVING, playerMovingProperties);





        PlayerAttackingProperties playerAttackingProperties = new PlayerAttackingProperties();
        PlayerAttackingState playerAttackingState = new PlayerAttackingState(AnimationStateHelper.IS_ATTACKING, playerAttackingProperties);

        EntityStateMachine stateMachine = new EntityStateMachine();

        //Stat system creation
        AbilityStatBoard abilityStatBoard = new AbilityStatBoard(_abilityStatboardSO);
        StatSystem statSystem = new StatSystem(abilityStatBoard, AttackStatType.Strength);


        //PlayerProperties creation
        PlayerProperties playerProperties = new PlayerProperties(EntityFaction.Player, statSystem.CombatStatBoard.Health.BaseValue);

        _player = new Player(playerView, playerProperties, playerIdlingState, playerMovingState, playerAttackingState,statSystem, skillSystem, stateMachine);
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
