using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem : ILifecycle<Entity>
{
    private Entity _parent;
    public Entity Parent => _parent;
    private EntityStateMachine _stateMachine;
    public EntityStateMachine StateMachine => _stateMachine;
    private Dictionary<string, Skill> _skills = new Dictionary<string, Skill>();

    public SkillSystem(List<Skill> skills)
    {
        foreach (var skill in skills)
        {
            if (!_skills.ContainsKey(skill.SkillName))
            {
                _skills.Add(skill.SkillName, skill);
            }
            else
            {
                Debug.LogWarning($"Duplicate skill name detected: {skill.SkillName}. Skipping duplicate.");
            }
        }
    }

    public void Initialize(Entity parent)
    {
        _parent = parent;
        if(parent == null)
        {
            Debug.LogError("Parent entity is null.");
            return;
        }
        _stateMachine = parent.StateMachine;
        foreach (var skill in _skills.Values)
        {
            skill.Initialize(this);
        }
    }

    public void Dispose()
    {
        _parent = null;
    }

    public Skill GetSkill(string skillName)
    {
        if (_skills.TryGetValue(skillName, out Skill skill))
        {
            return skill;
        }
        Debug.LogError($"Skill '{skillName}' not found in SkillSystem.");
        return null;
    }
    public bool AddSkill(Skill newSkill)
    {
        if (newSkill == null)
        {
            Debug.LogError("Attempted to add a null skill.");
            return false;
        }

        if (_skills.ContainsKey(newSkill.SkillName))
        {
            Debug.LogWarning($"Skill '{newSkill.SkillName}' already exists in the SkillSystem. Use a different skill or update the existing one.");
            return false;
        }

        _skills.Add(newSkill.SkillName, newSkill);

        // If the SkillSystem has already been initialized, initialize the new skill immediately.
        if (_parent != null)
        {
            newSkill.Initialize(this);
        }

        return true;
    }


}
