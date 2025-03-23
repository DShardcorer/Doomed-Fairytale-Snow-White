using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentUI : IngameMenuPageUI
{
    [SerializeField] private EquipmentSlotUI[] _equipmentSlotUIs;
    public EquipmentSlotUI[] EquipmentSlotUIs => _equipmentSlotUIs;
    [SerializeField] private EquipmentInventoryUI _equipmentInventoryUI;
    public EquipmentInventoryUI EquipmentInventoryUI => _equipmentInventoryUI;

    public override void Initialize(IngameMenuUI parent)
    {
        base.Initialize(parent);
        foreach (EquipmentSlotUI equipmentSlotUI in _equipmentSlotUIs)
        {
            equipmentSlotUI.Initialize(this);
        }
        _equipmentInventoryUI.Initialize(this);
        PlayerEquipmentEventSystem.PlayerEquipmentSystem_OnEquipmentChanged += EquipmentSystem_OnEquipmentChanged;
    }

    private void EquipmentSystem_OnEquipmentChanged(object sender, IReadOnlyDictionary<EquipmentSlotType, EquipmentInventoryItem> e)
    {
        foreach (EquipmentSlotUI equipmentSlotUI in _equipmentSlotUIs)
        {
            if (e.ContainsKey(equipmentSlotUI.SlotType))
            {
                equipmentSlotUI.UpdateUI(e[equipmentSlotUI.SlotType]);
            }
            else
            {
                equipmentSlotUI.UpdateUI(null);
            }
        }
        
    }

    public void FireUnequipItemEvent(EquipmentInventoryItem item)
    {
        PlayerEquipmentEventSystem.InvokePlayerEquipmentUI_OnItemUnequipped(item);
    }

}
