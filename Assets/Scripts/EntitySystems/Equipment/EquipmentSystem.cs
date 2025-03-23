using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSystem : ILifecycle<Entity>
{
    private Entity _parent;
    public Entity Entity => _parent;
    private Dictionary<EquipmentSlotType, EquipmentInventoryItem> _equippedItems = new Dictionary<EquipmentSlotType, EquipmentInventoryItem>();
    public IReadOnlyDictionary<EquipmentSlotType, EquipmentInventoryItem> EquippedItems => _equippedItems;

    // Helper method to check if the parent entity is the player.
    private bool IsPlayerEntity() => _parent is Player; // Or use _parent.CompareTag("Player") if you tag your player

    public void Initialize(Entity parent)
    {
        _parent = parent;
        // Only subscribe to player-specific events if this entity is the player.
        if (IsPlayerEntity())
        {
            PlayerEquipmentEventSystem.EquipmentInventoryUI_OnItemEquipped += EquipmentInventoryUI_OnItemEquipped;
            PlayerEquipmentEventSystem.PlayerEquipmentUI_OnItemUnequipped += PlayerEquipmentUI_OnItemUnequipped;
        }
    }

    public void InvokeInitialEvents()
    {
        if (IsPlayerEntity())
        {
            PlayerEquipmentEventSystem.InvokePlayerEquipmentSystem_OnEquipmentChanged(_equippedItems);
        }
    }

    private void PlayerEquipmentUI_OnItemUnequipped(object sender, EquipmentInventoryItem e)
    {
        UnequipItem(e);
        if (IsPlayerEntity())
        {
            PlayerEquipmentEventSystem.InvokePlayerEquipmentSystem_OnEquipmentChanged(_equippedItems);
        }
    }

    private void EquipmentInventoryUI_OnItemEquipped(object sender, EquipmentInventoryItem e)
    {
        Debug.Log("EquipmentInventoryUI_OnItemEquipped");
        EquipItem(e);
        if (IsPlayerEntity())
        {
            PlayerEquipmentEventSystem.InvokePlayerEquipmentSystem_OnEquipmentChanged(_equippedItems);
        }
    }

    public void Dispose()
    {
        _equippedItems.Clear();
        _equippedItems = null;
        _parent = null;
        if (IsPlayerEntity())
        {
            PlayerEquipmentEventSystem.EquipmentInventoryUI_OnItemEquipped -= EquipmentInventoryUI_OnItemEquipped;
            PlayerEquipmentEventSystem.PlayerEquipmentUI_OnItemUnequipped -= PlayerEquipmentUI_OnItemUnequipped;
        }
    }

    public void EquipItem(EquipmentInventoryItem item)
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
        if (IsPlayerEntity())
        {
            PlayerEquipmentEventSystem.InvokePlayerEquipmentSystem_OnEquipmentChanged(_equippedItems);
        }
    }

    public void UnequipItem(EquipmentInventoryItem item)
    {
        if (_equippedItems.ContainsKey(item.EquipmentData.equipmentSlotType))
        {
            _equippedItems.Remove(item.EquipmentData.equipmentSlotType);
        }
        item.isEquipped = false;
        if (IsPlayerEntity())
        {
            PlayerEquipmentEventSystem.InvokePlayerEquipmentSystem_OnEquipmentChanged(_equippedItems);
        }
    }

    public EquipmentInventoryItem GetEquippedItem(EquipmentSlotType slotType)
    {
        if (_equippedItems.ContainsKey(slotType))
        {
            return _equippedItems[slotType];
        }
        return null;
    }
}
