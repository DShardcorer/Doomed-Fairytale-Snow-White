using System;
using System.Collections.Generic;

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
    }
    public virtual void Initialize(Entity entity)
    {
        _entity = entity;
    }
    public event EventHandler OnItemListChanged;
    public event EventHandler OnMaterialItemListChanged;
    public event EventHandler OnConsumableItemListChanged;
    public event EventHandler OnEquipmentItemListChanged;
    public event EventHandler OnMiscellaneousItemListChanged;

    public void AddItem(ItemData itemData)
    {
        if (itemDictionary.ContainsKey(itemData))
        {
            itemDictionary[itemData].AddToStack();
            switch (itemData.itemType)
            {
                case ItemType.Material:
                    materialItemDictionary[itemData].AddToStack();
                    OnMaterialItemListChanged?.Invoke(this, EventArgs.Empty);
                    break;
                case ItemType.Consumable:
                    consumableItemDictionary[itemData].AddToStack();
                    OnConsumableItemListChanged?.Invoke(this, EventArgs.Empty);
                    break;
                case ItemType.Equipment:
                    equipmentItemDictionary[itemData].AddToStack();
                    OnEquipmentItemListChanged?.Invoke(this, EventArgs.Empty);
                    break;
                case ItemType.Miscellaneous:
                    miscellaneousItemDictionary[itemData].AddToStack();
                    OnMiscellaneousItemListChanged?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }
        else
        {
            InventoryItem newItem = new InventoryItem(itemData);
            items.Add(newItem);
            itemDictionary.Add(itemData, newItem);
            switch (itemData.itemType)
            {
                case ItemType.Material:
                    materialItems.Add(newItem);
                    materialItemDictionary.Add(itemData, newItem);
                    OnMaterialItemListChanged?.Invoke(this, EventArgs.Empty);
                    break;
                case ItemType.Consumable:
                    consumableItems.Add(newItem);
                    consumableItemDictionary.Add(itemData, newItem);
                    OnConsumableItemListChanged?.Invoke(this, EventArgs.Empty);
                    break;
                case ItemType.Equipment:
                    equipmentItems.Add(newItem);
                    equipmentItemDictionary.Add(itemData, newItem);
                    OnEquipmentItemListChanged?.Invoke(this, EventArgs.Empty);
                    break;
                case ItemType.Miscellaneous:
                    miscellaneousItems.Add(newItem);
                    miscellaneousItemDictionary.Add(itemData, newItem);
                    OnMiscellaneousItemListChanged?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
        
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
                    OnMaterialItemListChanged?.Invoke(this, EventArgs.Empty);
                    break;
                case ItemType.Consumable:
                    consumableItemDictionary[itemData].RemoveFromStack();
                    OnConsumableItemListChanged?.Invoke(this, EventArgs.Empty);
                    break;
                case ItemType.Equipment:
                    equipmentItemDictionary[itemData].RemoveFromStack();
                    OnEquipmentItemListChanged?.Invoke(this, EventArgs.Empty);
                    break;
                case ItemType.Miscellaneous:
                    miscellaneousItemDictionary[itemData].RemoveFromStack();
                    OnMiscellaneousItemListChanged?.Invoke(this, EventArgs.Empty);
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
                        OnMaterialItemListChanged?.Invoke(this, EventArgs.Empty);
                        break;
                    case ItemType.Consumable:
                        consumableItems.Remove(consumableItemDictionary[itemData]);
                        consumableItemDictionary.Remove(itemData);
                        OnConsumableItemListChanged?.Invoke(this, EventArgs.Empty);
                        break;
                    case ItemType.Equipment:
                        equipmentItems.Remove(equipmentItemDictionary[itemData]);
                        equipmentItemDictionary.Remove(itemData);
                        OnEquipmentItemListChanged?.Invoke(this, EventArgs.Empty);
                        break;
                    case ItemType.Miscellaneous:
                        miscellaneousItems.Remove(miscellaneousItemDictionary[itemData]);
                        miscellaneousItemDictionary.Remove(itemData);
                        OnMiscellaneousItemListChanged?.Invoke(this, EventArgs.Empty);
                        break;
                }
            }
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }



}
