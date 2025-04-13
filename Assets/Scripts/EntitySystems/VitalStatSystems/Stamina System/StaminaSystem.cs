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

        public StaminaSystem(int maxStamina)
        {
            this.maxStamina = maxStamina;
            currentStamina = maxStamina;
        }
    
        public virtual void Initialize(Entity.Entity parent)
        {
            entity = parent;
            // Register with the update manager
            GameManager.Instance.UpdateManager.AddUpdatable(this);
        }
    
        // Virtual method for derived classes to override with event invocation
        protected virtual void OnStaminaChanged() { }
    
        public virtual void InvokeInitialEvents()
        {
            // Base implementation does nothing.
        }
    
        public virtual void Dispose()
        {
            entity = null;
            // Remove from update manager
            GameManager.Instance.UpdateManager.RemoveUpdatable(this);
        }
    
        public virtual void UpdateLogic()
        {
            // Recover 5% of max stamina every second
            RestoreStamina(maxStamina / 20 * Time.deltaTime);
        }
    
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
            {
                currentStamina = 0;
            }
        }
    
        public virtual void RestoreStamina(float stamina)
        {
            if (currentStamina >= maxStamina)
            {
                return;
            }
            currentStamina += stamina;
            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
            OnStaminaChanged();
        }
    }
}
