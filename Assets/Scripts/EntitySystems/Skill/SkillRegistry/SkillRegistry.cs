using System;
using System.Collections.Generic;
using EntitySystems.Skill.ActiveSkills.Player.Dash;
using EntitySystems.Skill.ActiveSkills.Player.Shoot;
using EntitySystems.Skill.PassiveSkills;
using EntitySystems.Skill.PassiveSkills.Flirt;
using EntitySystems.Skill.PassiveSkills.PerceptiveEye;
using Helpers;
using UnityEngine;

namespace EntitySystems.Skill.SkillRegistry
{
    public static class SkillRegistry
    {
        private static Dictionary<string, ActiveSkillInfoSO> _activeSkillInfoDictionary;
        private static Dictionary<string, PassiveSkillInfoSO> _passiveSkillInfoDictionary;

        private static readonly Dictionary<string, Func<ActiveSkillInfoSO, ActiveSkill>> _activeSkillCreators = new();
        private static readonly Dictionary<string, Func<PassiveSkillInfoSO, PassiveSkill>> _passiveSkillCreators = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Initialize()
        {
            LoadAllSkillInfo();
            RegisterDefaultCreators();
        }

        private static void LoadAllSkillInfo()
        {
            _activeSkillInfoDictionary = new Dictionary<string, ActiveSkillInfoSO>();

            ActiveSkillInfoSO[] activeSkillInfoSOs =
                UnityEngine.Resources.LoadAll<ActiveSkillInfoSO>(HelperResourcePath.ActiveSkillPath);
            foreach (ActiveSkillInfoSO activeSkillInfoSO in activeSkillInfoSOs)
                _activeSkillInfoDictionary.Add(activeSkillInfoSO.SkillName, activeSkillInfoSO);

            _passiveSkillInfoDictionary = new Dictionary<string, PassiveSkillInfoSO>();
            PassiveSkillInfoSO[] passiveSkillInfoSOs =
                UnityEngine.Resources.LoadAll<PassiveSkillInfoSO>(HelperResourcePath.PassiveSkillPath);
            foreach (PassiveSkillInfoSO passiveSkillInfoSO in passiveSkillInfoSOs)
                _passiveSkillInfoDictionary.Add(passiveSkillInfoSO.SkillName, passiveSkillInfoSO);
        }

        private static void RegisterDefaultCreators()
        {
            RegisterActiveSkill<DashActiveSkillInfoSO>(HelperSkillName.DashSkill,
                info => new DashActiveSkill(info));
            RegisterActiveSkill<ShootActiveSkillInfoSO>(HelperSkillName.ShootSkill,
                info => new ShootActiveSkill(info));
            RegisterPassiveSkill(HelperSkillName.NaturalStrength,
                info => new NaturalStrengthPassiveSkill(info));
            RegisterPassiveSkill(HelperSkillName.BodyControl,
                info => new BodyControlPassiveSkill(info));
            RegisterPassiveSkill(HelperSkillName.Flirt,
                info => new FlirtPassiveSkill(info));
            RegisterPassiveSkill(HelperSkillName.PerceptiveEye,
                info => new PerceptiveEyePassiveSkill(info));
        }

        public static void RegisterActiveSkill<T>(string skillName, Func<T, ActiveSkill> creator) where T : ActiveSkillInfoSO
        {
            if (_activeSkillCreators.ContainsKey(skillName))
            {
                Debug.LogWarning($"[SkillRegistry] Overwriting creator for active skill '{skillName}'");
            }

            _activeSkillCreators[skillName] = info => 
            {
                if (info is T typedInfo)
                {
                    return creator(typedInfo);
                }
                
                Debug.LogError($"[SkillRegistry] Expected {typeof(T).Name} for skill '{skillName}' but got {info.GetType().Name}");
                return null;
            };
        }

        public static void RegisterPassiveSkill<T>(string skillName, Func<T, PassiveSkill> creator) where T : PassiveSkillInfoSO
        {
            if (_passiveSkillCreators.ContainsKey(skillName))
            {
                Debug.LogWarning($"[SkillRegistry] Overwriting creator for passive skill '{skillName}'");
            }

            _passiveSkillCreators[skillName] = info => 
            {
                if (info is T typedInfo)
                {
                    return creator(typedInfo);
                }
                
                Debug.LogError($"[SkillRegistry] Expected {typeof(T).Name} for skill '{skillName}' but got {info.GetType().Name}");
                return null;
            };
        }

        // Keep the non-generic version for backward compatibility
        public static void RegisterPassiveSkill(string skillName, Func<PassiveSkillInfoSO, PassiveSkill> creator)
        {
            if (_passiveSkillCreators.ContainsKey(skillName))
            {
                Debug.LogWarning($"[SkillRegistry] Overwriting creator for passive skill '{skillName}'");
            }

            _passiveSkillCreators[skillName] = creator;
        }

        public static ActiveSkillInfoSO GetActiveSkillInfo(string skillName)
        {
            if (!_activeSkillInfoDictionary.TryGetValue(skillName, out var info))
            {
                Debug.LogError($"[SkillRegistry] No ActiveSkillInfoSO found for skill name '{skillName}'.");
                return null;
            }
            return info;
        }

        public static PassiveSkillInfoSO GetPassiveSkillInfo(string skillName)
        {
            if (!_passiveSkillInfoDictionary.TryGetValue(skillName, out var info))
            {
                Debug.LogError($"[SkillRegistry] No PassiveSkillInfoSO found for skill name '{skillName}'.");
                return null;
            }
            return info;
        }

        public static ActiveSkill CreateActiveSkill(string skillName)
        {
            var info = GetActiveSkillInfo(skillName);
            if (info == null)
                return null;

            if (_activeSkillCreators.TryGetValue(skillName, out var creator))
            {
                return creator(info);
            }

            Debug.LogError($"[SkillRegistry] No creator registered for active skill '{skillName}'. " +
                           "Please ensure the skill is registered before trying to create it.");
            return null;
        }

        public static PassiveSkill CreatePassiveSkill(string skillName)
        {
            var info = GetPassiveSkillInfo(skillName);
            if (info == null)
                return null;

            if (_passiveSkillCreators.TryGetValue(skillName, out var creator))
            {
                return creator(info);
            }

            Debug.LogError($"[SkillRegistry] No creator registered for passive skill '{skillName}'. " +
                           "Please ensure the skill is registered before trying to create it.");
            return null;
        }

        // Helper methods for creating skills from info objects directly
        public static ActiveSkill CreateActiveSkill(ActiveSkillInfoSO info)
        {
            if (info == null)
            {
                Debug.LogError("[SkillRegistry] ActiveSkillInfoSO is null.");
                return null;
            }

            if (_activeSkillCreators.TryGetValue(info.SkillName, out var creator))
            {
                return creator(info);
            }

            Debug.LogError($"[SkillRegistry] No creator registered for active skill '{info.SkillName}'. " +
                           "Please ensure the skill is registered before trying to create it.");
            return null;
        }

        public static PassiveSkill CreatePassiveSkill(PassiveSkillInfoSO info)
        {
            if (info == null)
            {
                Debug.LogError("[SkillRegistry] PassiveSkillInfoSO is null.");
                return null;
            }

            if (_passiveSkillCreators.TryGetValue(info.SkillName, out var creator))
            {
                return creator(info);
            }

            Debug.LogError($"[SkillRegistry] No creator registered for passive skill '{info.SkillName}'. " +
                           "Please ensure the skill is registered before trying to create it.");
            return null;
        }
    }
}