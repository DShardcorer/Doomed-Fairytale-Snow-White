using System;

using DefaultNamespace.UI.Time;
using EntitySystems.VitalStatSystems.Health_System;
using EntitySystems.VitalStatSystems.Mana_System;
using EntitySystems.VitalStatSystems.Stamina_System;
using General;
using UI.Player;
using UnityEngine;

namespace GeneralManagers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        public Transform uiContainer;

        [SerializeField] private IngameMenuUI _ingameMenuUI;
        public IngameMenuUI IngameMenuUI => _ingameMenuUI;


        [SerializeField] private HealthUI _healthUI;
        public HealthUI HealthUI => _healthUI;

        [SerializeField] private ManaUI _manaUI;
        public ManaUI ManaUI => _manaUI;

        [SerializeField] private StaminaUI _staminaUI;
        public StaminaUI StaminaUI => _staminaUI;

        [SerializeField] private DamagePopupUIManager _damagePopupUIManager;
        public DamagePopupUIManager DamagePopupUIManager => _damagePopupUIManager;

        [SerializeField] private TimeUI _timeUI;
        public TimeUI TimeUI => _timeUI;

        public void DisableOnScreenUI()
        {
            _timeUI.gameObject.SetActive(false);
            _healthUI.gameObject.SetActive(false);
            _manaUI.gameObject.SetActive(false);
            _staminaUI.gameObject.SetActive(false);
        }

        public void EnableOnScreenUI()
        {
            _timeUI.gameObject.SetActive(true);
            _healthUI.gameObject.SetActive(true);
            _manaUI.gameObject.SetActive(true);
            _staminaUI.gameObject.SetActive(true);
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }


        public void Initialize()
        {
            GameManager.Instance.InputManager.openMenuInputted += InputManager_openMenuInputted;
            _ingameMenuUI.Initialize(this);
            _ingameMenuUI.gameObject.SetActive(false);
            _healthUI.Initialize(this);
            _manaUI.Initialize(this);
            _staminaUI.Initialize(this);
            _damagePopupUIManager.Initialize(this);
        }


        private void InputManager_openMenuInputted(object sender, EventArgs e)
        {
            _ingameMenuUI.gameObject.SetActive(!_ingameMenuUI.gameObject.activeSelf);
        }
    }
}