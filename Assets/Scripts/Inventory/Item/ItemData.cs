using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData: ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType type;
    public int maxStack;
    public bool isEquippable;
    public float weight; // Weight of a single item

    public enum ItemType { Consumable, Weapon, Armor, Misc }
}
