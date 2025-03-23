using System.Collections.Generic;
using UnityEngine;

public enum EquipmentSlotType
{
    Head,
    Chest,
    Legs,
    Boots,
    Hands,
    Weapon,
    OffHand,
    Necklace,
    Ring1,
    Ring2
}
public enum EquipmentItemType
{
    Sword,
    Spear,
    Bow,
    Shield,
    Helmet,
    Chestplate,
    Leggings,
    Boots,
    Gloves,
    Ring,
    Necklace
}


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
