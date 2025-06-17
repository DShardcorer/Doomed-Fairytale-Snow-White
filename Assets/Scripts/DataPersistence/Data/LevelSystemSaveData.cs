using System;

namespace DataPersistence.Data
{
    [Serializable]
    public class LevelSystemSaveData
    {
        public int level;
        public int experience;
        public int experienceToNextLevel;
    }
}