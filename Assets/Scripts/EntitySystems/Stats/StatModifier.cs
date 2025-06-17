using System;
using UnityEngine.Serialization;

namespace EntitySystems.Stats
{
    public enum StatModifierType { Flat, Percentage }

    [Serializable]
    public class StatModifier
    {
        public StatType StatType;
        [FormerlySerializedAs("ModifierType")] public StatModifierType statModifierType;
        public float Value;

        public StatModifier(StatType statType, StatModifierType statModifierType, float value)
        {
            StatType = statType;
            this.statModifierType = statModifierType;
            Value = value;
        }

        public string GetStatString()
        {
            string modifierTypeString = statModifierType == StatModifierType.Flat ? "" : "%";
            return $"{StatType}+{Value}{modifierTypeString}";
        }

    }
}