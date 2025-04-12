using EntitySystems.Equipment;
using GeneralManagers;
using Item.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Player.Equipment
{
    public class EquipmentSlotUI : MonoBehaviour, ILifecycle<PlayerEquipmentUI>, IPointerClickHandler
    {
        private PlayerEquipmentUI _playerEquipmentUI;
        public PlayerEquipmentUI PlayerEquipmentUI => _playerEquipmentUI;
        [SerializeField] private EquipmentSlotType _slotType;
        [SerializeField] private Image _iconImage;
        public EquipmentSlotType SlotType => _slotType;
        private EquipmentInventoryItem _item;

        private float _lastClickTime;
        private const float DoubleClickThreshold = 0.3f; // Time in seconds


        public void Initialize(PlayerEquipmentUI parent)
        {
            _playerEquipmentUI = parent;
        }

        public void Dispose()
        {
            _playerEquipmentUI = null;
        }

        public void UpdateUI(EquipmentInventoryItem item)
        {

            if (item != null)
            {
                _item = item;
                _iconImage.sprite = _item.EquipmentData.icon;
                _iconImage.GetComponent<CanvasGroup>().alpha = 1;
            }
            else
            {
                _item = null;
                _iconImage.sprite = null;
                _iconImage.GetComponent<CanvasGroup>().alpha = 0;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            float timeSinceLastClick = Time.time - _lastClickTime;
            if (timeSinceLastClick < DoubleClickThreshold)
            {
                Unequip();
            }
            _lastClickTime = Time.time;
        }

        private void Unequip()
        {
            _item.isEquipped = false;
            _playerEquipmentUI.FireUnequipItemEvent(_item);
            _item = null;
            _iconImage.sprite = null;
            _iconImage.GetComponent<CanvasGroup>().alpha = 0;
        }
    }
}
