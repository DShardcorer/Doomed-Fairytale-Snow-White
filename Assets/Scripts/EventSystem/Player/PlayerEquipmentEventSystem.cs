using System;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerEquipmentEventSystem
{
    public static event EventHandler<IReadOnlyDictionary<EquipmentSlotType, ItemData_Equipment>> PlayerEquipmentSystem_OnEquipmentChanged;

    public static event EventHandler<EquipmentInventoryItem> EquipmentInventoryUI_OnItemEquipped;

    public static void InvokePlayerEquipmentSystem_OnEquipmentChanged(IReadOnlyDictionary<EquipmentSlotType, ItemData_Equipment> e)
    {
        PlayerEquipmentSystem_OnEquipmentChanged?.Invoke(null, e);
    }

    public static void InvokeEquipmentInventoryUI_OnItemEquipped(EquipmentInventoryItem e)
    {
        EquipmentInventoryUI_OnItemEquipped?.Invoke(null, e);
    }



}
