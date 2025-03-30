using System;
using UnityEngine;

public class PlayerEquipmentSystem : EquipmentSystem
{
    public override void Initialize(Entity parent)
    {
        // For the player, set the parent and subscribe to equipment UI events.
        _parent = parent;
        PlayerEquipmentEventSystem.OnEquipmentEquipped += EquipmentInventoryUI_OnItemEquipped;
        PlayerEquipmentEventSystem.OnEquipmentUnequippedUnequipped += PlayerEquipmentUI_OnItemUnequipped;
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
}
