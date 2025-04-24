using DataPersistence;
using Entity.Player;
using GeneralManagers;
using SceneSwitch;
using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    private void Start()
    {
        // Ensure GameManager and its components are initialized first.
        GameManager.Instance.Initialize();

        // Then initialize UIManager or notify UI components.
        UIManager.Instance.Initialize();
        InvokeInitialEvents();
        
        DataPersistenceManager.Instance.Initialize();
        SceneSwitchManager.Instance.Initialize();

    }
    private void InvokeInitialEvents()
    {
        Player player = GameManager.Instance.PlayerManager.GetPlayer();
        player.InventorySystem.InvokeInitialEvents();
        player.HealthSystem.InvokeInitialEvents();
        player.ManaSystem.InvokeInitialEvents();
        player.StaminaSystem.InvokeInitialEvents();
        player.StatSystem.InvokeInitialEvents();
        player.EquipmentSystem.InvokeInitialEvents();
        player.LevelSystem.InvokeInitialEvents();
        player.ActiveSkillSystem.InvokeInitialEvents();
        player.PassiveSkillSystem.InvokeInitialEvents();
        
        
        GameManager.Instance.GameTimeManager.InvokeInitialEvents();
    }
}
