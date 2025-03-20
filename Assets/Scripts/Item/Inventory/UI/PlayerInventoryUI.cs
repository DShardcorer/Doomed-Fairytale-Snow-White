using System;
using UnityEngine;
using System.Collections.Generic;

public class PlayerInventoryUI : MonoBehaviour
{
    private PlayerInventoryManager _playerInventoryManager;
    public PlayerInventoryManager InventoryManager => _playerInventoryManager;
    
    private Inventory _inventory;
    public Inventory PlayerInventory => _inventory;

    [SerializeField] private Transform _itemSlotParent;

    private List<ItemSlotUI> _itemSlots = new List<ItemSlotUI>();

    private PoolManager _poolManager;

    public void Initialize(PlayerInventoryManager playerInventoryManager, Inventory inventory)
    {
        _playerInventoryManager = playerInventoryManager;
        _inventory = inventory;
        _poolManager = _playerInventoryManager.Manager.PoolManager;
        
        UpdateUI();
    }

    public void UpdateUI()
    {
        Debug.Log("Updating UI");
        // Ensure we have the correct number of slots
        AdjustItemSlotCount(_inventory.items.Count);

        // Update each slot with the corresponding item
        for (int i = 0; i < _inventory.items.Count; i++)
        {
            _itemSlots[i].UpdateUI(_inventory.items[i]);
        }

        // Disable extra slots if necessary
        for (int i = _inventory.items.Count; i < _itemSlots.Count; i++)
        {
            _itemSlots[i].UpdateUI(null);
        }
    }

    private void AdjustItemSlotCount(int requiredCount)
    {
        // Add more slots if needed
        while (_itemSlots.Count < requiredCount)
        {
            ItemSlotUI newSlot = _poolManager.GetObject(UINameHelper.ItemSlotUI).GetComponent<ItemSlotUI>();
            newSlot.transform.SetParent(_itemSlotParent, false);
            _itemSlots.Add(newSlot);
        }

        // Return excess slots to the pool if needed
        while (_itemSlots.Count > requiredCount)
        {
            ItemSlotUI excessSlot = _itemSlots[_itemSlots.Count - 1];
            _poolManager.ReturnObject(UINameHelper.ItemSlotUI, excessSlot.gameObject);
            _itemSlots.RemoveAt(_itemSlots.Count - 1);
        }
    }
}
