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
        public Dictionary<ItemDataSO, InventoryItem> itemDictionary;

        public List<InventoryItem> materialItems;
        public Dictionary<ItemDataSO, InventoryItem> materialItemDictionary;

        public List<InventoryItem> consumableItems;
        public Dictionary<ItemDataSO, InventoryItem> consumableItemDictionary;

        public List<InventoryItem> equipmentItems;
        public Dictionary<ItemDataSO, InventoryItem> equipmentItemDictionary;

        public List<InventoryItem> miscellaneousItems;
        public Dictionary<ItemDataSO, InventoryItem> miscellaneousItemDictionary;

        public InventorySystem()
        {
            items = new List<InventoryItem>();
            itemDictionary = new Dictionary<ItemDataSO, InventoryItem>();
            materialItems = new List<InventoryItem>();
            materialItemDictionary = new Dictionary<ItemDataSO, InventoryItem>();
            consumableItems = new List<InventoryItem>();
            consumableItemDictionary = new Dictionary<ItemDataSO, InventoryItem>();
            equipmentItems = new List<InventoryItem>();
            equipmentItemDictionary = new Dictionary<ItemDataSO, InventoryItem>();
            miscellaneousItems = new List<InventoryItem>();
            miscellaneousItemDictionary = new Dictionary<ItemDataSO, InventoryItem>();
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
                currentWeight += item.itemDataSo.weight * item.stackSize;
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
            AddItem(inventoryItem.itemDataSo, inventoryItem.stackSize);
        }

        public virtual void AddItem(ItemDataSO itemDataSo, int amount)
        {
            InventoryItem item;
            bool isNewItem = !itemDictionary.TryGetValue(itemDataSo, out item);

            if (isNewItem)
            {
                item = InventoryItemFactory.CreateItem(itemDataSo);
                items.Add(item);
                itemDictionary[itemDataSo] = item;
            }

            item.AddToStack(amount);

            switch (itemDataSo.itemType)
            {
                case ItemType.Material:
                    if (isNewItem) materialItems.Add(item);
                    materialItemDictionary[itemDataSo] = item;
                    OnMaterialItemListChanged(materialItems);
                    break;
                case ItemType.Consumable:
                    if (isNewItem) consumableItems.Add(item);
                    consumableItemDictionary[itemDataSo] = item;
                    OnConsumableItemListChanged(consumableItems);
                    break;
                case ItemType.Equipment:
                    if (isNewItem) equipmentItems.Add(item);
                    equipmentItemDictionary[itemDataSo] = item;
                    OnEquipmentItemListChanged(equipmentItems);
                    break;
                case ItemType.Miscellaneous:
                    if (isNewItem) miscellaneousItems.Add(item);
                    miscellaneousItemDictionary[itemDataSo] = item;
                    OnMiscellaneousItemListChanged(miscellaneousItems);
                    break;
            }

            OnItemListChanged(items);
            IncrementCurrentWeight(itemDataSo.weight * amount);
        }


        public virtual void AddItem(ItemDataSO itemDataSo)
        {
            InventoryItem item;
            bool isNewItem = !itemDictionary.TryGetValue(itemDataSo, out item);

            if (isNewItem)
            {
                item = InventoryItemFactory.CreateItem(itemDataSo);
                items.Add(item);
                itemDictionary[itemDataSo] = item;
            }

            item.AddToStack();

            switch (itemDataSo.itemType)
            {
                case ItemType.Material:
                    if (isNewItem) materialItems.Add(item);
                    materialItemDictionary[itemDataSo] = item;
                    OnMaterialItemListChanged(materialItems);
                    break;
                case ItemType.Consumable:
                    if (isNewItem) consumableItems.Add(item);
                    consumableItemDictionary[itemDataSo] = item;
                    OnConsumableItemListChanged(consumableItems);
                    break;
                case ItemType.Equipment:
                    if (isNewItem) equipmentItems.Add(item);
                    equipmentItemDictionary[itemDataSo] = item;
                    OnEquipmentItemListChanged(equipmentItems);
                    break;
                case ItemType.Miscellaneous:
                    if (isNewItem) miscellaneousItems.Add(item);
                    miscellaneousItemDictionary[itemDataSo] = item;
                    OnMiscellaneousItemListChanged(miscellaneousItems);
                    break;
            }

            OnItemListChanged(items);
            IncrementCurrentWeight(itemDataSo.weight);
        }

        public virtual void RemoveItem(InventoryItem inventoryItem)
        {
            RemoveItem(inventoryItem.itemDataSo, inventoryItem.stackSize);
        }

        public virtual void RemoveItem(ItemDataSO itemDataSo, int amount)
        {
            if (itemDictionary.ContainsKey(itemDataSo))
            {
                itemDictionary[itemDataSo].RemoveFromStack(amount);
                switch (itemDataSo.itemType)
                {
                    case ItemType.Material:
                        materialItemDictionary[itemDataSo].RemoveFromStack(amount);
                        OnMaterialItemListChanged(materialItems);
                        break;
                    case ItemType.Consumable:
                        consumableItemDictionary[itemDataSo].RemoveFromStack(amount);
                        OnConsumableItemListChanged(consumableItems);
                        break;
                    case ItemType.Equipment:
                        equipmentItemDictionary[itemDataSo].RemoveFromStack(amount);
                        OnEquipmentItemListChanged(equipmentItems);
                        break;
                    case ItemType.Miscellaneous:
                        miscellaneousItemDictionary[itemDataSo].RemoveFromStack(amount);
                        OnMiscellaneousItemListChanged(miscellaneousItems);
                        break;
                }

                if (itemDictionary[itemDataSo].stackSize <= 0)
                {
                    items.Remove(itemDictionary[itemDataSo]);
                    itemDictionary.Remove(itemDataSo);
                    switch (itemDataSo.itemType)
                    {
                        case ItemType.Material:
                            materialItems.Remove(materialItemDictionary[itemDataSo]);
                            materialItemDictionary.Remove(itemDataSo);
                            OnMaterialItemListChanged(materialItems);
                            break;
                        case ItemType.Consumable:
                            consumableItems.Remove(consumableItemDictionary[itemDataSo]);
                            consumableItemDictionary.Remove(itemDataSo);
                            OnConsumableItemListChanged(consumableItems);
                            break;
                        case ItemType.Equipment:
                            equipmentItems.Remove(equipmentItemDictionary[itemDataSo]);
                            equipmentItemDictionary.Remove(itemDataSo);
                            OnEquipmentItemListChanged(equipmentItems);
                            break;
                        case ItemType.Miscellaneous:
                            miscellaneousItems.Remove(miscellaneousItemDictionary[itemDataSo]);
                            miscellaneousItemDictionary.Remove(itemDataSo);
                            OnMiscellaneousItemListChanged(miscellaneousItems);
                            break;
                    }
                }

                OnItemListChanged(items);
                DecrementCurrentWeight(itemDataSo.weight * amount);
            }
        }

        public virtual void RemoveItem(ItemDataSO itemDataSo)
        {
            if (itemDictionary.ContainsKey(itemDataSo))
            {
                itemDictionary[itemDataSo].RemoveFromStack();
                switch (itemDataSo.itemType)
                {
                    case ItemType.Material:
                        materialItemDictionary[itemDataSo].RemoveFromStack();
                        OnMaterialItemListChanged(materialItems);
                        break;
                    case ItemType.Consumable:
                        consumableItemDictionary[itemDataSo].RemoveFromStack();
                        OnConsumableItemListChanged(consumableItems);
                        break;
                    case ItemType.Equipment:
                        equipmentItemDictionary[itemDataSo].RemoveFromStack();
                        OnEquipmentItemListChanged(equipmentItems);
                        break;
                    case ItemType.Miscellaneous:
                        miscellaneousItemDictionary[itemDataSo].RemoveFromStack();
                        OnMiscellaneousItemListChanged(miscellaneousItems);
                        break;
                }

                if (itemDictionary[itemDataSo].stackSize <= 0)
                {
                    items.Remove(itemDictionary[itemDataSo]);
                    itemDictionary.Remove(itemDataSo);
                    switch (itemDataSo.itemType)
                    {
                        case ItemType.Material:
                            materialItems.Remove(materialItemDictionary[itemDataSo]);
                            materialItemDictionary.Remove(itemDataSo);
                            OnMaterialItemListChanged(materialItems);
                            break;
                        case ItemType.Consumable:
                            consumableItems.Remove(consumableItemDictionary[itemDataSo]);
                            consumableItemDictionary.Remove(itemDataSo);
                            OnConsumableItemListChanged(consumableItems);
                            break;
                        case ItemType.Equipment:
                            equipmentItems.Remove(equipmentItemDictionary[itemDataSo]);
                            equipmentItemDictionary.Remove(itemDataSo);
                            OnEquipmentItemListChanged(equipmentItems);
                            break;
                        case ItemType.Miscellaneous:
                            miscellaneousItems.Remove(miscellaneousItemDictionary[itemDataSo]);
                            miscellaneousItemDictionary.Remove(itemDataSo);
                            OnMiscellaneousItemListChanged(miscellaneousItems);
                            break;
                    }
                }

                OnItemListChanged(items);
                DecrementCurrentWeight(itemDataSo.weight);
            }
        }
    }
}