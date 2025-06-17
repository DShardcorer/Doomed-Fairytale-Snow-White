using System;
using UnityEngine;

namespace DataPersistence.Data
{
    [Serializable]
    public class PlayerPositionSaveData
    {
        // Current scene and position
        public string currentSceneName;
        public float positionX;
        public float positionY;
        public float positionZ;
        
        // Overworld position (used when switching between normal/overworld views)
        public float overworldPositionX;
        public float overworldPositionY;
        public float overworldPositionZ;
    }
}