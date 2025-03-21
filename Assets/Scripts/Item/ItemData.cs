using UnityEngine;
public enum ItemType
{
    Equipment,
    Consumable,
    Material,
    Miscellaneous
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "ItemData")]
public class ItemData: ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
    public float weight =1;
}
