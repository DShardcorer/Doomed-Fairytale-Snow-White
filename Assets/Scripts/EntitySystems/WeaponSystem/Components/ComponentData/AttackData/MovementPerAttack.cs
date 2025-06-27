using System;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace EntitySystems.WeaponSystem.Components.ComponentData.AttackData
{
    [Serializable]
    public class MovementPerAttack
    {
        [field: SerializeField] public MovementDirection DirectionCompareToEntityLastMovementVector2 { get; private set; }

        [field: SerializeField] public float Velocity { get; private set; }
    }
    public enum MovementDirection
    {
        Forward,
        Backward,
        Left,
        Right
    }
}