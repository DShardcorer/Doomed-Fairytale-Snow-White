using EntitySystems.WeaponSystem;
using UnityEngine;

namespace Item
{
    [CreateAssetMenu(fileName = "ItemDataSOEquipment_Weapon", menuName = "ItemData/Equipment ItemData/Weapon")]
    public class ItemDataSOEquipment_Weapon : ItemDataSOEquipment
    {
        [field: SerializeField] public WeaponDataSO WeaponData { get; private set; }
    }
}
