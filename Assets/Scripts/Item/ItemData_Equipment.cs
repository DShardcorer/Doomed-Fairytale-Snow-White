using UnityEngine;

public enum EquipmentSlotType
{
    Head,
    Chest,
    Legs,
    Hands,
    Weapon,
    OffHand,
    Accessory
}
public enum EquipmentItemType
{
    Sword,
    Spear,
    Bow,
    Shield,
    Helmet,
    Chestplate,
    Boots,
    Gloves,
    Ring,
    Amulet
}


[CreateAssetMenu(fileName = "New Equipment Item", menuName = "Equipment ItemData")]
public class ItemData_Equipment : ItemData
{
    public EquipmentSlotType equipmentSlotType;
    
}
