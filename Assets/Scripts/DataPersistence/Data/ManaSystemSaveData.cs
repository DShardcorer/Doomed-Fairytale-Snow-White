using System;
using System.Collections.Generic;

namespace DataPersistence.Data
{


    [Serializable]
    public class ManaSystemSaveData
    {
        public float maxMana;
        public float currentMana;
        public List<RecoveryEffectSaveData> activeRecoveryEffects;
    }
}