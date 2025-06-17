using System;
using System.Collections.Generic;

namespace DataPersistence.Data
{
    [Serializable]
    public class PassiveSkillSaveData
    {
        public string skillName;  // The unique identifier for the skill
    }

    [Serializable]
    public class PassiveSkillSystemSaveData
    {
        public List<PassiveSkillSaveData> passiveSkills;
    }
}