using System;
using UnityEngine;
public class OnExperienceChangedEventArgs : EventArgs
{
    public int Experience { get; set; }
    public int ExperienceToNextLevel { get; set; }
}

public class OnLevelChangedEventArgs : EventArgs
{
    public int level { get; set; }
}


public static class PlayerLevelEventSystem
{
    public static event EventHandler<OnExperienceChangedEventArgs> OnInitialExperienceSet;
    public static event EventHandler<OnLevelChangedEventArgs> OnInitialLevelSet;
    public static event EventHandler<OnExperienceChangedEventArgs> OnExperienceChanged;
    public static event EventHandler<OnLevelChangedEventArgs> OnLevelChanged;

    public static void InvokeOnExperienceChanged(int experience, int experienceToNextLevel)
    {
        OnExperienceChanged?.Invoke(null, new OnExperienceChangedEventArgs { Experience = experience, ExperienceToNextLevel = experienceToNextLevel });
    }

    public static void InvokeOnLevelChanged(int level)
    {
        OnLevelChanged?.Invoke(null, new OnLevelChangedEventArgs { level = level });
    }

    public static void InvokeOnInitialExperienceSet(int experience, int experienceToNextLevel)
    {
        OnInitialExperienceSet?.Invoke(null, new OnExperienceChangedEventArgs { Experience = experience, ExperienceToNextLevel = experienceToNextLevel });
    }

    public static void InvokeOnInitialLevelSet(int level)
    {
        OnInitialLevelSet?.Invoke(null, new OnLevelChangedEventArgs { level = level });
    }


}
