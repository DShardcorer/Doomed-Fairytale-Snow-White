using System.Collections.Generic;
using EventSystem.Player;
using Item.Inventory;
using TMPro;

namespace UI.Player.Inventory
{
    public class PlayerInventoryUI : IngameMenuPageUI
    {
        public List<PlayerInventoryTabUI> playerInventoryTabs;
        public List<InventoryPageUI> playerInventoryPages;
        public TextMeshProUGUI weightText;

        public override void Initialize(IngameMenuUI ingameMenuUI)
        {
            base.Initialize(ingameMenuUI);

            // Subscribe to PlayerInventoryEventSystem
            PlayerInventoryEventSystem.OnItemListChanged += Inventory_OnItemListChanged;
            PlayerInventoryEventSystem.OnMaterialItemListChanged += Inventory_OnMaterialItemListChanged;
            PlayerInventoryEventSystem.OnConsumableItemListChanged += Inventory_OnConsumableItemListChanged;
            PlayerInventoryEventSystem.OnEquipmentItemListChanged += Inventory_OnEquipmentItemListChanged;
            PlayerInventoryEventSystem.OnMiscellaneousItemListChanged += Inventory_OnMiscellaneousItemListChanged;
            PlayerInventoryEventSystem.OnWeightChanged += Inventory_OnWeightChanged;

            foreach (InventoryPageUI page in playerInventoryPages)
            {
                page.Initialize();
            }

            foreach (PlayerInventoryTabUI tab in playerInventoryTabs)
            {
                tab.Initialize(this);
                tab.DeselectTab();
            }

            // Select the first tab and show the first page
            playerInventoryTabs[0].SelectTab();
            foreach (InventoryPageUI page in playerInventoryPages)
            {
                page.gameObject.SetActive(false);
            }
            playerInventoryPages[0].gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            // Unsubscribe to prevent memory leaks
            PlayerInventoryEventSystem.OnItemListChanged -= Inventory_OnItemListChanged;
            PlayerInventoryEventSystem.OnMaterialItemListChanged -= Inventory_OnMaterialItemListChanged;
            PlayerInventoryEventSystem.OnConsumableItemListChanged -= Inventory_OnConsumableItemListChanged;
            PlayerInventoryEventSystem.OnEquipmentItemListChanged -= Inventory_OnEquipmentItemListChanged;
            PlayerInventoryEventSystem.OnMiscellaneousItemListChanged -= Inventory_OnMiscellaneousItemListChanged;
            PlayerInventoryEventSystem.OnWeightChanged -= Inventory_OnWeightChanged;
        }

        private void Inventory_OnWeightChanged(object sender, WeightEventArgs e)
        {
            weightText.text = $"{e.currentWeight} / {e.weightCapacity}";
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
            foreach (InventoryPageUI page in playerInventoryPages)
            {
                page.UpdateUI(items);
            }
        }

        public void UpdateInventoryUI(PlayerInventoryType playerInventoryType, List<InventoryItem> items)
        {
            foreach (InventoryPageUI page in playerInventoryPages)
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

            foreach (InventoryPageUI page in playerInventoryPages)
            {
                page.gameObject.SetActive(page.playerInventoryType == playerInventoryType);
            }
        }
    }
}
