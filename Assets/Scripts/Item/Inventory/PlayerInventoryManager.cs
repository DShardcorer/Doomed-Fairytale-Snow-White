using System;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour, ILifecycle<GameManager>
{
    private GameManager _manager;
    public GameManager Manager => _manager;
    public Inventory playerInventory;
    public PlayerInventoryUI playerInventoryUI;


    public void Initialize(GameManager manager)
    {
        _manager = manager;
        GameManager.Instance.InputManager.openInventoryInputted += InputManager_openInventoryInputted;
        playerInventory = GameManager.Instance.PlayerManager.GetPlayer().Inventory;
        playerInventory.OnItemListChanged += Inventory_OnItemListChanged;
        playerInventoryUI.Initialize(this, playerInventory);
        playerInventoryUI.UpdateUI();
    }

    private void InputManager_openInventoryInputted(object sender, EventArgs e)
    {
        Debug.Log("Open Inventory");
        playerInventoryUI.gameObject.SetActive(!playerInventoryUI.gameObject.activeSelf);
    }

    private void Inventory_OnItemListChanged(object sender, EventArgs e)
    {
        playerInventoryUI.UpdateUI();
    }

    public void Dispose()
    {
        _manager = null;
        playerInventory = null;
    }

}
