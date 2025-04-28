using System.Collections.Generic;
using Helpers;
using UnityEngine;

namespace EntitySystems.Skill.SkillFactory
{
    public static class SkillRegistry
    {
        private static Dictionary<string, ActiveSkillInfoSO> _activeSkillInfoDictionary;
        private static Dictionary<string, PassiveSkillInfoSO> _passiveSkillInfoDictionary;

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
                _activeSkillInfoDictionary.Add(activeSkillInfoSO.SkillName, activeSkillInfoSO);

            _passiveSkillInfoDictionary = new Dictionary<string, PassiveSkillInfoSO>();
            PassiveSkillInfoSO[] passiveSkillInfoSOs =
                UnityEngine.Resources.LoadAll<PassiveSkillInfoSO>(HelperResourcePath.PassiveSkillPath);
            foreach (PassiveSkillInfoSO passiveSkillInfoSO in passiveSkillInfoSOs)
                _passiveSkillInfoDictionary.Add(passiveSkillInfoSO.SkillName, passiveSkillInfoSO);

        }

        public static ActiveSkillInfoSO GetActiveSkillInfo(string skillName)
        {
            return _activeSkillInfoDictionary[skillName];
        }

        public static SkillInfoSO GetPassiveSkillInfo(string skillName)
        {
            return _passiveSkillInfoDictionary[skillName];
        }

        public static ActiveSkill CreateActiveSkill(string skillName)
        {
            return _activeSkillInfoDictionary[skillName].Create();
        }
        public static PassiveSkill CreatePassiveSkill(string skillName)
        {
            return _passiveSkillInfoDictionary[skillName].Create();
        }
    }
}