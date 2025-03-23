using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Item", menuName = "Equipment ItemData")]
public class ItemData_Equipment : ItemData
{

    public EquipmentSlotType equipmentSlotType;
    public EquipmentItemType equipmentItemType;
    public List<StatModifier> StatModifiers;

    public string GetStatsString()
    {
        string statsString = "";
        foreach (StatModifier statModifier in StatModifiers)
        {
            statsString += statModifier.GetStatString() + "\n";
        }
        return statsString;
    }
    
}
