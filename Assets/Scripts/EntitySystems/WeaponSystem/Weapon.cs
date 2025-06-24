using GeneralManagers;
using UnityEngine;

namespace EntitySystems.WeaponSystem
{
    public class Weapon : ILifecycle<WeaponSystem>
    {
        private WeaponView _view;
        public WeaponView View => _view;
        private WeaponSystem _parent;

        public void Initialize(WeaponSystem parent, WeaponView view)
        {
            _view = view;
            Initialize(parent);
        }

        public void Initialize(WeaponSystem parent)
        {
            _parent = parent;
            if (_view == null)
            {
                Debug.LogError("WeaponView is not assigned in Weapon.");
            }
            else
            {
                _view.Initialize(this);
            }
        }

        public void Enter()
        {
            _view.SetIsAttacking(true);
        }
        public void Update()
        {
            _view.SetAnimationDirection(_parent.Parent.Properties.lastMovementVector);
        }
        public void FixedUpdate()
        {
            //Fixed update weapon state if needed
        }
        
        public void Exit()
        {
            _view.SetIsAttacking(false);
        }


        public void Dispose()
        {
            _view = null;
            _parent = null;
        }
    }
}