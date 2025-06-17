using DataPersistence;
using DataPersistence.Data;
using EntitySystems.Level;
using EventSystem.Player;

namespace EntitySystems.PlayerSystems
{
    public class PlayerLevelSystem : LevelSystem, IDataPersistence
    {
        protected Entity.Entity _entity;
        
        public PlayerLevelSystem(int level = 1) : base(level) { }
        
        public void Initialize(Entity.Entity parent)
        {
            _entity = parent;
            ((IDataPersistence)this).AddDataPersistenceObject();
        }

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

        public void LoadData(GameData saveData)
        {
            if (saveData.PlayerLevelSystemSaveData != null)
            {
                SaveLoadHelper.LoadFromSaveData(this, saveData.PlayerLevelSystemSaveData);
            }
            InvokeInitialEvents();
        }

        public void SaveData(ref GameData data)
        {
            data.PlayerLevelSystemSaveData = SaveLoadHelper.CreateSaveData(this);
        }
    }
}