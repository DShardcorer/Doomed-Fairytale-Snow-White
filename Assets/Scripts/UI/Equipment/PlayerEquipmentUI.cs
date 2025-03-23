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
    }




}
