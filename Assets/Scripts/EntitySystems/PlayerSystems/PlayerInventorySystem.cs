using System.Collections.Generic;
using EventSystem.Player;
using Item.Inventory;

namespace EntitySystems.PlayerSystems
{
    public class PlayerInventorySystem : InventorySystem
    {
        public override void InvokeInitialEvents()
        {
            PlayerInventoryEventSystem.InvokeItemListChanged(items);
            PlayerInventoryEventSystem.InvokeMaterialItemListChanged(materialItems);
            PlayerInventoryEventSystem.InvokeConsumableItemListChanged(consumableItems);
            PlayerInventoryEventSystem.InvokeEquipmentItemListChanged(equipmentItems);
            PlayerInventoryEventSystem.InvokeMiscellaneousItemListChanged(miscellaneousItems);
            PlayerInventoryEventSystem.InvokeWeightChanged(_currentWeight, _capacity);
        }

        protected override void OnItemListChanged(List<InventoryItem> items)
        {
            PlayerInventoryEventSystem.InvokeItemListChanged(items);
        }

        protected override void OnMaterialItemListChanged(List<InventoryItem> materialItems)
        {
            PlayerInventoryEventSystem.InvokeMaterialItemListChanged(materialItems);
        }

        protected override void OnConsumableItemListChanged(List<InventoryItem> consumableItems)
        {
            PlayerInventoryEventSystem.InvokeConsumableItemListChanged(consumableItems);
        }

        protected override void OnEquipmentItemListChanged(List<InventoryItem> equipmentItems)
        {
            PlayerInventoryEventSystem.InvokeEquipmentItemListChanged(equipmentItems);
        }

        protected override void OnMiscellaneousItemListChanged(List<InventoryItem> miscellaneousItems)
        {
            PlayerInventoryEventSystem.InvokeMiscellaneousItemListChanged(miscellaneousItems);
        }

        protected override void OnWeightChanged(float currentWeight, float capacity)
        {
            PlayerInventoryEventSystem.InvokeWeightChanged(currentWeight, capacity);
        }
    }
}
