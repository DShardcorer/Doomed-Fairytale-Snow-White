using System;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour, ILifecycle<GameManager>
{
    private GameManager _manager;
    public GameManager Manager => _manager;
    private Inventory _playerInventory;
    public Inventory PlayerInventory => _playerInventory;
    private PlayerInventoryUI _inventoryUI;
    public PlayerInventoryUI InventoryUI => _inventoryUI;


    public void Initialize(GameManager manager)
    {
        _manager = manager;
        // GameManager.Instance.InputManager.openInventoryInputted += InputManager_openInventoryInputted;
        _playerInventory = GameManager.Instance.PlayerManager.GetPlayer().Inventory;
        _playerInventory.OnItemListChanged += Inventory_OnItemListChanged;
        _playerInventory.OnMaterialItemListChanged += Inventory_OnMaterialItemListChanged;
        _playerInventory.OnConsumableItemListChanged += Inventory_OnConsumableItemListChanged;
        _playerInventory.OnEquipmentItemListChanged += Inventory_OnEquipmentItemListChanged;
        _playerInventory.OnMiscellaneousItemListChanged += Inventory_OnMiscellaneousItemListChanged;

        while(_inventoryUI == null)
        {
            _inventoryUI = UIManager.Instance.PlayerInventoryUI;
        }
        Debug.Log("Player Inventory Manager Initialized");
        _inventoryUI.UpdateAllUI();


    }

    private void Inventory_OnMiscellaneousItemListChanged(object sender, EventArgs e)
    {
        _inventoryUI.UpdateInventoryUI(PlayerInventoryType.Miscellaneous);
    }

    private void Inventory_OnEquipmentItemListChanged(object sender, EventArgs e)
    {
        _inventoryUI.UpdateInventoryUI(PlayerInventoryType.Equipment);
    }

    private void Inventory_OnConsumableItemListChanged(object sender, EventArgs e)
    {
        _inventoryUI.UpdateInventoryUI(PlayerInventoryType.Consumable);
    }

    private void Inventory_OnMaterialItemListChanged(object sender, EventArgs e)
    {
        _inventoryUI.UpdateInventoryUI(PlayerInventoryType.Material);
    }


    private void Inventory_OnItemListChanged(object sender, EventArgs e)
    {
        _inventoryUI.UpdateAllUI();
    }

    private void InputManager_openInventoryInputted(object sender, EventArgs e)
    {
        Debug.Log("Open Inventory");
        // _inventoryUI.gameObject.SetActive(!_inventoryUI.gameObject.activeSelf);
    }

    public void Dispose()
    {
        _manager = null;
        _playerInventory = null;
    }

}
