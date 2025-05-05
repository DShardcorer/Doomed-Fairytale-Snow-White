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
        public class ActiveSkillGainedEventArgs : EventArgs
        {
            public ActiveSkill activeSkill;
            
            public ActiveSkillGainedEventArgs(ActiveSkill activeSkill)
            {
                this.activeSkill = activeSkill;
            }
        }
        public static Action<ActiveSkillGainedEventArgs> OnActiveSkillGained;
        
        public static void InvokeActiveSkillGained(ActiveSkill activeSkill)
        {
            OnActiveSkillGained?.Invoke(new ActiveSkillGainedEventArgs(activeSkill));
        }
        
        public class PassiveSkillGainedEventArgs : EventArgs
        {
            public PassiveSkill passiveSkill;
            
            public PassiveSkillGainedEventArgs(PassiveSkill passiveSkill)
            {
                this.passiveSkill = passiveSkill;
            }
        }
        public static Action<PassiveSkillGainedEventArgs> OnPassiveSkillGained;
        public static void InvokePassiveSkillGained(PassiveSkill passiveSkill)
        {
            OnPassiveSkillGained?.Invoke(new PassiveSkillGainedEventArgs(passiveSkill));
        }
        public class ActiveSkillRemovedEventArgs : EventArgs
        {
            public ActiveSkill activeSkill;
            
            public ActiveSkillRemovedEventArgs(ActiveSkill activeSkill)
            {
                this.activeSkill = activeSkill;
            }
        }
    }
}