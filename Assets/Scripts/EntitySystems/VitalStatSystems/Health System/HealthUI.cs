using EventSystem.Player;
using GeneralManagers;
using UnityEngine;
using UnityEngine.UI;

namespace EntitySystems.VitalStatSystems.Health_System
{
    public class HealthUI : MonoBehaviour, ILifecycle<UIManager>
    {
        private UIManager _parent;
        public UIManager Parent => _parent;
        [SerializeField] private int maxHealthForReference = 500;

        [SerializeField] private GameObject healthBar;

        [SerializeField] private Image healthFill;

        public void Initialize(UIManager parent)
        {
            _parent = parent;
            healthFill.fillAmount = 1;
            PlayerVitalStatsEventSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        }

        private void HealthSystem_OnHealthChanged(object sender, HealthChangedEventArgs e)
        {
            //Increase health bar size according to max health and max health for reference
            healthBar.transform.localScale = new Vector3( (float)e.MaxHealth/maxHealthForReference, 1, 1);

            healthFill.fillAmount = (float)e.CurrentHealth / e.MaxHealth;
        }

        public void Dispose()
        {
            _parent = null;
        }


    }
}