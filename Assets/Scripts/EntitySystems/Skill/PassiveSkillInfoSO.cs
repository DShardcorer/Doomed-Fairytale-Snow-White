using UnityEngine;

namespace EntitySystems.Skill
{
    public class PassiveSkillInfoSO : SkillInfoSO
    {
        public virtual PassiveSkill Create()
        {
            return SkillFactory.SkillFactory.CreatePassiveSkill(this);
        }
    }
}