using UnityEngine;

namespace EntitySystems.Skill
{
    public class PassiveSkillInfoSO : SkillInfoSO
    {
        public virtual PassiveSkill Create()
        {
            return SkillRegistry.SkillRegistry.CreatePassiveSkill(this);
        }
    }
}