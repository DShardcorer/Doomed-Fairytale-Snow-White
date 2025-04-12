using System.Collections.Generic;
using EntitySystems.Equipment;
using EventSystem.Player;
using GeneralManagers;
using Helpers;
using Item.Inventory;
using Pool;
using UnityEngine;

namespace UI.Player.Equipment
{
    public class EquipmentInventoryUI : MonoBehaviour, ILifecycle<PlayerEquipmentUI>
    {
        private PlayerEquipmentUI _parent;
        private List<EquipmentInventorySlotUI> _equipmentSlots = new List<EquipmentInventorySlotUI>();
        private PoolManager _poolManager;

        private List<EquipmentInventoryItem> _equipmentItems;

    



        public void FireEquipEvent(EquipmentInventoryItem item)
        {
            PlayerEquipmentEventSystem.InvokeEquipmentInventoryUI_ItemEquipped(item);
        }



        public void Initialize(PlayerEquipmentUI parent)
        {
            _parent = parent;
            _poolManager = GameManager.Instance.PoolManager;
            //sub to inventory eventsystem change events
            PlayerInventoryEventSystem.OnEquipmentItemListChanged += Inventory_OnEquipmentItemListChanged;
            PlayerEquipmentEventSystem.OnEquipmentChanged += EquipmentSystem_OnEquipmentChanged;
    
        }

        private void EquipmentSystem_OnEquipmentChanged(object sender, IReadOnlyDictionary<EquipmentSlotType, EquipmentInventoryItem> e)
        {
            UpdateEquippedStatus();
        }

        private void Inventory_OnEquipmentItemListChanged(object sender, List<InventoryItem> e)
        {
            UpdateUI(e);
        }

        public void Dispose()
        {
            _parent = null;
        }
        public void UpdateEquippedStatus()
        {
            foreach (EquipmentInventorySlotUI slot in _equipmentSlots)
            {
                slot.UpdateEquippedStatus();
            }
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
                EquipmentInventorySlotUI newSlot = _poolManager.GetObject(HelperUIName.EquipmentInventorySlotUI).GetComponent<EquipmentInventorySlotUI>();
                newSlot.transform.SetParent(transform, false);
                newSlot.Initialize(this);
                _equipmentSlots.Add(newSlot);
            }

            // Return excess slots to the pool if needed
            while (_equipmentSlots.Count > requiredCount)
            {
                EquipmentInventorySlotUI excessSlot = _equipmentSlots[_equipmentSlots.Count - 1];
                _poolManager.ReturnObject(HelperUIName.EquipmentInventorySlotUI, excessSlot.gameObject);
                excessSlot.Dispose();
                _equipmentSlots.RemoveAt(_equipmentSlots.Count - 1);
            }
        }


    }
}
