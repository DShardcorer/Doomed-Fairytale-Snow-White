using EntitySystems.WeaponSystem.Components.ComponentData.AttackData;
using UnityEngine;

namespace EntitySystems.WeaponSystem.Components.ComponentData
{
    public class WeaponDamageData: WeaponComponentData
    {
        [field: SerializeField] public DamagePerAttack[] Damages { get; private set; }

        public WeaponDamageData()
        {
            DependencyType = typeof(WeaponDamage);
        }
    }
}