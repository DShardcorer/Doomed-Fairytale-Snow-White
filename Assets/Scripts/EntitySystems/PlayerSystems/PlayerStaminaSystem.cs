using DataPersistence;
using DataPersistence.Data;
using EntitySystems.VitalStatSystems.Stamina_System;
using EventSystem.Player;
namespace EntitySystems.PlayerSystems
{
    public class PlayerStaminaSystem : StaminaSystem, IDataPersistence
    {
        public PlayerStaminaSystem(int maxStamina) : base(maxStamina)
        {
        }
    
        public override void Initialize(Entity.Entity parent)
        {
            base.Initialize(parent);
            // Additional player-specific initialization can be performed here if needed.
            // Register with the data persistence manager
            ((IDataPersistence)this).AddDataPersistenceObject();
        }


    
        public override void InvokeInitialEvents()
        {
            PlayerVitalStatsEventSystem.InvokeStaminaChanged(this, 
                new StaminaChangedEventArgs(currentStamina, maxStamina));
        }
    
        // Override the hook to invoke player-specific stamina changed events.
        protected override void OnStaminaChanged()
        {
            PlayerVitalStatsEventSystem.InvokeStaminaChanged(this, 
                new StaminaChangedEventArgs(currentStamina, maxStamina));
        }

        public void LoadData(GameData saveData)
        {
            this.currentStamina = saveData.stamina;
        }

        public void SaveData(ref GameData data)
        {
            data.stamina = currentStamina;
        }
    }
}
