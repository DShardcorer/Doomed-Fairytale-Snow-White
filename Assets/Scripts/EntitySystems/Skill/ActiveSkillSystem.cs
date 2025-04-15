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
        protected Dictionary<string, ActiveSkill> activeSkills = new Dictionary<string, ActiveSkill>();

        public ActiveSkillSystem(List<ActiveSkill> skills)
        {
            foreach (var skill in skills)
            {
                if (!activeSkills.ContainsKey(skill.SkillName))
                {
                    activeSkills.Add(skill.SkillName, skill);
                }
                else
                {
                    Debug.LogWarning($"Duplicate skill name detected: {skill.SkillName}. Skipping duplicate.");
                }
            }
        }

        public virtual void Initialize(Entity.Entity parent)
        {
            _parent = parent;
            if(parent == null)
            {
                Debug.LogError("Parent entity is null.");
                return;
            }
            _stateMachine = parent.StateMachine;
            foreach (var skill in activeSkills.Values)
            {
                skill.Initialize(this);
            }
        }

        public virtual void InvokeInitialEvents()
        {
            //Should be overriden in derived classes
        }

        public void Dispose()
        {
            _parent = null;
        }

        public ActiveSkill GetSkill(string skillName)
        {
            if (activeSkills.TryGetValue(skillName, out ActiveSkill skill))
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

            if (activeSkills.ContainsKey(newActiveSkill.SkillName))
            {
                Debug.LogWarning($"Skill '{newActiveSkill.SkillName}' already exists in the SkillSystem. Use a different skill or update the existing one.");
                return false;
            }

            activeSkills.Add(newActiveSkill.SkillName, newActiveSkill);

            // If the SkillSystem has already been initialized, initialize the new skill immediately.
            if (_parent != null)
            {
                newActiveSkill.Initialize(this);
            }

            return true;
        }
        
        public virtual bool RemoveSkill(string skillName)
        {
            if (activeSkills.ContainsKey(skillName))
            {
                activeSkills.Remove(skillName);
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
