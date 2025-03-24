using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSystem : ILifecycle<Entity>
{
    protected Entity _parent;
    public Entity Entity => _parent;
    
    protected Dictionary<EquipmentSlotType, EquipmentInventoryItem> _equippedItems = new Dictionary<EquipmentSlotType, EquipmentInventoryItem>();
    public IReadOnlyDictionary<EquipmentSlotType, EquipmentInventoryItem> EquippedItems => _equippedItems;

    public virtual void Initialize(Entity parent)
    {
        _parent = parent;
        // Common initialization for any entity, but no event subscriptions here.
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
        if (_equippedItems.ContainsKey(item.EquipmentData.equipmentSlotType))
        {
            _equippedItems[item.EquipmentData.equipmentSlotType] = item;
        }
        else
        {
            _equippedItems.Add(item.EquipmentData.equipmentSlotType, item);
        }
        item.isEquipped = true;
    }

    public virtual void UnequipItem(EquipmentInventoryItem item)
    {
        if (_equippedItems.ContainsKey(item.EquipmentData.equipmentSlotType))
        {
            _equippedItems.Remove(item.EquipmentData.equipmentSlotType);
        }
        item.isEquipped = false;
    }

    public EquipmentInventoryItem GetEquippedItem(EquipmentSlotType slotType)
    {
        return _equippedItems.ContainsKey(slotType) ? _equippedItems[slotType] : null;
    }

    // If needed, you can also change the event handler methods to be protected virtual.

}
