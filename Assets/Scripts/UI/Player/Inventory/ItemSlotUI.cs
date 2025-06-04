using Item;
using Item.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Player.Inventory
{
    public class ItemSlotUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _stackSizeText;
        private float _lastClickTime;
        private const float DoubleClickThreshold = 0.3f; // Time in seconds


        [FormerlySerializedAs("_item")] public InventoryItem item;

        public void UpdateUI(InventoryItem item)
        {
            this.item = item;
            _icon.sprite = item.itemDataSo.icon;
            if (this.item.stackSize > 1)
            {
                _stackSizeText.text = this.item.stackSize.ToString();
            }
            else
            {
                _stackSizeText.text = "";
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Time.time - _lastClickTime < DoubleClickThreshold)
            {
                OnDoubleClick();
            }
            _lastClickTime = Time.time;
        }

        private void OnDoubleClick()
        {
            if(item.itemDataSo.itemType == ItemType.Equipment){
                Debug.Log("Equipping item: " + item.itemDataSo.itemName);
            }
        }



    }
}
