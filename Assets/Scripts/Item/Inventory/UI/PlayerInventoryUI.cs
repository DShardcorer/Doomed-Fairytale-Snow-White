using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryUI : MonoBehaviour, ILifecycle<UIManager>
{
    private UIManager _uiManager;
    public UIManager UIManager => _uiManager;
    private PlayerInventoryManager _playerInventoryManager;
    public PlayerInventoryManager PlayerInventoryManager => _playerInventoryManager;

    public List<PlayerInventoryTabUI> playerInventoryTabs;

    public List<PlayerInventoryPageUI> playerInventoryPages;

    public void Initialize(UIManager manager)
    {
        _uiManager = manager;
        _playerInventoryManager = GameManager.Instance.PlayerInventoryManager;
        foreach (PlayerInventoryPageUI page in playerInventoryPages)
        {
            page.Initialize(this);
        }
        foreach (PlayerInventoryTabUI tab in playerInventoryTabs)
        {
            tab.Initialize(this);
        }
        Debug.Log("Player Inventory UI Initialized");

        //Deselct all tabs
        foreach (PlayerInventoryTabUI tab in playerInventoryTabs)
        {
            tab.DeselectTab();
        }
        //Select the first tab
        playerInventoryTabs[0].SelectTab();
        //Hide all pages
        foreach (PlayerInventoryPageUI page in playerInventoryPages)
        {
            page.gameObject.SetActive(false);
        }
        //Show the first page
        playerInventoryPages[0].gameObject.SetActive(true);
        
    }
    public void UpdateAllUI()
    {
        foreach (PlayerInventoryPageUI page in playerInventoryPages)
        {
            page.UpdateUI();
        }
    }
    public void UpdateInventoryUI(PlayerInventoryType playerInventoryType)
    {
        foreach (PlayerInventoryPageUI page in playerInventoryPages)
        {
            if (page.playerInventoryType == playerInventoryType)
            {
                page.UpdateUI();
            }
        }
    }

    public void SwitchToInventoryType(PlayerInventoryType playerInventoryType)
    {
        foreach (PlayerInventoryTabUI tab in playerInventoryTabs)
        {
            if (tab.playerInventoryType == playerInventoryType)
            {
                tab.SelectTab();
            }
            else
            {
                tab.DeselectTab();
            }
        }
        foreach (PlayerInventoryPageUI page in playerInventoryPages)
        {
            if (page.playerInventoryType == playerInventoryType)
            {
                page.gameObject.SetActive(true);
            }
            else
            {
                page.gameObject.SetActive(false);
            }
        }
    }
    


    public void Dispose()
    {
        _playerInventoryManager = null;
    }
}
