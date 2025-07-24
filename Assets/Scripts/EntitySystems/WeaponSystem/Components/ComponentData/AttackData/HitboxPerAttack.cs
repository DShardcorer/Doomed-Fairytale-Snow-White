using System;
using UnityEngine;
using Utilities;

namespace EntitySystems.WeaponSystem.Components.ComponentData.AttackData
{
    [Serializable]
    public class HitboxPerAttack : AttackData
    {
        [field: SerializeField] public HitboxPerAttackDirection[] DirectionalHitboxes { get; private set; }
        
    }

    [Serializable]
    public class HitboxPerAttackDirection
    {
        public bool DrawGizmo;
        [field: SerializeField] public Direction Direction { get; private set; }
        [field: SerializeField] public float Angle { get; private set; }
        [field: SerializeField] public Rect HitboxRect { get; private set; }

    }
}