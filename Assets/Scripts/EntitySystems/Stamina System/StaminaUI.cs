using UnityEngine;
using UnityEngine.UI;

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
        //Increase stamina bar size according to max stamina and max stamina for reference
        staminaBar.transform.localScale = new Vector3((float)e.MaxStamina / maxStaminaForReference, 1, 1);

        staminaFill.fillAmount = (float)e.CurrentStamina / e.MaxStamina;
    }

    public void Dispose()
    {
        _parent = null;
    }
    
}
