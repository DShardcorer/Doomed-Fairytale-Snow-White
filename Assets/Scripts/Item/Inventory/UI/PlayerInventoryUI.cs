using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryUI : MonoBehaviour, ILifecycle<UIManager>
{
    private UIManager _uiManager;
    public UIManager UIManager => _uiManager;


    public List<PlayerInventoryTabUI> playerInventoryTabs;

    public List<PlayerInventoryPageUI> playerInventoryPages;

    public TextMeshProUGUI weightText;

    

    public void Initialize(UIManager manager)
    {
        _uiManager = manager;
        GameManager.Instance.PlayerManager.GetPlayer().Inventory.OnItemListChanged += Inventory_OnItemListChanged;
        GameManager.Instance.PlayerManager.GetPlayer().Inventory.OnMaterialItemListChanged += Inventory_OnMaterialItemListChanged;
        GameManager.Instance.PlayerManager.GetPlayer().Inventory.OnConsumableItemListChanged += Inventory_OnConsumableItemListChanged;
        GameManager.Instance.PlayerManager.GetPlayer().Inventory.OnEquipmentItemListChanged += Inventory_OnEquipmentItemListChanged;
        GameManager.Instance.PlayerManager.GetPlayer().Inventory.OnMiscellaneousItemListChanged += Inventory_OnMiscellaneousItemListChanged;
        GameManager.Instance.PlayerManager.GetPlayer().Inventory.OnWeightChanged += Inventory_OnWeightChanged;
        
        foreach (PlayerInventoryPageUI page in playerInventoryPages)
        {
            page.Initialize(this);
        }
        foreach (PlayerInventoryTabUI tab in playerInventoryTabs)
        {
            tab.Initialize(this);
        }

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

    private void Inventory_OnWeightChanged(object sender, Inventory.WeightEventArgs e)
    {
        weightText.text = e.currentWeight + " / " + e.weightCapacity;
    }

    private void Inventory_OnMiscellaneousItemListChanged(object sender, List<InventoryItem> e)
    {
        UpdateInventoryUI(PlayerInventoryType.Miscellaneous, e);
    }

    private void Inventory_OnEquipmentItemListChanged(object sender, List<InventoryItem> e)
    {
        UpdateInventoryUI(PlayerInventoryType.Equipment, e);
    }

    private void Inventory_OnConsumableItemListChanged(object sender, List<InventoryItem> e)
    {
        UpdateInventoryUI(PlayerInventoryType.Consumable, e);
    }

    private void Inventory_OnMaterialItemListChanged(object sender, List<InventoryItem> e)
    {
        UpdateInventoryUI(PlayerInventoryType.Material, e);
    }

    private void Inventory_OnItemListChanged(object sender, List<InventoryItem> e)
    {
        UpdateInventoryUI(PlayerInventoryType.All, e);
    }

    public void UpdateAllUI(List<InventoryItem> items)
    {
        foreach (PlayerInventoryPageUI page in playerInventoryPages)
        {
            page.UpdateUI(items);
        }
    }
    public void UpdateInventoryUI(PlayerInventoryType playerInventoryType, List<InventoryItem> items)
    {
        foreach (PlayerInventoryPageUI page in playerInventoryPages)
        {
            if (page.playerInventoryType == playerInventoryType)
            {
                page.UpdateUI(items);
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
        _uiManager = null;
    }
}
