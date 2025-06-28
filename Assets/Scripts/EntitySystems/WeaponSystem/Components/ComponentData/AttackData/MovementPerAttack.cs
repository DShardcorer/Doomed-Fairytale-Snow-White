using System;
using UnityEngine;
using Utility;

namespace EntitySystems.WeaponSystem.Components.ComponentData.AttackData
{
    [Serializable]
    public class MovementPerAttack: AttackData
    {
        [field: SerializeField] public Direction DirectionCompareToEntityLastMovementVector2 { get; private set; }

        [field: SerializeField] public float Velocity { get; private set; }
    }

}