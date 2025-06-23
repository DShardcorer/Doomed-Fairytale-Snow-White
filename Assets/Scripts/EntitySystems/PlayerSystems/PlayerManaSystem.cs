using DataPersistence;
using DataPersistence.Data;
using EntitySystems.VitalStatSystems.Mana_System;
using EventBus.Player;

namespace EntitySystems.PlayerSystems
{
    public class PlayerManaSystem : ManaSystem, IDataPersistence
    {
        public PlayerManaSystem(float maxMana) : base(maxMana)
        {
        }

        public override void Initialize(EntityBase.Entity parent)
        {
            base.Initialize(parent);
            // Additional player-specific initialization can be done here if needed.
        }

        public override void InvokeInitialEvents()
        {
            PlayerVitalStatsEventSystem.InvokeManaChanged(this, new ManaChangedEventArgs(currentMana, maxMana));
        }

        protected override void OnManaChanged()
        {
            PlayerVitalStatsEventSystem.InvokeManaChanged(this, new ManaChangedEventArgs(currentMana, maxMana));
        }

        public void LoadData(GameData saveData)
        {
            if (saveData.PlayerManaSystemSaveData != null)
            {
                SaveLoadHelper.LoadFromSaveData(this, saveData.PlayerManaSystemSaveData);
            }
            InvokeInitialEvents();
        }

        public void SaveData(ref GameData data)
        {
            data.PlayerManaSystemSaveData = SaveLoadHelper.CreateSaveData(this);
        }
    }
}
