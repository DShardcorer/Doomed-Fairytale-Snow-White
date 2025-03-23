using System;
using System.Collections.Generic;
using TMPro;


public class PlayerInventoryUI : IngameMenuPageUI
{
    public List<PlayerInventoryTabUI> playerInventoryTabs;

    public List<PlayerInventoryPageUI> playerInventoryPages;

    public TextMeshProUGUI weightText;

    

    public override void Initialize(IngameMenuUI ingameMenuUI)
    {
        base.Initialize(ingameMenuUI);
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

        foreach (PlayerInventoryTabUI tab in playerInventoryTabs)
        {
            tab.DeselectTab();
        }
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
    


}
