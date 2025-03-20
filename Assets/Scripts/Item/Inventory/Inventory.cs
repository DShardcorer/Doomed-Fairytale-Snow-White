using System;
using System.Collections.Generic;

public class Inventory
{
    private Entity _entity;
    public Entity Entity => _entity;

    public List<InventoryItem> items;
    public Dictionary<ItemData, InventoryItem> itemDictionary;


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

    public void AddItem(ItemData itemData)
    {
        if (itemDictionary.ContainsKey(itemData))
        {
            itemDictionary[itemData].AddToStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(itemData);
            items.Add(newItem);
            itemDictionary.Add(itemData, newItem);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveItem(ItemData itemData)
    {
        if (itemDictionary.ContainsKey(itemData))
        {
            itemDictionary[itemData].RemoveFromStack();
            if (itemDictionary[itemData].stackSize <= 0)
            {
                items.Remove(itemDictionary[itemData]);
                itemDictionary.Remove(itemData);
            }
            OnItemListChanged?.Invoke(this, EventArgs.Empty);
        }
    }


}
