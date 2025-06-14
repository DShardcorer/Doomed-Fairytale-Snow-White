namespace DefaultNamespace.EntitySystems.VitalStatSystems
{
    public class RecoveryOverTimeEffect
    {
        public float TotalAmount { get; }
        public float RemainingAmount { get; set; }
        public float Duration { get; }
        public float RemainingTime { get; set; }
        public float RecoveryRate { get; }

        public RecoveryOverTimeEffect(float amount, float duration)
        {
            TotalAmount = amount;
            RemainingAmount = amount;
            Duration = duration;
            RemainingTime = duration;
            RecoveryRate = amount / duration;
        }
    }
}