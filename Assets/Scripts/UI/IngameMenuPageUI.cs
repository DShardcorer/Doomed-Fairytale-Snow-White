using UnityEngine;

public class IngameMenuPageUI : MonoBehaviour, ILifecycle<IngameMenuUI>
{
    private IngameMenuUI _ingameMenuUI;
    public IngameMenuUI IngameMenuUI => _ingameMenuUI;
    public IngameMenuType ingameMenuType;
    public virtual void Initialize(IngameMenuUI parent)
    {
        _ingameMenuUI = parent;
    }

    public virtual void Dispose()
    {
        _ingameMenuUI = null;
    }

    public virtual void ShowPage()
    {
        gameObject.SetActive(true);
    }

    public virtual void HidePage()
    {
        gameObject.SetActive(false);
    }

    
}
