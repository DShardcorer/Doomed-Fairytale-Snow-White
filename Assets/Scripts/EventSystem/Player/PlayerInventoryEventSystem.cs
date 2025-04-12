using System;
using System.Collections.Generic;
using Item.Inventory;

namespace EventSystem.Player
{
    public class WeightEventArgs : EventArgs
    {
        public float currentWeight;
        public float weightCapacity;
    }

    public static class PlayerInventoryEventSystem
    {
        public static event EventHandler<List<InventoryItem>> OnItemListChanged;
        public static event EventHandler<List<InventoryItem>> OnMaterialItemListChanged;
        public static event EventHandler<List<InventoryItem>> OnConsumableItemListChanged;
        public static event EventHandler<List<InventoryItem>> OnEquipmentItemListChanged;
        public static event EventHandler<List<InventoryItem>> OnMiscellaneousItemListChanged;
        public static event EventHandler<WeightEventArgs> OnWeightChanged;


        public static void InvokeItemListChanged(List<InventoryItem> items)
        {
            OnItemListChanged?.Invoke(null, items);
        }
        public static void InvokeMaterialItemListChanged(List<InventoryItem> items)
        {
            OnMaterialItemListChanged?.Invoke(null, items);
        }
        public static void InvokeConsumableItemListChanged(List<InventoryItem> items)
        {
            OnConsumableItemListChanged?.Invoke(null, items);
        }
        public static void InvokeEquipmentItemListChanged(List<InventoryItem> items)
        {
            OnEquipmentItemListChanged?.Invoke(null, items);
        }
        public static void InvokeMiscellaneousItemListChanged(List<InventoryItem> items)
        {
            OnMiscellaneousItemListChanged?.Invoke(null, items);
        }
        public static void InvokeWeightChanged(float currentWeight, float weightCapacity)
        {
            OnWeightChanged?.Invoke(null, new WeightEventArgs { currentWeight = currentWeight, weightCapacity = weightCapacity });
        }


    }
}