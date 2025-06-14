using System.Collections.Generic;
using DefaultNamespace.EntitySystems.VitalStatSystems;
using EventSystem.Entity;
using EventSystem.Player;
using GeneralManagers;

namespace EntitySystems.VitalStatSystems.Health_System
{
    public class HealthSystem : ILifecycle<Entity.Entity>, IUpdatable
    {
        protected Entity.Entity _entity;
        public Entity.Entity Entity => _entity;

        protected float maxHealth;
        public float MaxHealth => maxHealth;

        protected float lastCurrentHealth;
        public float LastCurrentHealth => lastCurrentHealth;
        protected float currentHealth;
        public float CurrentHealth => currentHealth;
        

        // Collection of active recovery effects - using Queue for FIFO behavior
        protected List<RecoveryOverTimeEffect> activeRecoveryEffects = new List<RecoveryOverTimeEffect>(4); // Pre-allocate for common case

        public HealthSystem(float maxHealth)
        {
            this.maxHealth = maxHealth;
            currentHealth = maxHealth;
            lastCurrentHealth = maxHealth;
        }

        public virtual void Initialize(Entity.Entity parent)
        {
            _entity = parent;
            GameManager.Instance.UpdateManager.AddUpdatable(this);
        }
        
        public virtual void Dispose()
        {
            _entity = null;
            GameManager.Instance.UpdateManager.RemoveUpdatable(this);
            activeRecoveryEffects.Clear();
        }
        
        public void UpdateLogic()
        {
            ProcessRecoveryEffects(UnityEngine.Time.deltaTime);
        }

        public float GetHealthPercentage()
        {
            return currentHealth / maxHealth;
        }

        // Virtual method for invoking initial events; base implementation does nothing.
        public virtual void InvokeInitialEvents() { }

        // Apply damage and trigger the OnHealthChanged hook.
        public virtual void TakeDamage(float damage)
        {
            if (damage <= 0) return;
            
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

        // Recover health immediately
        public virtual void Recover(float amount)
        {
            if (amount <= 0 || currentHealth >= maxHealth) return;

            lastCurrentHealth = currentHealth;
            currentHealth += amount;
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;

            OnHealthChanged();
        }

        // Add a recovery over time effect to the stack
        public virtual void RecoverOvertime(float amount, float duration)
        {
            if (amount <= 0 || duration <= 0 || currentHealth >= maxHealth) return;

            activeRecoveryEffects.Add(new RecoveryOverTimeEffect(amount, duration));
        }

        // Process all active recovery effects
        protected virtual void ProcessRecoveryEffects(float deltaTime)
        {
            if (activeRecoveryEffects.Count == 0 || currentHealth >= maxHealth) return;

            float totalHealingThisFrame = 0;

            for (int i = activeRecoveryEffects.Count - 1; i >= 0; i--)
            {
                var effect = activeRecoveryEffects[i];

                // Calculate healing amount for this frame
                float healingThisFrame = effect.RecoveryRate * deltaTime;

                // Don't heal more than what's remaining
                if (healingThisFrame > effect.RemainingAmount)
                    healingThisFrame = effect.RemainingAmount;

                totalHealingThisFrame += healingThisFrame;
                effect.RemainingAmount -= healingThisFrame;
                effect.RemainingTime -= deltaTime;

                // Remove completed effects
                if (effect.RemainingTime <= 0 || effect.RemainingAmount <= 0)
                {
                    // Swap and pop for efficient removal
                    int lastIndex = activeRecoveryEffects.Count - 1;
                    if (i < lastIndex)
                        activeRecoveryEffects[i] = activeRecoveryEffects[lastIndex];
                    activeRecoveryEffects.RemoveAt(lastIndex);
                }
            }

            // Apply the total healing only once per frame
            if (totalHealingThisFrame > 0)
            {
                // Skip OnHealthChanged event to apply direct healing
                lastCurrentHealth = currentHealth;
                currentHealth += totalHealingThisFrame;
                if (currentHealth > maxHealth)
                    currentHealth = maxHealth;
                
                OnHealthChanged();
            }
        }

        // Virtual hook for derived classes to override when health changes.
        protected virtual void OnHealthChanged()
        {
            EntityVitalStatsEventSystem.InvokeHealthChanged(_entity, new HealthChangedEventArgs(lastCurrentHealth, currentHealth, maxHealth));
        }
    }
}