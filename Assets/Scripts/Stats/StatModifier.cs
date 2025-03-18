
public enum ModifierType { Flat, Percentage }

public class StatModifier
{
    public StatType StatType { get; private set; }
    public ModifierType ModifierType { get; private set; }
    public float Value { get; private set; }

    public StatModifier(StatType statType, ModifierType modifierType, float value)
    {
        StatType = statType;
        ModifierType = modifierType;
        Value = value;
    }
}
