using System.Collections.Generic;
using EntitySystems.WeaponSystem;
using GeneralManagers;
using Item;
using Item.Inventory;
using UnityEngine;

namespace EntitySystems.Equipment
{
    public class EquipmentSystem : ILifecycle<EntityBase.Entity>
    {
        protected EntityBase.Entity _parent;
        public EntityBase.Entity Entity => _parent;

        protected Dictionary<EquipmentSlotType, EquipmentInventoryItem> _equippedItems =
            new Dictionary<EquipmentSlotType, EquipmentInventoryItem>();

        public IReadOnlyDictionary<EquipmentSlotType, EquipmentInventoryItem> EquippedItems => _equippedItems;

        public virtual void Initialize(EntityBase.Entity parent)
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

        public bool IsPrimaryWeaponEquipped()
        {
            return _equippedItems.ContainsKey(EquipmentSlotType.PrimaryWeapon);
        }

        public bool IsSecondaryWeaponEquipped()
        {
            return _equippedItems.ContainsKey(EquipmentSlotType.SecondaryWeapon);
        }

        public virtual void EquipItem(EquipmentInventoryItem item)
        {
            if (_equippedItems.ContainsKey(item.EquipmentDataSo.equipmentSlotType))
            {
                _equippedItems[item.EquipmentDataSo.equipmentSlotType] = item;
            }
            else
            {
                _equippedItems.Add(item.EquipmentDataSo.equipmentSlotType, item);
            }

            // Initialize equipment regardless of whether it's a new or replaced item
            if (item.EquipmentDataSo.equipmentSlotType == EquipmentSlotType.PrimaryWeapon)
            {
                ItemDataSOEquipment_Weapon weaponItemData = item.EquipmentDataSo as ItemDataSOEquipment_Weapon;
                if (weaponItemData == null)
                {
                    Debug.LogError("ItemDataSOEquipment_Weapon is required for PrimaryWeapon slot.");
                    return;
                }

                _parent.WeaponSystem.EquipWeapon(weaponItemData.WeaponData,
                    WeaponSystem.WeaponSystem.WeaponSlotType.Primary);
            }
            // Add handling for secondary weapon if needed similarly

            item.isEquipped = true;
        }

        public virtual void UnequipItem(EquipmentInventoryItem item)
        {
            if (_equippedItems.ContainsKey(item.EquipmentDataSo.equipmentSlotType))
            {
                if (item.EquipmentDataSo.equipmentSlotType == EquipmentSlotType.PrimaryWeapon)
                {
                    _parent.WeaponSystem.UnequipWeapon(WeaponSystem.WeaponSystem.WeaponSlotType.Primary);
                }
                // Add handling for secondary weapon if needed
                
                _equippedItems.Remove(item.EquipmentDataSo.equipmentSlotType);
            }

            item.isEquipped = false;
        }

        public EquipmentInventoryItem GetEquippedItem(EquipmentSlotType slotType)
        {
            return _equippedItems.ContainsKey(slotType) ? _equippedItems[slotType] : null;
        }
    }
}