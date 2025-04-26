using System.Collections.Generic;
using Helpers;
using UnityEngine;

namespace EntitySystems.Skill.SkillFactory
{
    public static class SkillRegistry
    {
        private static Dictionary<string, ActiveSkillInfoSO> _activeSkillInfoDictionary;
        private static Dictionary<string, SkillInfoSO> _passiveSkillInfoDictionary;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Initialize()
        {
            LoadAllSkillInfo();
        }

        private static void LoadAllSkillInfo()
        {
            _activeSkillInfoDictionary = new Dictionary<string, ActiveSkillInfoSO>();

            ActiveSkillInfoSO[] activeSkillInfoSOs =
                UnityEngine.Resources.LoadAll<ActiveSkillInfoSO>(HelperResourcePath.ActiveSkillPath);
            foreach (ActiveSkillInfoSO activeSkillInfoSO in activeSkillInfoSOs)
                _activeSkillInfoDictionary.Add(activeSkillInfoSO.name, activeSkillInfoSO);

            _passiveSkillInfoDictionary = new Dictionary<string, SkillInfoSO>();
            SkillInfoSO[] passiveSkillInfoSOs =
                UnityEngine.Resources.LoadAll<SkillInfoSO>(HelperResourcePath.PassiveSkillPath);
            foreach (SkillInfoSO passiveSkillInfoSO in passiveSkillInfoSOs)
                _passiveSkillInfoDictionary.Add(passiveSkillInfoSO.name, passiveSkillInfoSO);
        }

        public static ActiveSkillInfoSO GetActiveSkillInfo(string skillName)
        {
            return _activeSkillInfoDictionary[skillName];
        }

        public static SkillInfoSO GetPassiveSkillInfo(string skillName)
        {
            return _passiveSkillInfoDictionary[skillName];
        }
    }
}