using UnityEngine;

public class EquipmentSlotUI : MonoBehaviour, ILifecycle<PlayerEquipmentUI>
{
    private PlayerEquipmentUI _playerEquipmentUI;
    public PlayerEquipmentUI PlayerEquipmentUI => _playerEquipmentUI;
    [SerializeField] private EquipmentSlotType _slotType;
    public EquipmentSlotType SlotType => _slotType;


    public void Initialize(PlayerEquipmentUI parent)
    {
        _playerEquipmentUI = parent;
    }

    public void Dispose()
    {
        _playerEquipmentUI = null;
    }

}
