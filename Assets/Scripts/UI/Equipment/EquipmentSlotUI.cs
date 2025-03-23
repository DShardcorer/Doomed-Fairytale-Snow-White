using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour, ILifecycle<PlayerEquipmentUI>, IPointerClickHandler
{
    private PlayerEquipmentUI _playerEquipmentUI;
    public PlayerEquipmentUI PlayerEquipmentUI => _playerEquipmentUI;
    [SerializeField] private EquipmentSlotType _slotType;
    private Image _iconImage;
    public EquipmentSlotType SlotType => _slotType;
    private ItemData_Equipment _itemData;

    private float _lastClickTime;
    private const float DoubleClickThreshold = 0.3f; // Time in seconds

    private void Awake()
    {
        _iconImage = GetComponent<Image>();
    }

    public void Initialize(PlayerEquipmentUI parent)
    {
        _playerEquipmentUI = parent;
    }

    public void Dispose()
    {
        _playerEquipmentUI = null;
    }

    public void UpdateUI(ItemData_Equipment itemData)
    {
        if (itemData != null)
        {
            _itemData = itemData;
            _iconImage.sprite = itemData.icon;
        }
        else
        {
            _itemData = null;
            _iconImage.sprite = null;
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
        _playerEquipmentUI.FireUnequipItemEvent(_slotType);
        _itemData = null;
        _iconImage.sprite = null;
    }
}
