using System;
using UnityEngine;
using Utilities;

namespace EntitySystems.WeaponSystem.Components.ComponentData.AttackData
{
    [Serializable]
    public class MovementPerAttack: AttackData
    {
        [field: SerializeField] public DirectionComparedToFacingDirection DirectionCompareToEntityLastMovementVector2 { get; private set; }

        [field: SerializeField] public float Velocity { get; private set; }
    }

}