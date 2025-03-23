using System;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerEquipmentEventSystem
{
    public static event EventHandler<IReadOnlyDictionary<EquipmentSlotType, EquipmentInventoryItem>> PlayerEquipmentSystem_OnEquipmentChanged;

    public static event EventHandler<EquipmentInventoryItem> EquipmentInventoryUI_OnItemEquipped;
    public static event EventHandler<EquipmentInventoryItem> PlayerEquipmentUI_OnItemUnequipped;

    public static void InvokePlayerEquipmentSystem_OnEquipmentChanged(IReadOnlyDictionary<EquipmentSlotType, EquipmentInventoryItem> e)
    {
        PlayerEquipmentSystem_OnEquipmentChanged?.Invoke(null, e);
    }

    public static void InvokeEquipmentInventoryUI_OnItemEquipped(EquipmentInventoryItem e)
    {
        EquipmentInventoryUI_OnItemEquipped?.Invoke(null, e);
    }
    public static void InvokePlayerEquipmentUI_OnItemUnequipped(EquipmentInventoryItem e)
    {
        PlayerEquipmentUI_OnItemUnequipped?.Invoke(null, e);
    }



}
