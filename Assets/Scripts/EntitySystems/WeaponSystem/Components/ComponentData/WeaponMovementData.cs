using EntitySystems.WeaponSystem.Components.ComponentData.AttackData;
using UnityEngine;

namespace EntitySystems.WeaponSystem.Components.ComponentData
{
    public class WeaponMovementData: WeaponComponentData
    {
        [field:SerializeField] public MovementPerAttack[] Movements { get; private set; }
    }
}