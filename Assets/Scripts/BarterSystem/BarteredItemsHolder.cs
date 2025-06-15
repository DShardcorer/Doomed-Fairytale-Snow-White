using System;
using System.Collections.Generic;
using Item.Inventory;


namespace DefaultNamespace.BarterSystem
{
    public class BarteredItemsHolder
    {
        public List<InventoryItem> BarteredItems = new List<InventoryItem>();
        private int totalValue = 0;
        public int TotalValue => totalValue;
        
        public Action<List<InventoryItem>> OnItemsListChanged;
        public void AddItem(InventoryItem item)
        {
            BarteredItems.Add(item);
            totalValue += item.itemDataSo.value * item.stackSize; // Assuming itemDataSo has a value property
            OnItemsListChanged?.Invoke(BarteredItems);
        }
        public void RemoveItem(InventoryItem item)
        {
            if (BarteredItems.Contains(item))
            {
                BarteredItems.Remove(item);
                totalValue -= item.itemDataSo.value * item.stackSize; // Assuming itemDataSo has a value property
                OnItemsListChanged?.Invoke(BarteredItems);
            }
        }
        public void ClearItems()
        {
            BarteredItems.Clear();
            totalValue = 0;
            OnItemsListChanged?.Invoke(BarteredItems);
        }
        public bool ContainsItem(InventoryItem item)
        {
            return BarteredItems.Contains(item);
        }
        
    }
}