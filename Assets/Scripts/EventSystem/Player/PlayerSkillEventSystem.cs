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
            public List<ActiveSkill> activeSkills;
            
            public ActiveSkillListChangedEventArgs(List<ActiveSkill> activeSkills)
            {
                this.activeSkills = activeSkills;
            }
        }
        public static Action<ActiveSkillListChangedEventArgs> OnActiveSkillListChanged;
        
        public static void InvokeActiveSkillListChanged(List<ActiveSkill> activeSkills)
        {
            OnActiveSkillListChanged?.Invoke(new ActiveSkillListChangedEventArgs(activeSkills));
        }
        
        public class PassiveSkillListChangedEventArgs : EventArgs
        {
            public List<PassiveSkill> passiveSkills;
            
            public PassiveSkillListChangedEventArgs(List<PassiveSkill> passiveSkills)
            {
                this.passiveSkills = passiveSkills;
            }
        }
        
        public static Action<PassiveSkillListChangedEventArgs> OnPassiveSkillListChanged;
        
        public static void InvokePassiveSkillListChanged(List<PassiveSkill> passiveSkills)
        {
            OnPassiveSkillListChanged?.Invoke(new PassiveSkillListChangedEventArgs(passiveSkills));
        }
    }
}