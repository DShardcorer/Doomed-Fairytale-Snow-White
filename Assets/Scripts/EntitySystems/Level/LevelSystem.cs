using System;
using UnityEngine;

public class LevelSystem
{
    private int _level;
    public int Level => _level;
    private int _experience;
    public int Experience => _experience;
    private int _experienceToNextLevel;
    public int ExperienceToNextLevel => _experienceToNextLevel;

    public event EventHandler OnExperienceChanged;
    public event EventHandler OnLevelChanged;

    public LevelSystem(int level= 1)
    {
        _level = level;
        _experience = 0;
        _experienceToNextLevel = CalculateExperienceToNextLevel(_level);
    }
    public void AddExperience(int amount)
    {
        _experience += amount;
        if (_experience >= _experienceToNextLevel)
        {
            _level++;
            _experience -= _experienceToNextLevel;
            _experienceToNextLevel = CalculateExperienceToNextLevel(_level);
            OnLevelChanged?.Invoke(this, EventArgs.Empty);
        }
        OnExperienceChanged?.Invoke(this, EventArgs.Empty);
    }

    public int CalculateExperienceToNextLevel(int level)
    {
        return level * 100;
    }

    


}
