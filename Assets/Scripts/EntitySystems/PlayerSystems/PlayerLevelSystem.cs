using UnityEngine;
using Events.Player;

public class PlayerLevelSystem : LevelSystem
{
    public PlayerLevelSystem(int level = 1) : base(level) { }

    public override void InvokeInitialEvents()
    {
        base.InvokeInitialEvents();
        PlayerLevelEventSystem.InvokeInitialExperienceSet(_experience, _experienceToNextLevel);
        PlayerLevelEventSystem.InvokeInitialLevelSet(_level);
    }

    public override void AddExperience(int amount)
    {
        base.AddExperience(amount);
        PlayerLevelEventSystem.InvokeExperienceChanged(_experience, _experienceToNextLevel);
    }

    protected override void OnLevelUp()
    {
        base.OnLevelUp();
        PlayerLevelEventSystem.InvokeLevelChanged(_level);
    }
}
