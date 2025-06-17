using System;
using System.Collections.Generic;

namespace DataPersistence.Data
{
    [Serializable]
    public class EquippedSkillSaveData
    {
        public int slotIndex;
        public string skillName;  // Unique identifier for the skill
    }

    [Serializable]
    public class EquippedSkillSystemSaveData
    {
        public List<EquippedSkillSaveData> equippedSkills;
    }
}