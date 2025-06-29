using System;
using UnityEngine;

namespace EntitySystems.WeaponSystem.Components.ComponentData.AttackData
{
    [Serializable]
    public class DamagePerAttack:AttackData
    {
        [field: SerializeField] public float Coefficient { get; private set; }
    }
}