using System.Collections.Generic;
using DefaultNamespace.EntitySystems.VitalStatSystems;
using GeneralManagers;
using UnityEngine;

namespace EntitySystems.VitalStatSystems.Mana_System
{
    public class ManaSystem : ILifecycle<Entity.Entity>, IUpdatable
    {
        protected Entity.Entity _entity;
        public Entity.Entity Entity => _entity;
    
        protected float maxMana;
        public float MaxMana => maxMana;
    
        protected float currentMana;
        public float CurrentMana => currentMana;



        // Collection of active recovery effects
        protected List<RecoveryOverTimeEffect> activeRecoveryEffects = new List<RecoveryOverTimeEffect>(4);

        public ManaSystem(float maxMana)
        {
            this.maxMana = maxMana;
            currentMana = maxMana;
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
            // Process recovery effects each frame
            ProcessRecoveryEffects(Time.deltaTime);
        }

        protected virtual void OnManaChanged() { }

        public virtual void InvokeInitialEvents() { }

        public virtual bool TryUseMana(float mana)
        {
            if (currentMana >= mana)
            {
                UseMana(mana);
                OnManaChanged();
                return true;
            }
            return false;
        }

        protected void UseMana(float mana)
        {
            currentMana -= mana;
            if (currentMana < 0)
                currentMana = 0;
        }

        public virtual void RestoreMana(float mana)
        {
            if (mana <= 0 || currentMana >= maxMana) return;

            currentMana += mana;
            if (currentMana > maxMana)
                currentMana = maxMana;
            
            OnManaChanged();
        }

        // Add a recovery over time effect
        public virtual void RecoverOvertime(float amount, float duration)
        {
            if (amount <= 0 || duration <= 0 || currentMana >= maxMana) return;

            activeRecoveryEffects.Add(new RecoveryOverTimeEffect(amount, duration));
        }

        // Process all active recovery effects
        protected virtual void ProcessRecoveryEffects(float deltaTime)
        {
            if (activeRecoveryEffects.Count == 0 || currentMana >= maxMana) return;

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
                RestoreMana(totalRecoveryThisFrame);
        }
    }
}