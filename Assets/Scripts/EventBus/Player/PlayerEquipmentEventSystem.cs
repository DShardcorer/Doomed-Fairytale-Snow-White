using System;
using System.Collections.Generic;
using EntitySystems.Equipment;
using Item.Inventory;

namespace EventBus.Player
{
    public static class PlayerEquipmentEventSystem
    {
        public static event EventHandler<IReadOnlyDictionary<EquipmentSlotType, EquipmentInventoryItem>> OnEquipmentChanged;

        public static event EventHandler<EquipmentInventoryItem> OnEquipmentEquipped;
        public static event EventHandler<EquipmentInventoryItem> OnEquipmentUnequippedUnequipped;

        public static void InvokePlayerEquipmentSystem_EquipmentChanged(IReadOnlyDictionary<EquipmentSlotType, EquipmentInventoryItem> e)
        {
            OnEquipmentChanged?.Invoke(null, e);
        }

        public static void InvokeEquipmentInventoryUI_ItemEquipped(EquipmentInventoryItem e)
        {
            OnEquipmentEquipped?.Invoke(null, e);
        }
        public static void InvokePlayerEquipmentUI_ItemUnequipped(EquipmentInventoryItem e)
        {
            OnEquipmentUnequippedUnequipped?.Invoke(null, e);
        }



    }
}
