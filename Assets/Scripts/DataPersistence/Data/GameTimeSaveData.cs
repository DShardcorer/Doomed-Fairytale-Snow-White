using System;

namespace DataPersistence.Data
{
    [Serializable]
    public class GameTimeSaveData
    {
        public int currentDay;
        public float currentTimeOfDay;
        public bool isPaused;
        public bool isReversing;
    }
}