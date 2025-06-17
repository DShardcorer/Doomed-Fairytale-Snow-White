using UnityEngine;
using System.Collections.Generic;
using Item.Inventory;

namespace Item
{
    public static class ItemRegistry
    {
        // Dictionary to hold all ItemData keyed by a unique identifier (here we're using itemName)
        private static Dictionary<string, ItemDataSO> itemDataDictionary;

        // Call this to initialize the registry, either on application start or when needed.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Initialize()
        {
            LoadAllItemData();
        }

        private static void LoadAllItemData()
        {
            itemDataDictionary = new Dictionary<string, ItemDataSO>();

            // Load all ItemData assets from the Resources/ItemData folder
            ItemDataSO[] items = UnityEngine.Resources.LoadAll<ItemDataSO>("ScriptableObjects/ItemData");

            foreach (ItemDataSO item in items)
            {
                if (!itemDataDictionary.ContainsKey(item.itemName))
                {
                    itemDataDictionary.Add(item.itemName, item);
                }
                else
                {
                    Debug.LogWarning("Duplicate ItemData detected with name: " + item.itemName);
                }
            }

            Debug.Log("Loaded " + itemDataDictionary.Count + " items from registry.");
        }

        // Public method to retrieve an ItemData by its unique key (or name, in this example)
        public static ItemDataSO GetItemDataByName(string name)
        {
            if (itemDataDictionary.TryGetValue(name, out ItemDataSO data))
            {
                return data;
            }

            Debug.LogError("ItemData not found in registry for name: " + name);
            return null;
        }
        public static InventoryItem CreateInventoryItem(string itemName, int quantity = 1)
        {
            ItemDataSO itemData = GetItemDataByName(itemName);
            if (itemData == null)
            {
                Debug.LogError("ItemData not found for itemName: " + itemName);
                return null;
            }

            // Create the appropriate type of InventoryItem based on the ItemData type
            if (itemData is ItemDataSOEquipment equipmentData)
            {
                return new EquipmentInventoryItem(equipmentData, quantity);
            }
            else
            {
                return new InventoryItem(itemData, quantity);
            }
        }
    }
}