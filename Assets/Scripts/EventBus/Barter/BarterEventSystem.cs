using System;
using DefaultNamespace.BarterSystem;
using Item.Inventory;

namespace DefaultNamespace.EventSystem.Barter
{
    public static class BarterEventSystem
    {
        public class BarterStartEventArgs : EventArgs
        {
            public InventorySystem PlayerInventory;
            public InventorySystem NpcInventory;
            public BarterStartEventArgs(InventorySystem playerInventory, InventorySystem npcInventory)
            {
                PlayerInventory = playerInventory;
                NpcInventory = npcInventory;
            }
        }
        public static Action<BarterStartEventArgs> OnBarterStart;

        public static void InvokeBarterStart(BarterStartEventArgs args)
        {
            OnBarterStart?.Invoke(args);
        }
        
        public static Action OnBarterComplete;
        public static void InvokeBarterComplete()
        {
            OnBarterComplete?.Invoke();
        }
    }
}