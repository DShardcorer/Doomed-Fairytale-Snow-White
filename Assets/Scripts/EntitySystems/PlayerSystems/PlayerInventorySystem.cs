using System.Collections.Generic;
using DataPersistence;
using DataPersistence.Data;
using EventSystem.Player;
using Item;
using Item.Inventory;

namespace EntitySystems.PlayerSystems
{

    public class PlayerInventorySystem : InventorySystem, IDataPersistence
    {
        public override void Initialize(Entity.Entity entity)
        {
            base.Initialize(entity);
            ((IDataPersistence)this).AddDataPersistenceObject();
        }

        public override void InvokeInitialEvents()
        {
            PlayerInventoryEventSystem.InvokeItemListChanged(ItemList);
            PlayerInventoryEventSystem.InvokeMaterialItemListChanged(materialItems);
            PlayerInventoryEventSystem.InvokeConsumableItemListChanged(consumableItems);
            PlayerInventoryEventSystem.InvokeEquipmentItemListChanged(equipmentItems);
            PlayerInventoryEventSystem.InvokeMiscellaneousItemListChanged(miscellaneousItems);
            PlayerInventoryEventSystem.InvokeWeightChanged(currentWeight, capacity);
        }

        protected override void OnItemListChanged(List<InventoryItem> items)
        {
            base.OnItemListChanged(items);
            PlayerInventoryEventSystem.InvokeItemListChanged(items);
        }

        protected override void OnMaterialItemListChanged(List<InventoryItem> materialItems)
        {
            base.OnMaterialItemListChanged(materialItems);
            PlayerInventoryEventSystem.InvokeMaterialItemListChanged(materialItems);
        }

        protected override void OnConsumableItemListChanged(List<InventoryItem> consumableItems)
        {
            base.OnConsumableItemListChanged(consumableItems);
            PlayerInventoryEventSystem.InvokeConsumableItemListChanged(consumableItems);
        }

        protected override void OnEquipmentItemListChanged(List<InventoryItem> equipmentItems)
        {
            base.OnEquipmentItemListChanged(equipmentItems);
            PlayerInventoryEventSystem.InvokeEquipmentItemListChanged(equipmentItems);
        }

        protected override void OnMiscellaneousItemListChanged(List<InventoryItem> miscellaneousItems)
        {
            base.OnMiscellaneousItemListChanged(miscellaneousItems);
            PlayerInventoryEventSystem.InvokeMiscellaneousItemListChanged(miscellaneousItems);
        }

        protected override void OnWeightChanged(float currentWeight, float capacity)
        {
            base.OnWeightChanged(currentWeight, capacity);
            PlayerInventoryEventSystem.InvokeWeightChanged(currentWeight, capacity);
        }

        public void LoadData(GameData saveData)
        {
            SaveLoadHelper.LoadFromSaveData(this, saveData.playerInventorySaveData);
            // Invoke events to update the UI or other systems
            InvokeInitialEvents();
        }

        public void SaveData(ref GameData data)
        {
            data.playerInventorySaveData = SaveLoadHelper.CreateSaveData(this);
        }
    }
}
