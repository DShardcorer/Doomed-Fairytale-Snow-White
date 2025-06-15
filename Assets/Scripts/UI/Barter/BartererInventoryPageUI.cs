using System.Collections.Generic;
using GeneralManagers;
using Helpers;
using Item.Inventory;
using Pool;
using UI.Player.Inventory;
using UnityEngine;

namespace DefaultNamespace.UI.Barter
{
    public class BartererInventoryPageUI: MonoBehaviour, ILifecycle<BarterUI>
    {
        private BartererType bartererType;
        public BartererType BartererType => bartererType;
        private BarterUI _parent;
        public BarterUI Parent => _parent;
        private List<BarterInventoryItemSlotUI> barterItemSlotUis = new List<BarterInventoryItemSlotUI>();

        private PoolManager _poolManager;
        public void Initialize(BarterUI parent, BartererType type)
        {
            Initialize(parent);
            SetBartererType(type);
        }
        public void Initialize(BarterUI parent)
        {
            _parent = parent;
            _poolManager = GameManager.Instance.PoolManager;
        }
        public void SetBartererType(BartererType type)
        {
            bartererType = type;
        }

        public void UpdateUI(List<InventoryItem> items)
        {
            // Ensure we have the correct number of slots
            AdjustItemSlotCount(items.Count);

            // Update each slot with the corresponding item
            for (int i = 0; i < items.Count; i++)
            {
                barterItemSlotUis[i].UpdateUI(items[i]);
            }

            // Disable extra slots if necessary
            for (int i = items.Count; i < barterItemSlotUis.Count; i++)
            {
                barterItemSlotUis[i].UpdateUI(null);
            }
        }

        private void AdjustItemSlotCount(int requiredCount)
        {
            // Add more slots if needed
            while (barterItemSlotUis.Count < requiredCount)
            {
                BarterInventoryItemSlotUI newSlot = _poolManager.GetObject(HelperUIName.BarterInventoryItemSlotUI).GetComponent<BarterInventoryItemSlotUI>();
                newSlot.transform.SetParent(transform, false);
                barterItemSlotUis.Add(newSlot);
                newSlot.Initialize(this);
            }

            // Return excess slots to the pool if needed
            while (barterItemSlotUis.Count > requiredCount)
            {
                BarterInventoryItemSlotUI excessSlot = barterItemSlotUis[barterItemSlotUis.Count - 1];
                excessSlot.Dispose();
                _poolManager.ReturnObject(HelperUIName.BarterInventoryItemSlotUI, excessSlot.gameObject);
                barterItemSlotUis.RemoveAt(barterItemSlotUis.Count - 1);
            }
        }

        public void Dispose()
        {
            barterItemSlotUis.Clear();
            barterItemSlotUis = null;
            _poolManager = null;
        }
    }
}