using System.Collections.Generic;
using EntitySystems.Stats;
using GeneralManagers;
using UnityEngine;

namespace EntitySystems.Skill
{
    public class PassiveSkillSystem : ILifecycle<EntityBase.Entity>
    {
        private EntityBase.Entity _parent;
        public EntityBase.Entity Parent => _parent;
        private StatSystem _statSystem;
        protected Dictionary<string, PassiveSkill> passiveSkillsDict;
        protected List<PassiveSkill> passiveSkills;
        public IReadOnlyList<PassiveSkill> PassiveSkills => passiveSkills.AsReadOnly();

        public virtual void Initialize(EntityBase.Entity parent)
        {
            _parent = parent;
            _statSystem = parent.StatSystem;

            foreach (var skill in passiveSkills)
            {
                skill.Initialize(this);
                skill.ApplyEffect();
            }

            _statSystem.RecalculateStats();
        }

        public virtual void Dispose()
        {
            _parent = null;
            _statSystem = null;
            passiveSkillsDict.Clear();
            passiveSkills.Clear();
        }
        public PassiveSkillSystem(List<PassiveSkill> skills)
        {
            passiveSkillsDict = new Dictionary<string, PassiveSkill>();
            passiveSkills = new List<PassiveSkill>();

            foreach (var skill in skills)
            {
                if (!passiveSkillsDict.ContainsKey(skill.SkillInfo.SkillName))
                {
                    passiveSkillsDict.Add(skill.SkillInfo.SkillName, skill);
                    passiveSkills.Add(skill);
                }
                else
                {
                    Debug.LogWarning($"Duplicate skill name detected: {skill.SkillInfo.SkillName}. Skipping duplicate.");
                }
            }
        }



        public virtual void InvokeInitialEvents()
        {
            // Should be overridden in derived classes
        }

        public virtual bool AddSkill(PassiveSkill newPassiveSkill)
        {
            if (!passiveSkillsDict.ContainsKey(newPassiveSkill.SkillInfo.SkillName))
            {
                passiveSkillsDict.Add(newPassiveSkill.SkillInfo.SkillName, newPassiveSkill);
                passiveSkills.Add(newPassiveSkill);

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

        public virtual bool RemoveSkill(PassiveSkill passiveSkill)
        {
            if (passiveSkillsDict.ContainsKey(passiveSkill.SkillInfo.SkillName))
            {
                passiveSkill.UnapplyEffect();
                passiveSkillsDict.Remove(passiveSkill.SkillInfo.SkillName);
                passiveSkills.Remove(passiveSkill);

                _statSystem.RecalculateStats();
                return true;
            }
            else
            {
                Debug.LogWarning($"Passive skill '{passiveSkill.SkillInfo.SkillName}' does not exist.");
                return false;
            }
        }
    }
}
