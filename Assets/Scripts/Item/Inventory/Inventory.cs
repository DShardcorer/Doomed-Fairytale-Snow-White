using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private Entity _entity;
    public Entity Entity => _entity;

    private float _capacity;
    public float Capacity => _capacity;
    private float _currentWeight;
    public float CurrentWeight => _currentWeight;

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

    public Inventory()
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
    
    public virtual void Initialize(Entity entity)
    {
        _entity = entity;
        _capacity = entity.StatSystem.AbilityStatBoard.Strength.ModifiedValue * 10;
        UpdateCurrentWeight();
    }

    // Helper method to check if the owning entity is the player.
    private bool IsPlayerEntity() => _entity is Player; // Or use _entity.CompareTag("Player")

    public void InvokeInitialEvents()
    {
        if (IsPlayerEntity())
        {
            PlayerInventoryEventSystem.InvokeItemListChanged(items);
            PlayerInventoryEventSystem.InvokeMaterialItemListChanged(materialItems);
            PlayerInventoryEventSystem.InvokeConsumableItemListChanged(consumableItems);
            PlayerInventoryEventSystem.InvokeEquipmentItemListChanged(equipmentItems);
            PlayerInventoryEventSystem.InvokeMiscellaneousItemListChanged(miscellaneousItems);
            PlayerInventoryEventSystem.InvokeWeightChanged(_currentWeight, _capacity);
        }
    }

    public void UpdateCurrentWeight()
    {
        _currentWeight = 0;
        foreach (InventoryItem item in items)
        {
            _currentWeight += item.ItemData.weight * item.stackSize;
        }
        if (IsPlayerEntity())
        {
            PlayerInventoryEventSystem.InvokeWeightChanged(_currentWeight, _capacity);
        }
    }

    public void IncrementCurrentWeight(float weight)
    {
        _currentWeight += weight;
        if (IsPlayerEntity())
        {
            PlayerInventoryEventSystem.InvokeWeightChanged(_currentWeight, _capacity);
        }
    }

    public void DecrementCurrentWeight(float weight)
    {
        _currentWeight -= weight;
        if (IsPlayerEntity())
        {
            PlayerInventoryEventSystem.InvokeWeightChanged(_currentWeight, _capacity);
        }
    }

    public void AddItem(ItemData itemData)
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
                if (IsPlayerEntity())
                    PlayerInventoryEventSystem.InvokeMaterialItemListChanged(materialItems);
                break;
            case ItemType.Consumable:
                if (isNewItem) consumableItems.Add(item);
                consumableItemDictionary[itemData] = item;
                if (IsPlayerEntity())
                    PlayerInventoryEventSystem.InvokeConsumableItemListChanged(consumableItems);
                break;
            case ItemType.Equipment:
                if (isNewItem) equipmentItems.Add(item);
                equipmentItemDictionary[itemData] = item;
                if (IsPlayerEntity())
                    PlayerInventoryEventSystem.InvokeEquipmentItemListChanged(equipmentItems);
                break;
            case ItemType.Miscellaneous:
                if (isNewItem) miscellaneousItems.Add(item);
                miscellaneousItemDictionary[itemData] = item;
                if (IsPlayerEntity())
                    PlayerInventoryEventSystem.InvokeMiscellaneousItemListChanged(miscellaneousItems);
                break;
        }

        if (IsPlayerEntity())
        {
            PlayerInventoryEventSystem.InvokeItemListChanged(items);
        }
        IncrementCurrentWeight(itemData.weight);
    }

    public void RemoveItem(ItemData itemData)
    {
        if (itemDictionary.ContainsKey(itemData))
        {
            itemDictionary[itemData].RemoveFromStack();
            switch (itemData.itemType)
            {
                case ItemType.Material:
                    materialItemDictionary[itemData].RemoveFromStack();
                    if (IsPlayerEntity())
                        PlayerInventoryEventSystem.InvokeMaterialItemListChanged(materialItems);
                    break;
                case ItemType.Consumable:
                    consumableItemDictionary[itemData].RemoveFromStack();
                    if (IsPlayerEntity())
                        PlayerInventoryEventSystem.InvokeConsumableItemListChanged(consumableItems);
                    break;
                case ItemType.Equipment:
                    equipmentItemDictionary[itemData].RemoveFromStack();
                    if (IsPlayerEntity())
                        PlayerInventoryEventSystem.InvokeEquipmentItemListChanged(equipmentItems);
                    break;
                case ItemType.Miscellaneous:
                    miscellaneousItemDictionary[itemData].RemoveFromStack();
                    if (IsPlayerEntity())
                        PlayerInventoryEventSystem.InvokeMiscellaneousItemListChanged(miscellaneousItems);
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
                        if (IsPlayerEntity())
                            PlayerInventoryEventSystem.InvokeMaterialItemListChanged(materialItems);
                        break;
                    case ItemType.Consumable:
                        consumableItems.Remove(consumableItemDictionary[itemData]);
                        consumableItemDictionary.Remove(itemData);
                        if (IsPlayerEntity())
                            PlayerInventoryEventSystem.InvokeConsumableItemListChanged(consumableItems);
                        break;
                    case ItemType.Equipment:
                        equipmentItems.Remove(equipmentItemDictionary[itemData]);
                        equipmentItemDictionary.Remove(itemData);
                        if (IsPlayerEntity())
                            PlayerInventoryEventSystem.InvokeEquipmentItemListChanged(equipmentItems);
                        break;
                    case ItemType.Miscellaneous:
                        miscellaneousItems.Remove(miscellaneousItemDictionary[itemData]);
                        miscellaneousItemDictionary.Remove(itemData);
                        if (IsPlayerEntity())
                            PlayerInventoryEventSystem.InvokeMiscellaneousItemListChanged(miscellaneousItems);
                        break;
                }
            }
            if (IsPlayerEntity())
            {
                PlayerInventoryEventSystem.InvokeItemListChanged(items);
            }
            DecrementCurrentWeight(itemData.weight);
        }
    }
}
