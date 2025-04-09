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

    }
    private void InvokeInitialEvents()
    {
        GameManager.Instance.PlayerManager.GetPlayer().InventorySystem.InvokeInitialEvents();
        GameManager.Instance.PlayerManager.GetPlayer().HealthSystem.InvokeInitialEvents();
        GameManager.Instance.PlayerManager.GetPlayer().ManaSystem.InvokeInitialEvents();
        GameManager.Instance.PlayerManager.GetPlayer().StaminaSystem.InvokeInitialEvents();
        GameManager.Instance.PlayerManager.GetPlayer().StatSystem.InvokeInitialEvents();
        GameManager.Instance.PlayerManager.GetPlayer().EquipmentSystem.InvokeInitialEvents();
        GameManager.Instance.PlayerManager.GetPlayer().LevelSystem.InvokeInitialEvents();
    }
}
