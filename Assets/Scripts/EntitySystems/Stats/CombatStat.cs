namespace EntitySystems.Stats
{
    public class CombatStat
    {
        public float BaseValue { get; set; }
        public float ModifiedValue { get; set; }

        public CombatStat(float baseValue = 10)
        {
            BaseValue = baseValue;
            ModifiedValue = baseValue;
        }
    }
}
