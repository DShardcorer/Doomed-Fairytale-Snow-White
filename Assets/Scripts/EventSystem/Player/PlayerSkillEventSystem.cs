using System;
using System.Collections.Generic;
using EntitySystems.Skill;
using Unity.VisualScripting;

namespace EventSystem.Player
{
    public static class PlayerSkillEventSystem
    {
        public class ActiveSkillListChangedEventArgs : EventArgs
        {
            public Dictionary<string, ActiveSkill> activeSkills;
            
            public ActiveSkillListChangedEventArgs(Dictionary<string, ActiveSkill> activeSkills)
            {
                this.activeSkills = activeSkills;
            }
        }
        public static Action<ActiveSkillListChangedEventArgs> OnActiveSkillListChanged;
        
        public static void InvokeActiveSkillListChanged(Dictionary<string, ActiveSkill> activeSkills)
        {
            OnActiveSkillListChanged?.Invoke(new ActiveSkillListChangedEventArgs(activeSkills));
        }
        
        public class PassiveSkillListChangedEventArgs : EventArgs
        {
            public Dictionary<string, PassiveSkill> passiveSkills;
            
            public PassiveSkillListChangedEventArgs(Dictionary<string, PassiveSkill> passiveSkills)
            {
                this.passiveSkills = passiveSkills;
            }
        }
        
        public static Action<PassiveSkillListChangedEventArgs> OnPassiveSkillListChanged;
        
        public static void InvokePassiveSkillListChanged(Dictionary<string, PassiveSkill> passiveSkills)
        {
            OnPassiveSkillListChanged?.Invoke(new PassiveSkillListChangedEventArgs(passiveSkills));
        }
    }
}