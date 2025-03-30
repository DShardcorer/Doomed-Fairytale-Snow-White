using System;
using UnityEngine;

namespace Events.Player
{
    public class OnExperienceChangedEventArgs : EventArgs
    {
        public int Experience { get; set; }
        public int ExperienceToNextLevel { get; set; }
    }

    public class OnLevelChangedEventArgs : EventArgs
    {
        public int Level { get; set; }
    }

    public static class PlayerLevelEventSystem
    {
        public static event EventHandler<OnExperienceChangedEventArgs> OnInitialExperienceSet;
        public static event EventHandler<OnLevelChangedEventArgs> OnInitialLevelSet;
        public static event EventHandler<OnExperienceChangedEventArgs> OnExperienceChanged;
        public static event EventHandler<OnLevelChangedEventArgs> OnLevelChanged;

        public static void InvokeInitialExperienceSet(int experience, int experienceToNextLevel)
        {
            OnInitialExperienceSet?.Invoke(null, new OnExperienceChangedEventArgs { Experience = experience, ExperienceToNextLevel = experienceToNextLevel });
        }

        public static void InvokeInitialLevelSet(int level)
        {
            OnInitialLevelSet?.Invoke(null, new OnLevelChangedEventArgs { Level = level });
        }

        public static void InvokeExperienceChanged(int experience, int experienceToNextLevel)
        {
            OnExperienceChanged?.Invoke(null, new OnExperienceChangedEventArgs { Experience = experience, ExperienceToNextLevel = experienceToNextLevel });
        }

        public static void InvokeLevelChanged(int level)
        {
            OnLevelChanged?.Invoke(null, new OnLevelChangedEventArgs { Level = level });
        }
    }
}
