using System;
using UnityEngine.Serialization;

namespace Item.Inventory
{
    [Serializable]
    public class InventoryItem
    {
        [FormerlySerializedAs("ItemData")] 
        public ItemDataSO itemDataSo;
        public int stackSize;

        public InventoryItem(ItemDataSO itemDataSo)
        {
            this.itemDataSo = itemDataSo;
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
