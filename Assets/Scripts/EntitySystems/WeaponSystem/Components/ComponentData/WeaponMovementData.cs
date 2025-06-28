using System;
using EntitySystems.WeaponSystem.Components.ComponentData.AttackData;
using UnityEngine;

namespace EntitySystems.WeaponSystem.Components.ComponentData
{
    [Serializable]
    public class WeaponMovementData: WeaponComponentData
    {
        [field: SerializeField] public MovementPerAttack[] Movements;
    }
}