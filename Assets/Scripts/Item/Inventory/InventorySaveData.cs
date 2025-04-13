using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Item.Inventory
{
[System.Serializable]
public class InventoryItemData
{
    public string itemName; // a unique identifier from ItemData
    public int stackSize;
}

[System.Serializable]
public class InventorySaveData
{
    public float capacity;
    public float currentWeight;
    public List<InventoryItemData> items;
    public List<InventoryItemData> materialItems;
    public List<InventoryItemData> consumableItems;
    public List<InventoryItemData> equipmentItems;
    public List<InventoryItemData> miscellaneousItems;
}

}