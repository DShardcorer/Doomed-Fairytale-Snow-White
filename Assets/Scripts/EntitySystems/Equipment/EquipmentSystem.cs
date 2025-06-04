using System.Collections.Generic;
using GeneralManagers;
using Item.Inventory;

namespace EntitySystems.Equipment
{
    public class EquipmentSystem : ILifecycle<Entity.Entity>
    {
        protected Entity.Entity _parent;
        public Entity.Entity Entity => _parent;
    
        protected Dictionary<EquipmentSlotType, EquipmentInventoryItem> _equippedItems = new Dictionary<EquipmentSlotType, EquipmentInventoryItem>();
        public IReadOnlyDictionary<EquipmentSlotType, EquipmentInventoryItem> EquippedItems => _equippedItems;

        public virtual void Initialize(Entity.Entity parent)
        {
            _parent = parent;

        }

        public virtual void InvokeInitialEvents()
        {
            // Derived classes can decide whether to invoke events.
        }

        public virtual void Dispose()
        {
            _equippedItems.Clear();
            _equippedItems = null;
            _parent = null;
        }

        public virtual void EquipItem(EquipmentInventoryItem item)
        {
            if (_equippedItems.ContainsKey(item.SoEquipmentDataSo.equipmentSlotType))
            {
                _equippedItems[item.SoEquipmentDataSo.equipmentSlotType] = item;
            }
            else
            {
                _equippedItems.Add(item.SoEquipmentDataSo.equipmentSlotType, item);
            }
            item.isEquipped = true;
        }

        public virtual void UnequipItem(EquipmentInventoryItem item)
        {
            if (_equippedItems.ContainsKey(item.SoEquipmentDataSo.equipmentSlotType))
            {
                _equippedItems.Remove(item.SoEquipmentDataSo.equipmentSlotType);
            }
            item.isEquipped = false;
        }

        public EquipmentInventoryItem GetEquippedItem(EquipmentSlotType slotType)
        {
            return _equippedItems.ContainsKey(slotType) ? _equippedItems[slotType] : null;
        }

        // If needed, you can also change the event handler methods to be protected virtual.

    }
}
