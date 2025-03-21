using System;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public ItemData ItemData { get; private set; }
    public int stackSize;

    public InventoryItem(ItemData itemData)
    {
        ItemData = itemData;
    }

    public void AddToStack()
    {
        stackSize++;
    }

    public void RemoveFromStack()
    {
        stackSize--;
    }

}
