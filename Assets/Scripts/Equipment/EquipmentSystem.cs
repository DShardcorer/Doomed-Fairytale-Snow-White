using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSystem: ILifecycle<Entity>
{
    private Entity _parent;
    public Entity Entity => _parent;
    private Dictionary<EquipmentSlotType, ItemData_Equipment> _equippedItems = new Dictionary<EquipmentSlotType, ItemData_Equipment>();
    public IReadOnlyDictionary<EquipmentSlotType, ItemData_Equipment> EquippedItems => _equippedItems;

    public event EventHandler<IReadOnlyDictionary<EquipmentSlotType, ItemData_Equipment>> OnEquipmentChanged;

    public void Initialize(Entity parent)
    {
        _parent = parent;
    }
    public void Dispose()
    {
        _equippedItems.Clear();
        _equippedItems = null;
        OnEquipmentChanged = null;
        _parent = null;
    }   

    public void EquipItem(ItemData_Equipment item)
    {
        if (_equippedItems.ContainsKey(item.equipmentSlotType))
        {
            _equippedItems[item.equipmentSlotType] = item;
        }
        else
        {
            _equippedItems.Add(item.equipmentSlotType, item);
        }
        OnEquipmentChanged?.Invoke(this, _equippedItems);
    }

    public void UnequipItem(EquipmentSlotType slotType)
    {
        if (_equippedItems.ContainsKey(slotType))
        {
            _equippedItems.Remove(slotType);
        }
        OnEquipmentChanged?.Invoke(this, _equippedItems);
    }
    public ItemData_Equipment GetEquippedItem(EquipmentSlotType slotType)
    {
        if (_equippedItems.ContainsKey(slotType))
        {
            return _equippedItems[slotType];
        }
        return null;
    }
}
