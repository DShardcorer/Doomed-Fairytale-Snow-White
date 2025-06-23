using EntityBase;
using EventBus.Entity;
using EventBus.Player;
using GeneralManagers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.General
{
    public class EntityHealthbarUI : MonoBehaviour, ILifecycle<EntityView>
    {
        private EntityView _parent;
        [SerializeField] private Image healthbarFill;



        public void Initialize(EntityView parent)
        {
            _parent = parent;
            healthbarFill.fillAmount = 1f;
            _parent.Parent.HealthSystem.OnHealthChangedHook += OnHealthChanged;
        }

        private void OnHealthChanged(HealthChangedEventArgs args)
        {
            healthbarFill.fillAmount = args.CurrentHealth / args.MaxHealth;
        }

        public void Dispose()
        {
            _parent = null;
            if (healthbarFill != null)
            {
                healthbarFill.fillAmount = 0f; // Reset the fill amount when disposing
            }
            if (_parent != null && _parent.Parent != null && _parent.Parent.HealthSystem != null)
            {
                _parent.Parent.HealthSystem.OnHealthChangedHook -= OnHealthChanged; // Unsubscribe from the event
            }
            
        }
    }
}
