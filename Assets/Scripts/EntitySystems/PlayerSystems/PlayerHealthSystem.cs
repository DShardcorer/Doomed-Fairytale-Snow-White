using EntitySystems.VitalStatSystems.Health_System;
using EventSystem.Player;

namespace EntitySystems.PlayerSystems
{
    public class PlayerHealthSystem : HealthSystem
    {
        public PlayerHealthSystem(float maxHealth) : base(maxHealth)
        {
        }

        public override void Initialize(Entity.Entity parent)
        {
            base.Initialize(parent);
        }

        public override void InvokeInitialEvents()
        {
            base.InvokeInitialEvents();
            PlayerVitalStatsEventSystem.InvokeHealthChanged(this, new HealthChangedEventArgs(lastCurrentHealth, currentHealth, maxHealth));
        }

        protected override void OnHealthChanged()
        {
            base.OnHealthChanged();
            PlayerVitalStatsEventSystem.InvokeHealthChanged(this, new HealthChangedEventArgs(lastCurrentHealth, currentHealth, maxHealth));
        }
    }
}
