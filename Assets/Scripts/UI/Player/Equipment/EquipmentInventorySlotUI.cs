using GeneralManagers;
using Item.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Player.Equipment
{
    public class EquipmentInventorySlotUI : MonoBehaviour, IPointerClickHandler, ILifecycle<EquipmentInventoryUI>
    {
        private EquipmentInventoryUI _equipmentInventoryUI;
        private EquipmentInventoryUI EquipmentInventoryUI => _equipmentInventoryUI;

        private EquipmentInventoryItem _equipmentItem;
        public EquipmentInventoryItem EquipmentInventoryItem => _equipmentItem;

        [SerializeField] private Image _equipmentIcon;
        [SerializeField] private TextMeshProUGUI _equipmentNameText;
        [SerializeField] private TextMeshProUGUI _equipmentTypeText;
        [SerializeField] private TextMeshProUGUI _equipmentStatsText;
        [SerializeField] private TextMeshProUGUI _equippedText;

        private float _lastClickTime;
        private const float DoubleClickThreshold = 0.3f; // Time in seconds

        public void Initialize(EquipmentInventoryUI equipmentInventoryUI)
        {
            _equipmentInventoryUI = equipmentInventoryUI;
        }

        public void Dispose()
        {
            _equipmentInventoryUI = null;
        }

        public void UpdateUI(EquipmentInventoryItem item)
        {

            _equipmentItem = item;
            _equipmentIcon.sprite = item.EquipmentData.icon;
            _equipmentNameText.text = item.EquipmentData.itemName;
            _equipmentTypeText.text = item.EquipmentData.equipmentItemType.ToString();
            _equipmentStatsText.text = item.EquipmentData.GetStatsString();
            _equippedText.text = item.isEquipped ? "E" : "";
            Debug.Log("Equipped: " + item.isEquipped);

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            float timeSinceLastClick = Time.time - _lastClickTime;
            if (timeSinceLastClick < DoubleClickThreshold)
            {
                OnDoubleClick();
            }
            _lastClickTime = Time.time;
        }

        private void OnDoubleClick()
        {
            _equipmentInventoryUI.FireEquipEvent(_equipmentItem);
        }

        public void UpdateEquippedStatus()
        {
            _equippedText.text = _equipmentItem.isEquipped ? "E" : "";
        }
    }
}
