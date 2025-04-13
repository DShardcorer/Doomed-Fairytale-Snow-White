using UnityEngine;
using System.Collections.Generic;

namespace Item
{
    public static class ItemDataRegistry
    {
        // Dictionary to hold all ItemData keyed by a unique identifier (here we're using itemName)
        private static Dictionary<string, ItemData> itemDataDictionary;

        // Call this to initialize the registry, either on application start or when needed.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            LoadAllItemData();
        }

        private static void LoadAllItemData()
        {
            itemDataDictionary = new Dictionary<string, ItemData>();

            // Load all ItemData assets from the Resources/ItemData folder
            ItemData[] items = UnityEngine.Resources.LoadAll<ItemData>("ScriptableObjects/ItemData");

            foreach (ItemData item in items)
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
        public static ItemData GetItemDataByName(string name)
        {
            if (itemDataDictionary.TryGetValue(name, out ItemData data))
            {
                return data;
            }

            Debug.LogError("ItemData not found in registry for name: " + name);
            return null;
        }
    }
}