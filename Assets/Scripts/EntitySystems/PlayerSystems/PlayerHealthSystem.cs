using DataPersistence;
using DataPersistence.Data;
using EntitySystems.VitalStatSystems.Health_System;
using EventSystem.Player;

namespace EntitySystems.PlayerSystems
{
    public class PlayerHealthSystem : HealthSystem, IDataPersistence
    {
        public PlayerHealthSystem(float maxHealth) : base(maxHealth)
        {
        }

        public override void Initialize(Entity.Entity parent)
        {
            base.Initialize(parent);
            ((IDataPersistence)this).AddDataPersistenceObject();
        }

        public override void InvokeInitialEvents()
        {
            base.InvokeInitialEvents();
            PlayerVitalStatsEventSystem.InvokeHealthChanged(this, new HealthChangedEventArgs(lastCurrentHealth, currentHealth, maxHealth));
        }

        protected override void InvokeHealthChanged()
        {
            base.InvokeHealthChanged();
            PlayerVitalStatsEventSystem.InvokeHealthChanged(this, new HealthChangedEventArgs(lastCurrentHealth, currentHealth, maxHealth));
        }
        
        public void LoadData(GameData saveData)
        {
            SaveLoadHelper.LoadFromSaveData(this, saveData.PlayerHealthSystemSaveData);
            InvokeInitialEvents();
        }

        public void SaveData(ref GameData data)
        {
            data.PlayerHealthSystemSaveData = SaveLoadHelper.CreateSaveData(this);
        }
    }
}