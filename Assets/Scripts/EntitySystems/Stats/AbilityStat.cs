public class AbilityStat
{
    public float BaseValue { get; set; }
    public float ModifiedValue { get; set; }

    public AbilityStat(float baseValue)
    {
        BaseValue = baseValue;
        ModifiedValue = baseValue;
    }

    public override string ToString()
    {
        return $"{ModifiedValue} (+{ModifiedValue - BaseValue})";
    }
}
