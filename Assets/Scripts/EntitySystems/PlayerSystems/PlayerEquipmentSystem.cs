using System.Collections.Generic;
using DataPersistence;
using DataPersistence.Data;
using EntitySystems.Equipment;
using EventSystem.Player;
using Item.Inventory;
using UnityEngine;

namespace EntitySystems.PlayerSystems
{
    public class PlayerEquipmentSystem : EquipmentSystem, IDataPersistence
    {
        public override void Initialize(Entity.Entity parent)
        {
            // For the player, set the parent and subscribe to equipment UI events.
            _parent = parent;
            PlayerEquipmentEventSystem.OnEquipmentEquipped += EquipmentInventoryUI_OnItemEquipped;
            PlayerEquipmentEventSystem.OnEquipmentUnequippedUnequipped += PlayerEquipmentUI_OnItemUnequipped;
            ((IDataPersistence)this).AddDataPersistenceObject();
        }

        public override void InvokeInitialEvents()
        {
            // Immediately notify that equipment has changed.
            PlayerEquipmentEventSystem.InvokePlayerEquipmentSystem_EquipmentChanged(_equippedItems);
        }

        public override void Dispose()
        {
            // Unsubscribe from events before disposing.
            PlayerEquipmentEventSystem.OnEquipmentEquipped -= EquipmentInventoryUI_OnItemEquipped;
            PlayerEquipmentEventSystem.OnEquipmentUnequippedUnequipped -= PlayerEquipmentUI_OnItemUnequipped;
            base.Dispose();
        }

        public override void EquipItem(EquipmentInventoryItem item)
        {
            base.EquipItem(item);
            // Always invoke the equipment changed event for the player.
            PlayerEquipmentEventSystem.InvokePlayerEquipmentSystem_EquipmentChanged(_equippedItems);
        }

        public override void UnequipItem(EquipmentInventoryItem item)
        {
            base.UnequipItem(item);
            PlayerEquipmentEventSystem.InvokePlayerEquipmentSystem_EquipmentChanged(_equippedItems);
        }

        // Optionally override the event handlers if any additional behavior is needed.
        protected void EquipmentInventoryUI_OnItemEquipped(object sender, EquipmentInventoryItem e)
        {
            EquipItem(e);
        }

        protected void PlayerEquipmentUI_OnItemUnequipped(object sender, EquipmentInventoryItem e)
        {
            UnequipItem(e);
        }

        public void LoadData(GameData saveData)
        {
            if (saveData.PlayerEquipmentSystemSaveData != null)
            {
                PlayerInventorySystem inventorySystem = _parent.InventorySystem as PlayerInventorySystem;
                if (inventorySystem != null)
                {
                    SaveLoadHelper.LoadFromSaveData(this, saveData.PlayerEquipmentSystemSaveData, inventorySystem);
                }
            }
            InvokeInitialEvents();
        }

        public void SaveData(ref GameData data)
        {
            data.PlayerEquipmentSystemSaveData = SaveLoadHelper.CreateSaveData(this);
        }
    }
}