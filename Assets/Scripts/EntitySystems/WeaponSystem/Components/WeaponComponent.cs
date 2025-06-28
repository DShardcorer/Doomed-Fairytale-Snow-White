using EntityBase;
using GeneralManagers;
using UnityEngine;

namespace EntitySystems.WeaponSystem.Components
{
    public abstract class WeaponComponent: ILifecycle<Weapon>
    {
        protected Weapon _weapon;
        protected Entity _entity;
        protected EntityView _entityView;
        protected AnimationTriggers _entityAnimationTriggers;
        protected WeaponAnimationTriggers _weaponAnimationTriggers;
        public virtual void Initialize(Weapon parent)
        {
            _weapon = parent;
            _entity = _weapon.Parent.Parent;
            _entityView = _entity.View;
            _entityAnimationTriggers = _entity.View.AnimationTriggers;
            _weaponAnimationTriggers = _weapon.View.GetComponent<WeaponAnimationTriggers>();
        }

        public virtual void Dispose()
        {
            _weapon = null;
            _entity = null;
            _entityView = null;
            _entityAnimationTriggers = null;
            _weaponAnimationTriggers = null;
        }
    }
}