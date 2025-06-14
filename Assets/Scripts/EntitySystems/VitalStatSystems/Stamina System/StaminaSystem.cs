using System.Collections.Generic;
using DefaultNamespace.EntitySystems.VitalStatSystems;
using GeneralManagers;
using UnityEngine;

namespace EntitySystems.VitalStatSystems.Stamina_System
{
    public class StaminaSystem : ILifecycle<Entity.Entity>, IUpdatable
    {
        protected Entity.Entity entity;
        public Entity.Entity Entity => entity;
    
        protected float maxStamina;
        public float MaxStamina => maxStamina;

        protected float currentStamina;
        public float CurrentStamina => currentStamina;


        // Collection of active recovery effects
        protected List<RecoveryOverTimeEffect> activeRecoveryEffects = new List<RecoveryOverTimeEffect>(4);

        public StaminaSystem(int maxStamina)
        {
            this.maxStamina = maxStamina;
            currentStamina = maxStamina;
        }
    
        public virtual void Initialize(Entity.Entity parent)
        {
            entity = parent;
            GameManager.Instance.UpdateManager.AddUpdatable(this);
        }
    
        public virtual void Dispose()
        {
            entity = null;
            GameManager.Instance.UpdateManager.RemoveUpdatable(this);
            activeRecoveryEffects.Clear();
        }
    
        public virtual void UpdateLogic()
        {
            // Process recovery effects first
            ProcessRecoveryEffects(Time.deltaTime);
            
            // Then apply natural stamina regeneration (5% per second)
            RestoreStamina(maxStamina / 20 * Time.deltaTime);
        }
    
        protected virtual void OnStaminaChanged() { }
    
        public virtual void InvokeInitialEvents() { }
    
        public virtual bool TryUseStamina(float stamina)
        {
            if (currentStamina >= stamina)
            {
                UseStamina(stamina);
                OnStaminaChanged();
                return true;
            }
            return false;
        }
    
        protected virtual void UseStamina(float stamina)
        {
            currentStamina -= stamina;
            if (currentStamina < 0)
                currentStamina = 0;
        }
    
        public virtual void RestoreStamina(float stamina)
        {
            if (stamina <= 0 || currentStamina >= maxStamina) return;

            currentStamina += stamina;
            if (currentStamina > maxStamina)
                currentStamina = maxStamina;
            
            OnStaminaChanged();
        }

        // Add a recovery over time effect
        public virtual void RecoverOvertime(float amount, float duration)
        {
            if (amount <= 0 || duration <= 0 || currentStamina >= maxStamina) return;

            activeRecoveryEffects.Add(new RecoveryOverTimeEffect(amount, duration));
        }

        // Process all active recovery effects
        protected virtual void ProcessRecoveryEffects(float deltaTime)
        {
            if (activeRecoveryEffects.Count == 0 || currentStamina >= maxStamina) return;

            float totalRecoveryThisFrame = 0;

            for (int i = activeRecoveryEffects.Count - 1; i >= 0; i--)
            {
                var effect = activeRecoveryEffects[i];

                float recoveryThisFrame = effect.RecoveryRate * deltaTime;
                if (recoveryThisFrame > effect.RemainingAmount)
                    recoveryThisFrame = effect.RemainingAmount;

                totalRecoveryThisFrame += recoveryThisFrame;
                effect.RemainingAmount -= recoveryThisFrame;
                effect.RemainingTime -= deltaTime;

                if (effect.RemainingTime <= 0 || effect.RemainingAmount <= 0)
                {
                    // Swap and pop for efficient removal
                    int lastIndex = activeRecoveryEffects.Count - 1;
                    if (i < lastIndex)
                        activeRecoveryEffects[i] = activeRecoveryEffects[lastIndex];
                    activeRecoveryEffects.RemoveAt(lastIndex);
                }
            }

            if (totalRecoveryThisFrame > 0)
                RestoreStamina(totalRecoveryThisFrame);
        }
    }
}