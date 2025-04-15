
using System.Collections.Generic;
using EntitySystems.Stats;
using GeneralManagers;
using UnityEngine;

namespace EntitySystems.Skill
{
    public class PassiveSkillSystem: ILifecycle<Entity.Entity>
    {
        private Entity.Entity _parent;
        public Entity.Entity Parent => _parent;
        private StatSystem _statSystem;
        protected Dictionary<string, PassiveSkill> passiveSkills;
        
        public PassiveSkillSystem(List<PassiveSkill> skills)
        {
            passiveSkills = new Dictionary<string, PassiveSkill>();
            foreach (var skill in skills)
            {
                if (!passiveSkills.ContainsKey(skill.SkillInfo.SkillName))
                {
                    passiveSkills.Add(skill.SkillInfo.SkillName, skill);
                }
                else
                {
                    Debug.LogWarning($"Duplicate skill name detected: {skill.SkillInfo.SkillName}. Skipping duplicate.");
                }
            }
        }
        public virtual void Initialize(Entity.Entity parent)
        {
            _parent = parent;
            _statSystem = parent.StatSystem;
            foreach ((string skillName, PassiveSkill skill) in passiveSkills)
            {
                skill.Initialize(this);
                skill.ApplyEffect();
            }
            _statSystem.RecalculateStats();
            
        }
        public virtual void Dispose()
        {
            _parent = null;
        }
        public virtual void InvokeInitialEvents()
        {
            
            //Should be overriden in derived classes
        }
        public virtual bool AddSkill(PassiveSkill newPassiveSkill)
        {
            if (!passiveSkills.ContainsKey(newPassiveSkill.SkillInfo.SkillName))
            {
                passiveSkills.Add(newPassiveSkill.SkillInfo.SkillName, newPassiveSkill);
                newPassiveSkill.Initialize(this);
                newPassiveSkill.ApplyEffect();
                _statSystem.RecalculateStats();
                return true;
            }
            else
            {
                Debug.LogWarning($"Passive skill '{newPassiveSkill.SkillInfo.SkillName}' already exists.");
                return false;
            }
        }

        public virtual bool RemoveSkill(PassiveSkill newPassiveSkill)
        {
            if (passiveSkills.ContainsKey(newPassiveSkill.SkillInfo.SkillName))
            {
                newPassiveSkill.UnapplyEffect();
                passiveSkills.Remove(newPassiveSkill.SkillInfo.SkillName);
                _statSystem.RecalculateStats();
                return true;
            }
            else
            {
                Debug.LogWarning($"Passive skill '{newPassiveSkill.SkillInfo.SkillName}' does not exist.");
                return false;
            }
        }


    }
}