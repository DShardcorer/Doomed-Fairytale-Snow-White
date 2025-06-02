using EventSystem.Entity;
using EventSystem.Player;
using GeneralManagers;

namespace EntitySystems.VitalStatSystems.Health_System
{
    public class HealthSystem : ILifecycle<Entity.Entity>
    {
        protected Entity.Entity _entity;
        public Entity.Entity Entity => _entity;
    
        protected float maxHealth;
        public float MaxHealth => maxHealth;

        protected float lastCurrentHealth;
        public float LastCurrentHealth => lastCurrentHealth;
        protected float currentHealth;
        public float CurrentHealth => currentHealth;

        public HealthSystem(float maxHealth)
        {
            this.maxHealth = maxHealth;
            currentHealth = maxHealth;
            lastCurrentHealth = maxHealth;
        }

        public virtual void Initialize(Entity.Entity parent)
        {
            _entity = parent;
        }

        public float GetHealthPercentage()
        {
            return currentHealth / maxHealth;
        }


        // Virtual method for invoking initial events; base implementation does nothing.
        public virtual void InvokeInitialEvents() { }

        public virtual void Dispose()
        {
            _entity = null;
        }

        // Apply damage and trigger the OnHealthChanged hook.
        public virtual void TakeDamage(float damage)
        {
            lastCurrentHealth = currentHealth;
            currentHealth -= damage;
            if (currentHealth < 0)
                currentHealth = 0;

            OnHealthChanged();

            if (currentHealth <= 0)
            {
                _entity.Die();
            }
        }

        // Virtual hook for derived classes to override when health changes.
        protected virtual void OnHealthChanged() {
            EntityVitalStatsEventSystem.InvokeHealthChanged(_entity, new HealthChangedEventArgs(lastCurrentHealth, currentHealth, maxHealth));
        }
    }
}
