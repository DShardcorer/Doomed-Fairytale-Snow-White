using System.Collections.Generic;
using EventSystem.Player;
using UnityEngine;

namespace EntitySystems.Skill
{
    public class PlayerActiveSkillSystem : ActiveSkillSystem
    {
        public PlayerActiveSkillSystem(List<ActiveSkill> skills) : base(skills)
        {
        }

        public override void Initialize(Entity.Entity parent)
        {
            base.Initialize(parent);
            InvokeInitialEvents();
        }

        public override void InvokeInitialEvents()
        {
            base.InvokeInitialEvents();
            PlayerSkillEventSystem.InvokeActiveSkillListChanged(activeSkills);
        }

        public override bool AddSkill(ActiveSkill skill)
        {
            if (base.AddSkill(skill))
            {
                PlayerSkillEventSystem.InvokeActiveSkillListChanged(activeSkills);
                return true;
            }

            return false;
        }

        public override bool RemoveSkill(string skillName)
        {
            if (base.RemoveSkill(skillName))
            {
                PlayerSkillEventSystem.InvokeActiveSkillListChanged(activeSkills);
                return true;
            }

            return false;
        }
    }
}