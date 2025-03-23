using UnityEngine;
using UnityEngine.EventSystems;

public class IngameMenuTabUI : MonoBehaviour, IPointerDownHandler, ILifecycle<IngameMenuUI>
{
    private IngameMenuUI _ingameMenuUI;
    public IngameMenuUI IngameMenuUI => _ingameMenuUI;
    public IngameMenuType ingameMenuType;
    public void Initialize(IngameMenuUI parent)
    {
        _ingameMenuUI = parent;
        Debug.Log("Ingame Menu Tab Initialized");
    }

    public void Dispose()
    {
        _ingameMenuUI = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer down event detected.");
        _ingameMenuUI.SwitchToMenuType(ingameMenuType);
    }

    public void SelectTab()
    {
        gameObject.GetComponent<CanvasGroup>().alpha = 1f;
    }

    public void DeselectTab()
    {
        gameObject.GetComponent<CanvasGroup>().alpha = 0.5f;
    }
}
