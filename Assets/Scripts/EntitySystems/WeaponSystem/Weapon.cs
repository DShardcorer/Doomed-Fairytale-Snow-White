using System.Collections.Generic;
using EntitySystems.WeaponSystem.Components;
using GeneralManagers;
using UnityEngine;

namespace EntitySystems.WeaponSystem
{
    public class Weapon : ILifecycle<WeaponSystem>
    {
        private WeaponView _view;
        public WeaponView View => _view;
        private WeaponSystem _parent;
        public WeaponSystem Parent => _parent;
        private List<WeaponComponent> _components = new List<WeaponComponent>();
        public List<WeaponComponent> Components => _components;
        public WeaponDataSO WeaponData => _view.WeaponData;
        public bool IsActive { get; private set; }

        public void Initialize(WeaponSystem parent, WeaponView view)
        {
            _view = view;
            Initialize(parent);
        }

        public void Initialize(WeaponSystem parent)
        {
            _parent = parent;
            _view.Initialize(this);
            _components.Clear();
            foreach (WeaponComponentData weaponComponentData in WeaponData.ComponentDataList)
            {
                if (weaponComponentData.DependencyType.IsSubclassOf(typeof(WeaponComponent)))
                {
                    WeaponComponent component =
                        (WeaponComponent)System.Activator.CreateInstance(weaponComponentData.DependencyType);
                    _components.Add(component);
                }
                else
                {
                    Debug.LogWarning(
                        $"Weapon Component Data {weaponComponentData.GetType().Name} does not have a valid DependencyType.");
                }
            }


            foreach (WeaponComponent component in _components)
            {
                component.Initialize(this);
            }
        }

        public void SetAsActiveWeapon()
        {
            if (!IsActive)
            {
                IsActive = true;
            }
        }

        public void SetAsInactiveWeapon()
        {
            if (IsActive)
            {
                IsActive = false;
            }
        }

        public void Enter()
        {
            _view.SetIsAttacking(true);
            _parent.SetActiveWeapon(this);
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
            _parent = null;
            foreach (WeaponComponent component in _components)
            {
                component.Dispose();
            }

            _components.Clear();
            _view = null;
        }
    }
}