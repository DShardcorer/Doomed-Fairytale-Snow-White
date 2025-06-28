using EntitySystems.WeaponSystem.Components.ComponentData.AttackData;
using UnityEngine;

namespace EntitySystems.WeaponSystem.Components.ComponentData
{
    public class WeaponHitboxData: WeaponComponentData
    {
        [field:SerializeField] public HitboxPerAttack[] Hitboxes { get; private set; }
        [field:SerializeField] public LayerMask HitboxLayers { get; private set; }
    }
}