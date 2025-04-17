using EventSystem.Entity;
using EventSystem.Player;
using GeneralManagers;
using Helpers;
using Pool;
using UnityEngine;

namespace General
{
    public class DamagePopupUIManager : MonoBehaviour, ILifecycle<UIManager>
    {
        private UIManager _uiManager;
        public UIManager UIManager => _uiManager;
        private PoolManager _poolManager;
        public PoolManager PoolManager => _poolManager;
        [SerializeField] private Vector2 offset = new Vector2(0, 0.5f);

        public void Initialize(UIManager controller)
        {
            _uiManager = controller;
            _poolManager = GameManager.Instance.PoolManager;
            EntityVitalStatsEventSystem.HealthChanged += OnHealthChanged;
        }

        private void OnHealthChanged(object sender, HealthChangedEventArgs e)
        {
            if (e.CurrentHealth - e.LastCurrentHealth == 0)
            {
                return;
            }

            DamagePopupUI damagePopup =
                _poolManager.GetObject(HelperUIName.DamagePopupUI).GetComponent<DamagePopupUI>();
            //Debug if damagePopup is null
            if (damagePopup == null)
            {
                Debug.Log("Damage Popup is null");
            }

            damagePopup.transform.position = ((Entity.Entity)sender).View.transform.position + (Vector3)offset;
            damagePopup.Initialize(this, e.LastCurrentHealth - e.CurrentHealth);
        }

        public void Dispose()
        {
            _uiManager = null;
            _poolManager = null;
            EntityVitalStatsEventSystem.HealthChanged -= OnHealthChanged;
        }
    }
}