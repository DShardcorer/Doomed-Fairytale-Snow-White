using System;
using UnityEngine;
using System.Collections.Generic;

public class PlayerInventoryPageUI : MonoBehaviour, ILifecycle<PlayerInventoryUI>
{
    public PlayerInventoryType playerInventoryType;
    private PlayerInventoryUI _playerInventoryUI;
    public PlayerInventoryUI PlayerInventoryUI => _playerInventoryUI;
    


    private List<ItemSlotUI> _itemSlots = new List<ItemSlotUI>();

    private PoolManager _poolManager;

    public void Initialize(PlayerInventoryUI playerInventoryUI)
    {
        _playerInventoryUI = playerInventoryUI;
        _poolManager = GameManager.Instance.PoolManager;
    }

    public void UpdateUI(List<InventoryItem> items)
    {
        // Ensure we have the correct number of slots
        AdjustItemSlotCount(items.Count);

        // Update each slot with the corresponding item
        for (int i = 0; i < items.Count; i++)
        {
            _itemSlots[i].UpdateUI(items[i]);
        }

        // Disable extra slots if necessary
        for (int i = items.Count; i < _itemSlots.Count; i++)
        {
            _itemSlots[i].UpdateUI(null);
        }
    }

    private void AdjustItemSlotCount(int requiredCount)
    {
        // Add more slots if needed
        while (_itemSlots.Count < requiredCount)
        {
            ItemSlotUI newSlot = _poolManager.GetObject(HelperUIName.InventorySlotUI).GetComponent<ItemSlotUI>();
            newSlot.transform.SetParent(transform, false);
            _itemSlots.Add(newSlot);
        }

        // Return excess slots to the pool if needed
        while (_itemSlots.Count > requiredCount)
        {
            ItemSlotUI excessSlot = _itemSlots[_itemSlots.Count - 1];
            _poolManager.ReturnObject(HelperUIName.InventorySlotUI, excessSlot.gameObject);
            _itemSlots.RemoveAt(_itemSlots.Count - 1);
        }
    }

    public void Dispose()
    {
        _playerInventoryUI = null;
        _itemSlots.Clear();
        _itemSlots = null;
        _poolManager = null;
    }
}
