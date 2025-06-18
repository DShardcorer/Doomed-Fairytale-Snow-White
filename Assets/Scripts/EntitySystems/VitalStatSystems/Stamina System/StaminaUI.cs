using EventBus.Player;
using GeneralManagers;
using UnityEngine;
using UnityEngine.UI;

namespace EntitySystems.VitalStatSystems.Stamina_System
{
    public class StaminaUI : MonoBehaviour, ILifecycle<UIManager>
    {
        private UIManager _parent;
        public UIManager Parent => _parent;
    
        [SerializeField] private int maxStaminaForReference = 500;
        [SerializeField] private GameObject staminaBar;
        [SerializeField] private Image staminaFill;

        public void Initialize(UIManager parent)
        {
            _parent = parent;
            staminaFill.fillAmount = 1;
            PlayerVitalStatsEventSystem.OnStaminaChanged += StaminaSystem_OnStaminaChanged;
        }

        private void StaminaSystem_OnStaminaChanged(object sender, StaminaChangedEventArgs e)
        {
            // Adjust the stamina bar's scale relative to the maximum stamina for reference.
            staminaBar.transform.localScale = new Vector3((float)e.MaxStamina / maxStaminaForReference, 1, 1);
            // Update the fill amount based on current stamina.
            staminaFill.fillAmount = (float)e.CurrentStamina / e.MaxStamina;
        }

        public void Dispose()
        {
            // Unsubscribe from the event to prevent memory leaks.
            PlayerVitalStatsEventSystem.OnStaminaChanged -= StaminaSystem_OnStaminaChanged;
            _parent = null;
        }
    }
}
