using System;
using System.Collections.Generic;
using EntitySystems.Equipment;

namespace DataPersistence.Data
{
    [Serializable]
    public class EquipmentSlotSaveData
    {
        public EquipmentSlotType slotType;
        public string itemName;  // Unique identifier for the equipped item
    }

    [Serializable]
    public class EquipmentSystemSaveData
    {
        public List<EquipmentSlotSaveData> equippedItems;
    }
}