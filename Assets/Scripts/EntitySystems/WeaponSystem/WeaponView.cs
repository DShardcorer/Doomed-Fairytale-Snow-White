using System;
using EntitySystems.WeaponSystem;
using GeneralManagers;
using Helpers;
using UnityEngine;

namespace EntitySystems.WeaponSystem
{
    public class WeaponView : MonoBehaviour, ILifecycle<WeaponSystem>
    {
        private WeaponSystem _parent;

        [SerializeField] private Animator _animator;
        public Animator Animator => _animator;
        private Vector2 _lastMovementVector;

        [SerializeField] private WeaponAnimationTriggers weaponAnimationTriggers;
        public WeaponAnimationTriggers WeaponAnimationTriggers => weaponAnimationTriggers;

        private WeaponDataSO weaponData;
        public WeaponDataSO WeaponData => weaponData;

        public void Initialize(WeaponSystem parent)
        {
            _parent = parent;
            if (_animator == null)
                _animator = GetComponent<Animator>();
            //log out the body type
            Debug.LogWarning(_parent.Parent.View.BodyType.ToString());
            RuntimeAnimatorController controller =  weaponData.GetBodyTypeAnimatorController(_parent.Parent.View.BodyType);
            if (controller == null)
            {
                Debug.LogError($"Animator Controller not found for body type: {_parent.Parent.View.BodyType}");
                return;
            }
            _animator.runtimeAnimatorController = controller;
               
            if (weaponAnimationTriggers == null)
                weaponAnimationTriggers = GetComponentInChildren<WeaponAnimationTriggers>();
        }
        public void Initialize(WeaponSystem parent, WeaponDataSO weaponData)
        {
            this.weaponData = weaponData;
            Initialize(parent);
        }





        public void Dispose()
        {
            _parent = null;
            weaponData = null;
        }

        public void SetIsAttacking(bool isAttacking)
        {
            _animator.SetBool(HelperAnimationStateName.IS_ATTACKING, isAttacking);
        }

        public virtual void SetAnimationDirection(Vector2 movement)
        {
            if (_lastMovementVector == movement)
                return;
            _lastMovementVector = movement;
            _animator.SetFloat(HelperAnimationStateName.MOVEMENT_X, movement.x);
            _animator.SetFloat(HelperAnimationStateName.MOVEMENT_Y, movement.y);
        }
    }
}