using System.Collections.Generic;

namespace DataPersistence.Data
{
[System.Serializable]
public class InventoryItemSaveData
{
    public string itemName; // a unique identifier from ItemData
    public int stackSize;
}

[System.Serializable]
public class InventorySaveData
{
    public float capacity;
    public float currentWeight;
    public List<InventoryItemSaveData> items;
    public List<InventoryItemSaveData> materialItems;
    public List<InventoryItemSaveData> consumableItems;
    public List<InventoryItemSaveData> equipmentItems;
    public List<InventoryItemSaveData> miscellaneousItems;
}

}