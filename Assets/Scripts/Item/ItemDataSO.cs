using System;
using UnityEngine;

namespace Item
{
    public enum ItemType
    {
        Equipment,
        Consumable,
        Material,
        Miscellaneous
    }
    
    [Serializable]
    [CreateAssetMenu(fileName = "New Item Data", menuName = "ItemData")]
    public class ItemDataSO: ScriptableObject
    {
        public ItemType itemType;
        public string itemName;
        public Sprite icon;
        [TextArea(3, 10)]
        public string description = "No description available.";
        public int maxStackSize = 99;
        public int value = 1;
        public float weight =1;
    }
}