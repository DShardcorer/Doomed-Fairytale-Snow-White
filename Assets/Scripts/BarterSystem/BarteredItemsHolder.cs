using System;
using System.Collections.Generic;
using Item.Inventory;
using UnityEngine;

namespace DefaultNamespace.BarterSystem
{
    public abstract class BarteredItemsHolder
    {
        public List<InventoryItem> BarteredItems = new List<InventoryItem>();
        protected int rawTotalValue = 0;
        protected float valueModifier = 1.0f;

        public int TotalValue => Mathf.RoundToInt(rawTotalValue * valueModifier);
        public float ValueModifier => valueModifier;
        public int RawTotalValue => rawTotalValue;

        public Action<List<InventoryItem>, int> OnItemsListChanged;
        public Action<float> OnModifierChanged;

        public void AddItem(InventoryItem item)
        {
            BarteredItems.Add(item);
            rawTotalValue += item.itemDataSo.value * item.stackSize;
            OnItemsListChanged?.Invoke(BarteredItems, TotalValue);
        }

        public void RemoveItem(InventoryItem item)
        {
            if (BarteredItems.Contains(item))
            {
                BarteredItems.Remove(item);
                rawTotalValue -= item.itemDataSo.value * item.stackSize;
                OnItemsListChanged?.Invoke(BarteredItems, TotalValue);
            }
        }

        public void ClearItems()
        {
            BarteredItems.Clear();
            rawTotalValue = 0;
            OnItemsListChanged?.Invoke(BarteredItems, TotalValue);
        }

        public bool ContainsItem(InventoryItem item)
        {
            return BarteredItems.Contains(item);
        }
        
        public abstract void UpdateValueModifier(float playerCharisma, float npcCharisma);
    }

    public class PlayerBarteredItemsHolder : BarteredItemsHolder
    {
        // Player items become more valuable with higher player charisma
        public override void UpdateValueModifier(float playerCharisma, float npcCharisma)
        {
            float charismaDiff = playerCharisma - npcCharisma;
            float percentPerCharismaPoint = 0.05f; // 5% per point
            
            valueModifier = 0.2f + (charismaDiff * percentPerCharismaPoint);
            valueModifier = Mathf.Clamp(valueModifier, 0.2f, 1f);
            
            OnModifierChanged?.Invoke(valueModifier);
        }
    }

    public class NpcBarteredItemsHolder : BarteredItemsHolder
    {
        // NPC items always have a higher base value
        private  float _npcBaseBias = 1.5f;
        
        public override void UpdateValueModifier(float playerCharisma, float npcCharisma)
        {
            float charismaDiff = playerCharisma - npcCharisma;
            float percentPerCharismaPoint = 0.05f; // 5% per point
            

            valueModifier = _npcBaseBias - (charismaDiff * percentPerCharismaPoint);
            
            // Ensure the modifier is at least 1.0 (no discount)
            valueModifier = Mathf.Max(valueModifier,1f);
            
            OnModifierChanged?.Invoke(valueModifier);
        }
        public void SetNpcBaseBias(float bias)
        {
            _npcBaseBias = bias;
        }
    }
}