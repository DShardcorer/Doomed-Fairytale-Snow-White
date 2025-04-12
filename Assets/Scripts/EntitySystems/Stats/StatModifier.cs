using System;

namespace EntitySystems.Stats
{
    public enum ModifierType { Flat, Percentage }

    [Serializable]
    public class StatModifier
    {
        public StatType StatType;
        public ModifierType ModifierType;
        public float Value;

        public StatModifier(StatType statType, ModifierType modifierType, float value)
        {
            StatType = statType;
            ModifierType = modifierType;
            Value = value;
        }

        public string GetStatString()
        {
            string modifierTypeString = ModifierType == ModifierType.Flat ? "" : "%";
            return $"{StatType}+{Value}{modifierTypeString}";
        }

    }
}