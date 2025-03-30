using System.Collections.Generic;
using UnityEngine;

public class IngameMenuUI : MonoBehaviour, ILifecycle<UIManager>
{
    private UIManager _uiManager;
    public UIManager UIManager => _uiManager;

    [SerializeField]
    private List<IngameMenuTabUI> _menuTabs = new List<IngameMenuTabUI>();

    [SerializeField]
    private List<IngameMenuPageUI> _menuPages = new List<IngameMenuPageUI>();


    public void Initialize(UIManager uIManager)
    {
        _uiManager = uIManager;
        foreach (IngameMenuTabUI menuTab in _menuTabs)
        {
            menuTab.Initialize(this);
        }

        foreach (IngameMenuPageUI menuPage in _menuPages)
        {
            menuPage.Initialize(this);
        }

        SwitchToMenuType(IngameMenuType.Status);
    }

    public void SwitchToMenuType(IngameMenuType menuType)
    {
        foreach (IngameMenuPageUI page in _menuPages)
        {
            if (page.ingameMenuType != menuType)
            {
                page.gameObject.SetActive(false);
            }else
            {
                page.gameObject.SetActive(true);
            }
        }

        foreach (IngameMenuTabUI menuTab in _menuTabs)
        {
            if (menuTab.ingameMenuType == menuType)
            {
                menuTab.SelectTab();
            }
            else
            {
                menuTab.DeselectTab();
            }
        }
    }

    public void Dispose()
    {
        _uiManager = null;
        foreach (IngameMenuTabUI menuTab in _menuTabs)
        {
            menuTab.Dispose();
        }
        foreach (IngameMenuPageUI menuPage in _menuPages)
        {
            menuPage.Dispose();
        }
    }

    public PlayerEquipmentUI GetPlayerEquipmentUI()
    {
        foreach (IngameMenuPageUI page in _menuPages)
        {
            if (page is PlayerEquipmentUI)
            {
                return page as PlayerEquipmentUI;
            }
        }
        return null;
    }

}
