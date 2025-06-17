using System.Collections.Generic;
using Entity;
using GeneralManagers;
using UnityEngine;

namespace EntitySystems.Skill
{
    public class ActiveSkillSystem : ILifecycle<Entity.Entity>
    {
        private Entity.Entity _parent;
        public Entity.Entity Parent => _parent;

        private EntityStateMachine _stateMachine;
        public EntityStateMachine StateMachine => _stateMachine;

        protected Dictionary<string, ActiveSkill> activeSkillsDict = new Dictionary<string, ActiveSkill>();
        protected List<ActiveSkill> activeSkills = new List<ActiveSkill>();
        public List<ActiveSkill> ActiveSkills => activeSkills;
        

        public ActiveSkillSystem(List<ActiveSkill> skills)
        {
            foreach (ActiveSkill skill in skills)
            {
                if (skill == null)
                {
                    Debug.LogError("Attempted to add a null skill.");
                    continue;
                }
                if (!activeSkillsDict.ContainsKey(skill.activeSkillInfo.SkillName))
                {
                    activeSkillsDict.Add(skill.activeSkillInfo.SkillName, skill);
                    activeSkills.Add(skill);
                }
                else
                {
                    Debug.LogWarning($"Duplicate skill name detected: {skill.activeSkillInfo.SkillName}. Skipping duplicate.");
                }
            }
        }

        public virtual void Initialize(Entity.Entity parent)
        {
            _parent = parent;

            if (parent == null)
            {
                Debug.LogError("Parent entity is null.");
                return;
            }

            _stateMachine = parent.StateMachine;

            foreach (var skill in activeSkills)
            {
                skill.Initialize(this);
            }
        }

        public virtual void InvokeInitialEvents()
        {
            // Should be overridden in derived classes
        }

        public void Dispose()
        {
            _parent = null;
            _stateMachine = null;
            //Call Dispose on each skill
            foreach (var skill in activeSkills)
            {
                skill.Dispose();
            }
            activeSkillsDict.Clear();
            activeSkills.Clear();
            Debug.LogWarning("ActiveSkillSystem disposed.");
        }

        public ActiveSkill GetSkill(string skillName)
        {
            if (activeSkillsDict.TryGetValue(skillName, out ActiveSkill skill))
            {
                return skill;
            }

            Debug.LogError($"Skill '{skillName}' not found in SkillSystem.");
            return null;
        }

        public virtual bool AddSkill(ActiveSkill newActiveSkill)
        {
            if (newActiveSkill == null)
            {
                Debug.LogError("Attempted to add a null skill.");
                return false;
            }

            if (activeSkillsDict.ContainsKey(newActiveSkill.activeSkillInfo.SkillName))
            {
                Debug.LogWarning($"Skill '{newActiveSkill.activeSkillInfo.SkillName}' already exists in the SkillSystem. Use a different skill or update the existing one.");
                return false;
            }

            activeSkillsDict.Add(newActiveSkill.activeSkillInfo.SkillName, newActiveSkill);
            activeSkills.Add(newActiveSkill);

            if (_parent != null)
            {
                newActiveSkill.Initialize(this);
            }

            return true;
        }

        public virtual bool RemoveSkill(string skillName)
        {
            if (activeSkillsDict.TryGetValue(skillName, out ActiveSkill skill))
            {
                activeSkillsDict.Remove(skillName);
                activeSkills.Remove(skill);
                return true;
            }
            else
            {
                Debug.LogWarning($"Skill '{skillName}' not found in SkillSystem.");
                return false;
            }
        }
    }
}
