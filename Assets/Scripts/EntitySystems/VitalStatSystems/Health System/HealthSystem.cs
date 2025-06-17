using System.Collections.Generic;
using DataPersistence.Data;
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
        protected float recoveryUpdateTimer = 0f;
        

        // Collection of active recovery effects - using Queue for FIFO behavior
        protected List<RecoveryOverTimeEffect> activeRecoveryEffects = new List<RecoveryOverTimeEffect>(4); // Pre-allocate for common case

        public HealthSystem(float maxHealth)
        {
            this.maxHealth = maxHealth;
            currentHealth = maxHealth;
            lastCurrentHealth = maxHealth;
        }

        #region APIS
        public List<RecoveryOverTimeEffect> GetActiveRecoveryEffects()
        {
            return activeRecoveryEffects;
        }
        public void SetMaxHealth(float value)
        {
            if (value <= 0) return;
            maxHealth = value;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
                OnHealthChanged();
            }
        }
        public void SetCurrentHealth(float value)
        {
            if (value < 0) return;
            lastCurrentHealth = currentHealth;
            currentHealth = value;
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;

            OnHealthChanged();
        }
        public void ClearRecoveryEffects()
        {
            activeRecoveryEffects.Clear();
            recoveryUpdateTimer = 0f;
        }
        public void AddRecoveryEffect(RecoveryOverTimeEffect effect)
        {
            if (effect == null || effect.RecoveryRate <= 0 || effect.Duration <= 0) return;
            activeRecoveryEffects.Add(effect);
        }
        public void AddRecoveryEffect(RecoveryEffectSaveData effectData)
        {
            // Create a new RecoveryOverTimeEffect from the save data
            var effect = new RecoveryOverTimeEffect(effectData.totalAmount, effectData.duration)
            {
                RemainingAmount = effectData.remainingAmount,
                RemainingTime = effectData.remainingTime
            };
            activeRecoveryEffects.Add(effect);
            
        }
        

        #endregion
        

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
       
        // Modified ProcessRecoveryEffects to update once per second
        protected virtual void ProcessRecoveryEffects(float deltaTime)
        {
            if (activeRecoveryEffects.Count == 0 || currentHealth >= maxHealth) return;

            // Update effect timers every frame for accurate duration tracking
            for (int i = activeRecoveryEffects.Count - 1; i >= 0; i--)
            {
                var effect = activeRecoveryEffects[i];
                effect.RemainingTime -= deltaTime;

                // Remove expired effects
                if (effect.RemainingTime <= 0)
                {
                    // Swap and pop for efficient removal
                    int lastIndex = activeRecoveryEffects.Count - 1;
                    if (i < lastIndex)
                        activeRecoveryEffects[i] = activeRecoveryEffects[lastIndex];
                    activeRecoveryEffects.RemoveAt(lastIndex);
                }
            }

            // Increment the timer
            recoveryUpdateTimer += deltaTime;

            // Only apply healing once per second
            if (recoveryUpdateTimer >= 1.0f)
            {
                float totalHealing = 0f;

                // Calculate and accumulate healing for the 1-second interval
                for (int i = activeRecoveryEffects.Count - 1; i >= 0; i--)
                {
                    var effect = activeRecoveryEffects[i];

                    // Calculate healing for this interval
                    float intervalHealing = effect.RecoveryRate * recoveryUpdateTimer;

                    // Don't heal more than what's remaining
                    if (intervalHealing > effect.RemainingAmount)
                        intervalHealing = effect.RemainingAmount;

                    totalHealing += intervalHealing;
                    effect.RemainingAmount -= intervalHealing;

                    // Remove effects that have depleted their healing amount
                    if (effect.RemainingAmount <= 0)
                    {
                        // Swap and pop for efficient removal
                        int lastIndex = activeRecoveryEffects.Count - 1;
                        if (i < lastIndex)
                            activeRecoveryEffects[i] = activeRecoveryEffects[lastIndex];
                        activeRecoveryEffects.RemoveAt(lastIndex);
                    }
                }

                // Apply the accumulated healing
                if (totalHealing > 0)
                {
                    lastCurrentHealth = currentHealth;
                    currentHealth += totalHealing;
                    if (currentHealth > maxHealth)
                        currentHealth = maxHealth;

                    OnHealthChanged();
                }

                // Reset timer
                recoveryUpdateTimer = 0f;
            }
        }

        // Virtual hook for derived classes to override when health changes.
        protected virtual void OnHealthChanged()
        {
            EntityVitalStatsEventSystem.InvokeHealthChanged(_entity, new HealthChangedEventArgs(lastCurrentHealth, currentHealth, maxHealth));
        }
    }
}