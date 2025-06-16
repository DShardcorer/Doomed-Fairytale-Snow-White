using System;
using DefaultNamespace.EventSystem.Barter;
using EntitySystems.PlayerSystems;
using GeneralManagers;
using Item;
using Item.Inventory;
using UnityEngine;

namespace DefaultNamespace.BarterSystem
{
    public class BarterManager : MonoBehaviour, ILifecycle<GameManager>
    {
        private GameManager _parent;
        private InventorySystem playerInventory;
        private InventorySystem npcInventory;
        private BarteredItemsHolder playerBarteredItemsHolder = new BarteredItemsHolder();
        private BarteredItemsHolder npcBarteredItemsHolder = new BarteredItemsHolder();
        
        public InventorySystem PlayerInventory => playerInventory;
        public InventorySystem NpcInventory => npcInventory;
        public BarteredItemsHolder PlayerBarteredItemsHolder => playerBarteredItemsHolder;
        public BarteredItemsHolder NpcBarteredItemsHolder => npcBarteredItemsHolder;

        public void Initialize(GameManager parent)
        {
            _parent = parent;
            // Initialization logic for barter system
            BarterEventSystem.OnBarterStart += OnBarterStart;
            BarterEventSystem.OnBarterComplete += CompleteBarter;
        }

        private void OnBarterStart(BarterEventSystem.BarterStartEventArgs obj)
        {
            StartBarter(obj.PlayerInventory, obj.NpcInventory);
        }

        public void StartBarter(InventorySystem playerInventory, InventorySystem npcInventory)
        {
            this.playerInventory = playerInventory;
            this.npcInventory = npcInventory;

            //Debug.log every item in npc inventory
            Debug.LogWarning("Starting barter with NPC. NPC Inventory Items:");
            foreach (var item in npcInventory.ItemList)
            {
                Debug.LogWarning($"Item: {item.itemDataSo.itemName}, Value: {item.itemDataSo.value}, Stack Size: {item.stackSize}");
            }
        }
        public void AddPlayerBarteredItem(InventoryItem item)
        {
            playerBarteredItemsHolder.AddItem(item);
            playerInventory.RemoveItem(item);
        }
        public void AddNpcBarteredItem(InventoryItem item)
        {
            npcBarteredItemsHolder.AddItem(item);
            npcInventory.RemoveItem(item);
        }

        public void RemovePlayerBarteredItem(InventoryItem item)
        {
            playerBarteredItemsHolder.RemoveItem(item);
            playerInventory.AddItem(item);
        }
        public void RemoveNpcBarteredItem(InventoryItem item)
        {
            npcBarteredItemsHolder.RemoveItem(item);
            npcInventory.AddItem(item);
        }

        public void CompleteBarter()
        {
            //Only complete barter if player bartered items have higher value than NPC bartered items
            if (playerBarteredItemsHolder.TotalValue >= npcBarteredItemsHolder.TotalValue)
            {
                // Logic to complete barter
                npcInventory.AddItems(playerBarteredItemsHolder.BarteredItems);
                playerInventory.AddItems(npcBarteredItemsHolder.BarteredItems);
                playerBarteredItemsHolder.ClearItems();
                npcBarteredItemsHolder.ClearItems();
                Debug.Log("Barter completed successfully.");
            }
            else
            {
                Debug.Log("Barter failed: Player's items are not of higher value than NPC's items.");
            }
        }

        public void Dispose()
        {
            // Cleanup logic for barter system
            _parent = null;
            Destroy(gameObject);
        }
    }
}