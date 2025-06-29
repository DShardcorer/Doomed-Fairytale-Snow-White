using EntitySystems.WeaponSystem;
using UnityEngine;

namespace Item
{
    public class ItemDataSOWeapon : ItemDataSOEquipment
    {
        [field: SerializeField] public WeaponDataSO WeaponData { get; private set; }
    }
}
