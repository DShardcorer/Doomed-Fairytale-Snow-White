using EntitySystems.Skill.SkillFactory;
using UnityEngine;

namespace EntitySystems.Skill
{
    public class PassiveSkillInfoSO : SkillInfoSO
    {
        public virtual PassiveSkill Create()
        {
            return SkillRegistry.CreatePassiveSkill(this);
        }
    }
}