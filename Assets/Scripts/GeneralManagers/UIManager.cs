using System;
using DefaultNamespace.UI.Barter;
using DefaultNamespace.UI.Time;
using EntitySystems.VitalStatSystems.Health_System;
using EntitySystems.VitalStatSystems.Mana_System;
using EntitySystems.VitalStatSystems.Stamina_System;
using General;
using UI.Player;
using UI.Player.Skill;
using UnityEngine;

using UnityEngine.EventSystems;
using System.Collections.Generic;
namespace GeneralManagers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        public Transform uiContainer;


        [SerializeField] private IngameMenuUI _ingameMenuUI;
        public IngameMenuUI IngameMenuUI => _ingameMenuUI;
        [SerializeField] private VolumeSettingsUI _volumeSettingsUI;
        public VolumeSettingsUI VolumeSettingsUI => _volumeSettingsUI;

        [SerializeField] private HealthUI _healthUI;
        public HealthUI HealthUI => _healthUI;

        [SerializeField] private ManaUI _manaUI;
        public ManaUI ManaUI => _manaUI;

        [SerializeField] private StaminaUI _staminaUI;
        public StaminaUI StaminaUI => _staminaUI;

        [SerializeField] private DamagePopupUIManager _damagePopupUIManager;
        public DamagePopupUIManager DamagePopupUIManager => _damagePopupUIManager;

        [SerializeField] private PlayerHotbarUI _playerHotbarUI;
        public PlayerHotbarUI PlayerHotbarUI => _playerHotbarUI;

        [SerializeField] private TimeUI _timeUI;
        public TimeUI TimeUI => _timeUI;

        [SerializeField] private GameObject popupContainer;
        public GameObject PopupContainer => popupContainer;

        [SerializeField] private BarterUI _barterUI;
        public BarterUI BarterUI => _barterUI;

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
            _volumeSettingsUI.Initialize(this);
            _healthUI.Initialize(this);
            _manaUI.Initialize(this);
            _staminaUI.Initialize(this);
            _damagePopupUIManager.Initialize(this);
            _barterUI.Initialize(this);
            _playerHotbarUI.Initialize(this);
        }


        private void Update()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                // Check for UI elements first
                PointerEventData eventData = new PointerEventData(EventSystem.current);
                eventData.position = UnityEngine.Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventData, results);

                if (results.Count > 0)
                {
                    Debug.LogWarning("Clicked on UI: " + results[0].gameObject.name);
                }
                else
                {
                    // If no UI was clicked, check for world objects
                    Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        Debug.LogWarning("Clicked on: " + hit.collider.name);
                    }
                }
            }
        }

        public void Dispose()
        {
            _ingameMenuUI.Dispose();
            _healthUI.Dispose();
            _manaUI.Dispose();
            _staminaUI.Dispose();
            _damagePopupUIManager.Dispose();
            _barterUI.Dispose();
            GameManager.Instance.InputManager.openMenuInputted -= InputManager_openMenuInputted;
            Instance = null;
        }


        private void InputManager_openMenuInputted(object sender, EventArgs e)
        {
            _ingameMenuUI.gameObject.SetActive(!_ingameMenuUI.gameObject.activeSelf);
        }
    }
}