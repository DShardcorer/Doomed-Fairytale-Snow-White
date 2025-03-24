using UnityEngine;

public class PlayerLevelSystem : LevelSystem
{
    public PlayerLevelSystem(int level = 1) : base(level) { }

    public override void InvokeInitialEvents()
    {
        base.InvokeInitialEvents();
        PlayerLevelEventSystem.InvokeOnInitialExperienceSet(_experience, _experienceToNextLevel);
        PlayerLevelEventSystem.InvokeOnInitialLevelSet(_level);
    }

    public override void AddExperience(int amount)
    {
        base.AddExperience(amount);
        PlayerLevelEventSystem.InvokeOnExperienceChanged(_experience, _experienceToNextLevel);
    }

    protected override void OnLevelUp()
    {
        base.OnLevelUp();
        PlayerLevelEventSystem.InvokeOnLevelChanged(_level);
    }
}
