using EventSystem.Entity;
using EventSystem.Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI.General
{
    public class EntityHealthbarUI : MonoBehaviour
    {
        [SerializeField] private Image healthbarFill;


        public void OnEnable()
        {
            EntityVitalStatsEventSystem.HealthChanged += OnHealthChanged;
            healthbarFill.fillAmount = 1f;
        }
        public void OnDisable()
        {
            EntityVitalStatsEventSystem.HealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(Entity.Entity entity, HealthChangedEventArgs args)
        {
            healthbarFill.fillAmount = args.CurrentHealth / args.MaxHealth;
        }
    }
}
