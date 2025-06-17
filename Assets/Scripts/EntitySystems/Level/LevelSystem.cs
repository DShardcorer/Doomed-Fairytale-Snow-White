namespace EntitySystems.Level
{
    public class LevelSystem
    {
        protected int _level;
        public int Level => _level;
    
        protected int _experience;
        public int Experience => _experience;
    
        protected int _experienceToNextLevel;
        public int ExperienceToNextLevel => _experienceToNextLevel;

        public LevelSystem(int level = 1)
        {
            _level = level;
            _experience = 0;
            _experienceToNextLevel = CalculateExperienceToNextLevel(_level);
        }

        #region APIS RESERVED FOR SAVE/LOAD

        public virtual void SetLevel(int level)
        {
            _level = level;
        }

        public virtual void SetExperience(int experience) 
        {
            _experience = experience;
        }

        public virtual void SetExperienceToNextLevel(int experienceToNextLevel)
        {
            _experienceToNextLevel = experienceToNextLevel;
        }

        #endregion
        

        public virtual void AddExperience(int amount)
        {
            _experience += amount;
            while (_experience >= _experienceToNextLevel)
            {
                _experience -= _experienceToNextLevel;
                _level++;
                _experienceToNextLevel = CalculateExperienceToNextLevel(_level);
                OnLevelUp();
            }
        }
        public virtual void InvokeInitialEvents()
        {
            // Base behavior for initializing events (if needed)
        }

        protected virtual void OnLevelUp()
        {
            // Base behavior for leveling up (if needed)
        }

        public virtual int CalculateExperienceToNextLevel(int level)
        {
            return level * 100; // Default formula for experience scaling
        }
    }
}
