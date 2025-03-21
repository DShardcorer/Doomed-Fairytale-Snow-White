using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private Entity _entity;
    public Entity Entity => _entity;

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
    }
    public event EventHandler<List<InventoryItem>> OnItemListChanged;
    public event EventHandler<List<InventoryItem>> OnMaterialItemListChanged;
    public event EventHandler<List<InventoryItem>> OnConsumableItemListChanged;
    public event EventHandler<List<InventoryItem>> OnEquipmentItemListChanged;
    public event EventHandler<List<InventoryItem>> OnMiscellaneousItemListChanged;

public void AddItem(ItemData itemData)
{
    InventoryItem item;
    bool isNewItem = !itemDictionary.TryGetValue(itemData, out item);

    if (isNewItem)
    {
        item = new InventoryItem(itemData);
        items.Add(item);
        itemDictionary[itemData] = item;
    }

    item.AddToStack();

    switch (itemData.itemType)
    {
        case ItemType.Material:
            if (isNewItem) materialItems.Add(item);
            materialItemDictionary[itemData] = item;
            OnMaterialItemListChanged?.Invoke(this, materialItems);
            break;
        case ItemType.Consumable:
            if (isNewItem) consumableItems.Add(item);
            consumableItemDictionary[itemData] = item;
            OnConsumableItemListChanged?.Invoke(this, consumableItems);
            break;
        case ItemType.Equipment:
            if (isNewItem) equipmentItems.Add(item);
            equipmentItemDictionary[itemData] = item;
            OnEquipmentItemListChanged?.Invoke(this, equipmentItems);
            break;
        case ItemType.Miscellaneous:
            if (isNewItem) miscellaneousItems.Add(item);
            miscellaneousItemDictionary[itemData] = item;
            OnMiscellaneousItemListChanged?.Invoke(this, miscellaneousItems);
            break;
    }

    OnItemListChanged?.Invoke(this, items);
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
                    OnMaterialItemListChanged?.Invoke(this, materialItems);
                    break;
                case ItemType.Consumable:
                    consumableItemDictionary[itemData].RemoveFromStack();
                    OnConsumableItemListChanged?.Invoke(this, consumableItems);
                    break;
                case ItemType.Equipment:
                    equipmentItemDictionary[itemData].RemoveFromStack();
                    OnEquipmentItemListChanged?.Invoke(this, equipmentItems);
                    break;
                case ItemType.Miscellaneous:
                    miscellaneousItemDictionary[itemData].RemoveFromStack();
                    OnMiscellaneousItemListChanged?.Invoke(this, miscellaneousItems);
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
                        OnMaterialItemListChanged?.Invoke(this, materialItems);
                        break;
                    case ItemType.Consumable:
                        consumableItems.Remove(consumableItemDictionary[itemData]);
                        consumableItemDictionary.Remove(itemData);
                        OnConsumableItemListChanged?.Invoke(this, consumableItems);
                        break;
                    case ItemType.Equipment:
                        equipmentItems.Remove(equipmentItemDictionary[itemData]);
                        equipmentItemDictionary.Remove(itemData);
                        OnEquipmentItemListChanged?.Invoke(this, equipmentItems);
                        break;
                    case ItemType.Miscellaneous:
                        miscellaneousItems.Remove(miscellaneousItemDictionary[itemData]);
                        miscellaneousItemDictionary.Remove(itemData);
                        OnMiscellaneousItemListChanged?.Invoke(this, miscellaneousItems);
                        break;
                }
            }
        }
        OnItemListChanged?.Invoke(this, items);
    }



}
