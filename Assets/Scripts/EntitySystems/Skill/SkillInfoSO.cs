
using UnityEngine;

namespace EntitySystems.Skill
{
    
    public class SkillInfoSO:ScriptableObject
    {
        public string SkillName;
        public bool IsMindBound;
        [TextArea(5,10)]
        public string SkillDescription;
        public Sprite SkillIcon;
        
        
    }
}