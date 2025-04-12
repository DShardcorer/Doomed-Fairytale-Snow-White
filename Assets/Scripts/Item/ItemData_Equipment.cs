using System.Collections.Generic;
using EntitySystems.Equipment;
using EntitySystems.Stats;
using UnityEngine;

namespace Item
{
    [CreateAssetMenu(fileName = "New Equipment Item", menuName = "Equipment ItemData")]
    public class ItemData_Equipment : ItemData
    {

        public EquipmentSlotType equipmentSlotType;
        public EquipmentItemType equipmentItemType;
        public List<StatModifier> StatModifiers;

        public string GetStatsString()
        {
            string statsString = "";
            foreach (StatModifier statModifier in StatModifiers)
            {
                statsString += statModifier.GetStatString() + "\n";
            }
            return statsString;
        }
    
    }
}
