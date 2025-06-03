using System;
using System.Collections.Generic;
using EntitySystems.Skill.PassiveSkills;
using EntitySystems.Skill.PassiveSkills.Flirt;
using EntitySystems.Skill.PassiveSkills.PerceptiveEye;
using Helpers;
using UnityEngine;

namespace EntitySystems.Skill.SkillFactory
{
    public static class SkillFactory
    {
        private static readonly Dictionary<string, Func<ActiveSkillInfoSO, ActiveSkill>> _activeSkillCreators = new();

        private static readonly Dictionary<string, Func<PassiveSkillInfoSO, PassiveSkill>>
            _passiveSkillCreators = new();

        static SkillFactory()
        {
            RegisterDefaults();
        }

        private static void RegisterDefaults()
        {
            RegisterPassiveSkill(HelperSkillName.NaturalStrength,
                info => new NaturalStrengthPassiveSkill(info));
            RegisterPassiveSkill(HelperSkillName.BodyControl,
                info => new BodyControlPassiveSkill(info));
            RegisterPassiveSkill(HelperSkillName.Flirt,
                info => new FlirtPassiveSkill(info));
            RegisterPassiveSkill(HelperSkillName.PerceptiveEye,
                info => new PerceptiveEyePassiveSkill(info));
        }

        public static void RegisterActiveSkill(string skillName, Func<ActiveSkillInfoSO, ActiveSkill> creator)
        {
            if (_activeSkillCreators.ContainsKey(skillName))
            {
                Debug.LogWarning($"[SkillFactory] Overwriting creator for active skill '{skillName}'");
            }

            _activeSkillCreators[skillName] = creator;
        }

        public static void RegisterPassiveSkill(string skillName, Func<PassiveSkillInfoSO, PassiveSkill> creator)
        {
            if (_passiveSkillCreators.ContainsKey(skillName))
            {
                Debug.LogWarning($"[SkillFactory] Overwriting creator for passive skill '{skillName}'");
            }

            _passiveSkillCreators[skillName] = creator;
        }

        public static ActiveSkill CreateActiveSkill(ActiveSkillInfoSO info)
        {
            if (info == null)
            {
                Debug.LogError("[SkillFactory] ActiveSkillInfoSO is null.");
                return null;
            }

            if (_activeSkillCreators.TryGetValue(info.SkillName, out var creator))
            {
                return creator(info);
            }

            Debug.LogError($"[SkillFactory] No creator registered for active skill '{info.SkillName}'. " +
                           "Please ensure the skill is registered before trying to create it.");
            return null;
        }

        public static PassiveSkill CreatePassiveSkill(PassiveSkillInfoSO info)
        {
            if (info == null)
            {
                Debug.LogError("[SkillFactory] PassiveSkillInfoSO is null.");
                return null;
            }

            if (_passiveSkillCreators.TryGetValue(info.SkillName, out var creator))
            {
                return creator(info);
            }

            Debug.LogError($"[SkillFactory] No creator registered for passive skill '{info.SkillName}'. " +
                           "Please ensure the skill is registered before trying to create it.");
            return null;
        }

        //Overloads for getting skills by name
        public static ActiveSkill CreateActiveSkill(string skillName)
        {
            if (SkillRegistry.GetActiveSkillInfo(skillName) is ActiveSkillInfoSO info)
            {
                return CreateActiveSkill(info);
            }

            Debug.LogError($"[SkillFactory] No ActiveSkillInfoSO found for skill name '{skillName}'.");
            return null;
        }

        public static PassiveSkill CreatePassiveSkill(string skillName)
        {
            if (SkillRegistry.GetPassiveSkillInfo(skillName) is PassiveSkillInfoSO info)
            {
                return CreatePassiveSkill(info);
            }

            Debug.LogError($"[SkillFactory] No PassiveSkillInfoSO found for skill name '{skillName}'.");
            return null;
        }
    }
}