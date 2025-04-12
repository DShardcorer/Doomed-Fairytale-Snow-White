using EntitySystems.VitalStatSystems.Mana_System;
using EventSystem.Player;

namespace EntitySystems.PlayerSystems
{
    public class PlayerManaSystem : ManaSystem
    {
        public PlayerManaSystem(float maxMana) : base(maxMana)
        {
        }

        public override void Initialize(Entity.Entity parent)
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
    }
}
