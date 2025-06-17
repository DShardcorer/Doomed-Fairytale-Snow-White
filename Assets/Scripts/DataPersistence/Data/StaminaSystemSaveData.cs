using System;
using System.Collections.Generic;

namespace DataPersistence.Data
{
    

    [Serializable]
    public class StaminaSystemSaveData
    {
        public float maxStamina;
        public float currentStamina;
        public List<RecoveryEffectSaveData> activeRecoveryEffects;
    }
}