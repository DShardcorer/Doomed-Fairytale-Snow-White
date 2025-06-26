using GeneralManagers;
using UnityEngine;

namespace EntitySystems.WeaponSystem.Components
{
    public abstract class WeaponComponent: MonoBehaviour, ILifecycle<Weapon>
    {
        protected Weapon _weapon;
        public virtual void Initialize(Weapon parent)
        {
            _weapon = parent;
        }

        public virtual void Dispose()
        {
            _weapon = null;
        }
    }
}