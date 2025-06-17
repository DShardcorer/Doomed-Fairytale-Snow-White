using System.Collections.Generic;

namespace DataPersistence.Data
{
    [System.Serializable]
    public class RecoveryEffectSaveData
    {
        public float totalAmount;
        public float remainingAmount;
        public float duration;
        public float remainingTime;
    }

    [System.Serializable]
    public class HealthSystemSaveData
    {
        public float maxHealth;
        public float currentHealth;
        public List<RecoveryEffectSaveData> activeRecoveryEffects;
    }
}