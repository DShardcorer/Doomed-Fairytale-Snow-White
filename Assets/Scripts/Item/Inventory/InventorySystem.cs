using System.Collections.Generic;

namespace Item.Inventory
{
    public class InventorySystem
    {
        protected Entity.Entity _entity;
        public Entity.Entity Entity => _entity;

        public float capacity;
        public float Capacity => capacity;
        public float currentWeight;
        public float CurrentWeight => currentWeight;

        public List<InventoryItem> items;
        public Dictionary<ItemData, InventoryItem> itemDictionary;

        public List<InventoryItem> materialItems;
        public Dictionary<ItemData, InventoryItem> materialItemDictionary;

        public List<InventoryItem> consumableItems;
        public Dictionary<ItemData, InventoryItem> consumableItemDictionary;

        public List<InventoryItem> equipmentItems;
        public Dictionary<ItemData, InventoryItem> equipmentItemDictionary;

        public List<InventoryItem> miscellaneousItems;
        public Dictionary<ItemData, InventoryItem> miscellaneousItemDictionary;

        public InventorySystem()
        {
            items = new List<InventoryItem>();
            itemDictionary = new Dictionary<ItemData, InventoryItem>();
            materialItems = new List<InventoryItem>();
            materialItemDictionary = new Dictionary<ItemData, InventoryItem>();
            consumableItems = new List<InventoryItem>();
            consumableItemDictionary = new Dictionary<ItemData, InventoryItem>();
            equipmentItems = new List<InventoryItem>();
            equipmentItemDictionary = new Dictionary<ItemData, InventoryItem>();
            miscellaneousItems = new List<InventoryItem>();
            miscellaneousItemDictionary = new Dictionary<ItemData, InventoryItem>();
        }

        public void ClearAll()
        {
            items.Clear();
            itemDictionary.Clear();
            materialItems.Clear();
            materialItemDictionary.Clear();
            consumableItems.Clear();
            consumableItemDictionary.Clear();
            equipmentItems.Clear();
            equipmentItemDictionary.Clear();
            miscellaneousItems.Clear();
            miscellaneousItemDictionary.Clear();
        }

        public virtual void Initialize(Entity.Entity entity)
        {
            _entity = entity;
            capacity = entity.StatSystem.AbilityStatBoard.Strength.ModifiedValue * 10;
            UpdateCurrentWeight();
        }

        public virtual void Dispose()
        {
            _entity = null;
        }

        public virtual void InvokeInitialEvents()
        {
            // Base inventory system does not trigger events by default.
        }

        // Virtual hooks for notifying changes; default implementations do nothing.
        protected virtual void OnItemListChanged(List<InventoryItem> items)
        {
        }

        protected virtual void OnMaterialItemListChanged(List<InventoryItem> materialItems)
        {
        }

        protected virtual void OnConsumableItemListChanged(List<InventoryItem> consumableItems)
        {
        }

        protected virtual void OnEquipmentItemListChanged(List<InventoryItem> equipmentItems)
        {
        }

        protected virtual void OnMiscellaneousItemListChanged(List<InventoryItem> miscellaneousItems)
        {
        }

        protected virtual void OnWeightChanged(float currentWeight, float capacity)
        {
        }

        public virtual void UpdateCurrentWeight()
        {
            currentWeight = 0;
            foreach (InventoryItem item in items)
            {
                currentWeight += item.ItemData.weight * item.stackSize;
            }

            OnWeightChanged(currentWeight, capacity);
        }

        public virtual void IncrementCurrentWeight(float weight)
        {
            currentWeight += weight;
            OnWeightChanged(currentWeight, capacity);
        }

        public virtual void DecrementCurrentWeight(float weight)
        {
            currentWeight -= weight;
            OnWeightChanged(currentWeight, capacity);
        }

        public virtual void AddItem(InventoryItem inventoryItem)
        {
            AddItem(inventoryItem.ItemData, inventoryItem.stackSize);
        }

        public virtual void AddItem(ItemData itemData, int amount)
        {
            InventoryItem item;
            bool isNewItem = !itemDictionary.TryGetValue(itemData, out item);

            if (isNewItem)
            {
                item = InventoryItemFactory.CreateItem(itemData);
                items.Add(item);
                itemDictionary[itemData] = item;
            }

            item.AddToStack(amount);

            switch (itemData.itemType)
            {
                case ItemType.Material:
                    if (isNewItem) materialItems.Add(item);
                    materialItemDictionary[itemData] = item;
                    OnMaterialItemListChanged(materialItems);
                    break;
                case ItemType.Consumable:
                    if (isNewItem) consumableItems.Add(item);
                    consumableItemDictionary[itemData] = item;
                    OnConsumableItemListChanged(consumableItems);
                    break;
                case ItemType.Equipment:
                    if (isNewItem) equipmentItems.Add(item);
                    equipmentItemDictionary[itemData] = item;
                    OnEquipmentItemListChanged(equipmentItems);
                    break;
                case ItemType.Miscellaneous:
                    if (isNewItem) miscellaneousItems.Add(item);
                    miscellaneousItemDictionary[itemData] = item;
                    OnMiscellaneousItemListChanged(miscellaneousItems);
                    break;
            }

            OnItemListChanged(items);
            IncrementCurrentWeight(itemData.weight * amount);
        }


        public virtual void AddItem(ItemData itemData)
        {
            InventoryItem item;
            bool isNewItem = !itemDictionary.TryGetValue(itemData, out item);

            if (isNewItem)
            {
                item = InventoryItemFactory.CreateItem(itemData);
                items.Add(item);
                itemDictionary[itemData] = item;
            }

            item.AddToStack();

            switch (itemData.itemType)
            {
                case ItemType.Material:
                    if (isNewItem) materialItems.Add(item);
                    materialItemDictionary[itemData] = item;
                    OnMaterialItemListChanged(materialItems);
                    break;
                case ItemType.Consumable:
                    if (isNewItem) consumableItems.Add(item);
                    consumableItemDictionary[itemData] = item;
                    OnConsumableItemListChanged(consumableItems);
                    break;
                case ItemType.Equipment:
                    if (isNewItem) equipmentItems.Add(item);
                    equipmentItemDictionary[itemData] = item;
                    OnEquipmentItemListChanged(equipmentItems);
                    break;
                case ItemType.Miscellaneous:
                    if (isNewItem) miscellaneousItems.Add(item);
                    miscellaneousItemDictionary[itemData] = item;
                    OnMiscellaneousItemListChanged(miscellaneousItems);
                    break;
            }

            OnItemListChanged(items);
            IncrementCurrentWeight(itemData.weight);
        }

        public virtual void RemoveItem(InventoryItem inventoryItem)
        {
            RemoveItem(inventoryItem.ItemData, inventoryItem.stackSize);
        }

        public virtual void RemoveItem(ItemData itemData, int amount)
        {
            if (itemDictionary.ContainsKey(itemData))
            {
                itemDictionary[itemData].RemoveFromStack(amount);
                switch (itemData.itemType)
                {
                    case ItemType.Material:
                        materialItemDictionary[itemData].RemoveFromStack(amount);
                        OnMaterialItemListChanged(materialItems);
                        break;
                    case ItemType.Consumable:
                        consumableItemDictionary[itemData].RemoveFromStack(amount);
                        OnConsumableItemListChanged(consumableItems);
                        break;
                    case ItemType.Equipment:
                        equipmentItemDictionary[itemData].RemoveFromStack(amount);
                        OnEquipmentItemListChanged(equipmentItems);
                        break;
                    case ItemType.Miscellaneous:
                        miscellaneousItemDictionary[itemData].RemoveFromStack(amount);
                        OnMiscellaneousItemListChanged(miscellaneousItems);
                        break;
                }

                if (itemDictionary[itemData].stackSize <= 0)
                {
                    items.Remove(itemDictionary[itemData]);
                    itemDictionary.Remove(itemData);
                    switch (itemData.itemType)
                    {
                        case ItemType.Material:
                            materialItems.Remove(materialItemDictionary[itemData]);
                            materialItemDictionary.Remove(itemData);
                            OnMaterialItemListChanged(materialItems);
                            break;
                        case ItemType.Consumable:
                            consumableItems.Remove(consumableItemDictionary[itemData]);
                            consumableItemDictionary.Remove(itemData);
                            OnConsumableItemListChanged(consumableItems);
                            break;
                        case ItemType.Equipment:
                            equipmentItems.Remove(equipmentItemDictionary[itemData]);
                            equipmentItemDictionary.Remove(itemData);
                            OnEquipmentItemListChanged(equipmentItems);
                            break;
                        case ItemType.Miscellaneous:
                            miscellaneousItems.Remove(miscellaneousItemDictionary[itemData]);
                            miscellaneousItemDictionary.Remove(itemData);
                            OnMiscellaneousItemListChanged(miscellaneousItems);
                            break;
                    }
                }

                OnItemListChanged(items);
                DecrementCurrentWeight(itemData.weight * amount);
            }
        }

        public virtual void RemoveItem(ItemData itemData)
        {
            if (itemDictionary.ContainsKey(itemData))
            {
                itemDictionary[itemData].RemoveFromStack();
                switch (itemData.itemType)
                {
                    case ItemType.Material:
                        materialItemDictionary[itemData].RemoveFromStack();
                        OnMaterialItemListChanged(materialItems);
                        break;
                    case ItemType.Consumable:
                        consumableItemDictionary[itemData].RemoveFromStack();
                        OnConsumableItemListChanged(consumableItems);
                        break;
                    case ItemType.Equipment:
                        equipmentItemDictionary[itemData].RemoveFromStack();
                        OnEquipmentItemListChanged(equipmentItems);
                        break;
                    case ItemType.Miscellaneous:
                        miscellaneousItemDictionary[itemData].RemoveFromStack();
                        OnMiscellaneousItemListChanged(miscellaneousItems);
                        break;
                }

                if (itemDictionary[itemData].stackSize <= 0)
                {
                    items.Remove(itemDictionary[itemData]);
                    itemDictionary.Remove(itemData);
                    switch (itemData.itemType)
                    {
                        case ItemType.Material:
                            materialItems.Remove(materialItemDictionary[itemData]);
                            materialItemDictionary.Remove(itemData);
                            OnMaterialItemListChanged(materialItems);
                            break;
                        case ItemType.Consumable:
                            consumableItems.Remove(consumableItemDictionary[itemData]);
                            consumableItemDictionary.Remove(itemData);
                            OnConsumableItemListChanged(consumableItems);
                            break;
                        case ItemType.Equipment:
                            equipmentItems.Remove(equipmentItemDictionary[itemData]);
                            equipmentItemDictionary.Remove(itemData);
                            OnEquipmentItemListChanged(equipmentItems);
                            break;
                        case ItemType.Miscellaneous:
                            miscellaneousItems.Remove(miscellaneousItemDictionary[itemData]);
                            miscellaneousItemDictionary.Remove(itemData);
                            OnMiscellaneousItemListChanged(miscellaneousItems);
                            break;
                    }
                }

                OnItemListChanged(items);
                DecrementCurrentWeight(itemData.weight);
            }
        }
    }
}