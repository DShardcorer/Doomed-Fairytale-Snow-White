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
        public float weight =1;
    }
}