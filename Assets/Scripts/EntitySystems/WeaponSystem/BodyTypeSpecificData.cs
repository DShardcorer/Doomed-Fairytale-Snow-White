using System;
using EntityBase;
using EntitySystems.WeaponSystem.Components;
using EntitySystems.WeaponSystem.Components.ComponentData;
using UnityEngine;

namespace EntitySystems.WeaponSystem
{
    [Serializable]
    public class BodyTypeSpecificData
    {
        [field: SerializeField] public BodyType BodyType { get; private set; }
        
        [field:SerializeField] public RuntimeAnimatorController WeaponAnimatorController { get; private set; }
        
        [field: SerializeReference] public WeaponHitboxData WeaponHitbox { get; private set; }
    }
}