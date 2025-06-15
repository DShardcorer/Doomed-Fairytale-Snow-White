using System.Collections.Generic;
using Item;
using Item.Inventory;


namespace EntitySystems.PlayerSystems
{
    public static partial class SaveLoadHelper
    {
        public static InventorySaveData CreateSaveData(PlayerInventorySystem inventory)
        {
            InventorySaveData saveData = new InventorySaveData
            {
                capacity = inventory.Capacity,
                currentWeight = inventory.CurrentWeight,
                items = ConvertItems(inventory.ItemList),
                materialItems = ConvertItems(inventory.materialItems),
                consumableItems = ConvertItems(inventory.consumableItems),
                equipmentItems = ConvertItems(inventory.equipmentItems),
                miscellaneousItems = ConvertItems(inventory.miscellaneousItems)
            };

            return saveData;
        }

        public static void LoadFromSaveData(PlayerInventorySystem inventory, InventorySaveData saveData)
        {
            inventory.ClearAll();

            inventory.capacity = saveData.capacity;
            inventory.currentWeight = saveData.currentWeight;

            inventory.ItemList = CreateItems(saveData.items, inventory.itemDictionary);
            inventory.materialItems = CreateItems(saveData.materialItems, inventory.materialItemDictionary);
            inventory.consumableItems = CreateItems(saveData.consumableItems, inventory.consumableItemDictionary);
            inventory.equipmentItems = CreateItems(saveData.equipmentItems, inventory.equipmentItemDictionary);
            inventory.miscellaneousItems =
                CreateItems(saveData.miscellaneousItems, inventory.miscellaneousItemDictionary);
        }

        private static List<InventoryItemSaveData> ConvertItems(List<InventoryItem> items)
        {
            var result = new List<InventoryItemSaveData>();
            foreach (var item in items)
            {
                result.Add(new InventoryItemSaveData
                {
                    itemName = item.itemDataSo.itemName,
                    stackSize = item.stackSize
                });
            }

            return result;
        }

        private static List<InventoryItem> CreateItems(List<InventoryItemSaveData> dataList,
            Dictionary<ItemDataSO, InventoryItem> dictionary)
        {
            var result = new List<InventoryItem>();
            foreach (var data in dataList)
            {
                var itemData = ItemDataRegistry.GetItemDataByName(data.itemName);
                var item = InventoryItemFactory.CreateItem(itemData);
                item.stackSize = data.stackSize;
                result.Add(item);
                dictionary[itemData] = item;
            }

            return result;
        }
    }

    public static partial class SaveLoadHelper
    {
    }
}