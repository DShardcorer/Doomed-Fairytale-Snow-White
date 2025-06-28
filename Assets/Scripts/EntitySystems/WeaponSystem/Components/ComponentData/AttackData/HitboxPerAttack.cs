using System;
using UnityEngine;

namespace EntitySystems.WeaponSystem.Components.ComponentData.AttackData
{
    [Serializable]
    public class HitboxPerAttack : AttackData
    {
        public bool DrawGizmo;
        [field: SerializeField] public Rect HitboxRect { get; private set; }
    }
}