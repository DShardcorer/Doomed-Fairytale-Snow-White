using GeneralManagers;
using Item.Inventory;
using TMPro;
using UI.Player.Inventory;
using UnityEngine;

namespace DefaultNamespace.UI.Barter
{
    public class BarterInventoryItemSlotUI: ItemSlotUI, ILifecycle<BartererInventoryPageUI>
    {
        [SerializeField] private TextMeshProUGUI barterPriceText;
        private BartererInventoryPageUI _parent;

        public override void UpdateUI(InventoryItem item)
        {
            barterPriceText.text = item?.itemDataSo.value.ToString() + "G";
            base.UpdateUI(item);
        }

        protected override void OnDoubleClick()
        {
            if (item != null && item.stackSize > 1)
            {
                _parent.Parent.StackSplitInputterUI.Show(item, _parent.BartererType);
            }
            else
            {
                switch (_parent.BartererType)
                {
                    case BartererType.Player:
                        GameManager.Instance.BarterManager.AddPlayerBarteredItem(item);
                        break;
                    case BartererType.NPC:
                        GameManager.Instance.BarterManager.AddNpcBarteredItem(item);
                        break;
                }
            }
            DisableItemInfoPopup();
        }


        public void Initialize(BartererInventoryPageUI parent)
        {
            _parent = parent;
        }

        public void Dispose()
        {
            _parent = null;
        }
    }
}