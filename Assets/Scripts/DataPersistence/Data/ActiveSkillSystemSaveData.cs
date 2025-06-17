using System;
using System.Collections.Generic;

namespace DataPersistence.Data
{
    [Serializable]
    public class ActiveSkillSaveData
    {
        public string skillName;  // The unique identifier for the skill
    }

    [Serializable]
    public class ActiveSkillSystemSaveData
    {
        public List<ActiveSkillSaveData> activeSkills;
    }
}