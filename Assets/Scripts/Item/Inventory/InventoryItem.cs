using System;

namespace Item.Inventory
{
    [Serializable]
    public class InventoryItem
    {
        public ItemData ItemData;
        public int stackSize;

        public InventoryItem(ItemData itemData)
        {
            ItemData = itemData;
        }
        public void AddToStack(int amount)
        {
            stackSize += amount;
        }
        public void RemoveFromStack(int amount)
        {
            stackSize -= amount;
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
}
