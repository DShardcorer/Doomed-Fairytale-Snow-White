using System.Collections.Generic;
using GeneralManagers;
using Helpers;
using Item.Inventory;
using Pool;
using UnityEngine;

namespace DefaultNamespace.UI.Barter
{
    public class BarteredItemsHolderUI:MonoBehaviour, ILifecycle<BarterUI>
    {
        private BartererType bartererType;
        public BartererType BartererType => bartererType;
        private BarterUI _parent;
        public BarterUI Parent => _parent;
        private List<BarteredItemSlotUI> barteredItemSlotUis = new List<BarteredItemSlotUI>();

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
                barteredItemSlotUis[i].UpdateUI(items[i]);
            }

            // Disable extra slots if necessary
            for (int i = items.Count; i < barteredItemSlotUis.Count; i++)
            {
                barteredItemSlotUis[i].UpdateUI(null);
            }
        }

        private void AdjustItemSlotCount(int requiredCount)
        {
            // Add more slots if needed
            while (barteredItemSlotUis.Count < requiredCount)
            {
                BarteredItemSlotUI newSlot = _poolManager.GetObject(HelperUIName.BarteredItemSlotUI).GetComponent<BarteredItemSlotUI>();
                newSlot.transform.SetParent(transform, false);
                barteredItemSlotUis.Add(newSlot);
                newSlot.Initialize(this);
            }

            // Return excess slots to the pool if needed
            while (barteredItemSlotUis.Count > requiredCount)
            {
                BarteredItemSlotUI excessSlot = barteredItemSlotUis[barteredItemSlotUis.Count - 1];
                excessSlot.Dispose();
                _poolManager.ReturnObject(HelperUIName.BarteredItemSlotUI, excessSlot.gameObject);
                barteredItemSlotUis.RemoveAt(barteredItemSlotUis.Count - 1);
            }
        }

        public void Dispose()
        {
            barteredItemSlotUis.Clear();
            barteredItemSlotUis = null;
            _poolManager = null;
        }
    }
}