using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentInventoryUI : MonoBehaviour, ILifecycle<PlayerEquipmentUI>
{
    private PlayerEquipmentUI _parent;
    private List<EquipmentInventorySlotUI> _equipmentSlots = new List<EquipmentInventorySlotUI>();
    private PoolManager _poolManager;

    private List<EquipmentInventoryItem> _equipmentItems;

    public event EventHandler<EquipmentInventoryItem> OnItemEquipped;



    public void FireEquipEvent(EquipmentInventoryItem item)
    {
        OnItemEquipped?.Invoke(this, item);
    }



    public void Initialize(PlayerEquipmentUI parent)
    {
        _parent = parent;
        _poolManager = GameManager.Instance.PoolManager;
        //sub to inventory change events
        GameManager.Instance.PlayerManager.GetPlayer().Inventory.OnEquipmentItemListChanged += Inventory_OnEquipmentItemListChanged;
    }

    private void Inventory_OnEquipmentItemListChanged(object sender, List<InventoryItem> e)
    {
        UpdateUI(e);
    }

    public void Dispose()
    {
        _parent = null;
    }

    public void UpdateUI(List<InventoryItem> items)
    {
        _equipmentItems = items.ConvertAll(item => (EquipmentInventoryItem)item);
        AdjustEquipmentSlotCount(items.Count);
        for (int i = 0; i < items.Count; i++)
        {
            _equipmentSlots[i].UpdateUI(_equipmentItems[i]);
        }

        // Disable extra slots if necessary
        for (int i = items.Count; i < _equipmentSlots.Count; i++)
        {
            _equipmentSlots[i].UpdateUI(null);
        }
    }

    private void AdjustEquipmentSlotCount(int requiredCount)
    {
        // Add more slots if needed
        while (_equipmentSlots.Count < requiredCount)
        {
            EquipmentInventorySlotUI newSlot = _poolManager.GetObject(UINameHelper.EquipmentInventorySlotUI).GetComponent<EquipmentInventorySlotUI>();
            newSlot.transform.SetParent(transform, false);
            newSlot.Initialize(this);
            _equipmentSlots.Add(newSlot);
        }

        // Return excess slots to the pool if needed
        while (_equipmentSlots.Count > requiredCount)
        {
            EquipmentInventorySlotUI excessSlot = _equipmentSlots[_equipmentSlots.Count - 1];
            _poolManager.ReturnObject(UINameHelper.EquipmentInventorySlotUI, excessSlot.gameObject);
            excessSlot.Dispose();
            _equipmentSlots.RemoveAt(_equipmentSlots.Count - 1);
        }
    }


}
