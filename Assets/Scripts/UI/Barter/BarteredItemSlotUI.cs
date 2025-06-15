using GeneralManagers;
using Item.Inventory;
using TMPro;
using UI.Player.Inventory;
using UnityEngine;

namespace DefaultNamespace.UI.Barter
{
    public class BarteredItemSlotUI: ItemSlotUI, ILifecycle<BarteredItemsHolderUI>
    {
        [SerializeField] private TextMeshProUGUI barterPriceText;
        private BarteredItemsHolderUI _parent;

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
                        GameManager.Instance.BarterManager.RemovePlayerBarteredItem(item);
                        break;
                    case BartererType.NPC:
                        GameManager.Instance.BarterManager.RemoveNpcBarteredItem(item);
                        break;
                }
            }
        }



        public void Initialize(BarteredItemsHolderUI parent)
        {
            _parent = parent;
        }

        public void Dispose()
        {
            _parent = null;
        }
    }
}