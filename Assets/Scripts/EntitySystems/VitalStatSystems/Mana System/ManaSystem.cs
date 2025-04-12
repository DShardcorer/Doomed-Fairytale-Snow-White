using GeneralManagers;

namespace EntitySystems.VitalStatSystems.Mana_System
{
    public class ManaSystem : ILifecycle<Entity.Entity>
    {
        protected Entity.Entity _entity;
        public Entity.Entity Entity => _entity;
    
        protected float maxMana;
        public float MaxMana => maxMana;
    
        protected float currentMana;
        public float CurrentMana => currentMana;

        public ManaSystem(float maxMana)
        {
            this.maxMana = maxMana;
            currentMana = maxMana;
        }

        public virtual void Initialize(Entity.Entity parent)
        {
            _entity = parent;
        }

        // Virtual method that derived classes can override to invoke mana changed events.
        protected virtual void OnManaChanged() { }

        public virtual void InvokeInitialEvents()
        {
            // Base implementation does nothing.
        }

        public virtual void Dispose()
        {
            _entity = null;
        }

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
            {
                currentMana = 0;
            }
        }

        public virtual void RestoreMana(float mana)
        {
            currentMana += mana;
            if (currentMana > maxMana)
            {
                currentMana = maxMana;
            }
            OnManaChanged();
        }
    }
}
