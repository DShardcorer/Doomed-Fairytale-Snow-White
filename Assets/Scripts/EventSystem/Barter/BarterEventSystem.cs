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
        public class BarterCompleteEventArgs : EventArgs
        {
            public BarteredItemsHolder PlayerBarteredItemsHolder;
            public BarteredItemsHolder NpcBarteredItemsHolder;
            public BarterCompleteEventArgs(BarteredItemsHolder playerBarteredItemsHolder, BarteredItemsHolder npcBarteredItemsHolder)
            {
                PlayerBarteredItemsHolder = playerBarteredItemsHolder;
                NpcBarteredItemsHolder = npcBarteredItemsHolder;
            }
        }
        public static Action<BarterCompleteEventArgs> OnBarterComplete;
        public static void InvokeBarterComplete(BarterCompleteEventArgs args)
        {
            OnBarterComplete?.Invoke(args);
        }
    }
}