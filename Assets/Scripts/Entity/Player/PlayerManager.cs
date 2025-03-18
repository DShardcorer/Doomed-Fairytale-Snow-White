using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, ILifecycle<GameManager>
{
    private GameManager _gameManager;
    private Player _player;

    public GameManager GameManager => _gameManager;

    [SerializeField] private GameObject _playerPrefab;

    [SerializeField] private PlayerPropertiesSO _playerPropertiesSO;



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


        //PlayerProperties creation
        PlayerProperties playerProperties = new PlayerProperties(_playerPropertiesSO);

        //SkillSystem creation
        PlayerDashingProperties playerDashingProperties = new PlayerDashingProperties();
        PlayerDashingState playerDashingState = new PlayerDashingState(playerDashingProperties, AnimationStateHelper.IS_DASHING);
        DashSkill dashSkill = new DashSkill(SkillNameHelper.DashSkill, 2f, playerDashingState);
        SkillSystem skillSystem = new SkillSystem(new List<Skill> { dashSkill });




        //States creation
        PlayerIdlingState playerIdlingState = new PlayerIdlingState(AnimationStateHelper.IS_IDLING);


        PlayerMovingProperties playerMovingProperties = new PlayerMovingProperties();
        PlayerMovingState playerMovingState = new PlayerMovingState(playerMovingProperties, AnimationStateHelper.IS_MOVING);





        PlayerAttackingProperties playerAttackingProperties = new PlayerAttackingProperties();
        PlayerAttackingState playerAttackingState = new PlayerAttackingState(playerAttackingProperties, AnimationStateHelper.IS_ATTACKING);

        EntityStateMachine stateMachine = new EntityStateMachine();

        _player = new Player(playerView, playerProperties, playerIdlingState, playerMovingState, playerAttackingState, skillSystem, stateMachine);
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
