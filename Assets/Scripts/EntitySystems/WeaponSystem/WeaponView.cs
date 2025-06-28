using System;
using EntitySystems.WeaponSystem;
using GeneralManagers;
using Helpers;
using UnityEngine;

namespace EntitySystems.WeaponSystem
{
    public class WeaponView : MonoBehaviour, ILifecycle<Weapon>
    {
        private Weapon _parent;

        [SerializeField] private Animator _animator;
        public Animator Animator => _animator;
        private Vector2 _lastMovementVector;

        [SerializeField] private SpriteRenderer weaponSpriteRenderer;
        public SpriteRenderer WeaponSpriteRenderer => weaponSpriteRenderer;
        
        [SerializeField] private WeaponAnimationTriggers weaponAnimationTriggers;
        public WeaponAnimationTriggers WeaponAnimationTriggers => weaponAnimationTriggers;
        
        [SerializeField] private WeaponDataSO weaponData;
        public WeaponDataSO WeaponData => weaponData;


        public void Initialize(Weapon parent)
        {
            _parent = parent;
            if (_animator == null)
                _animator = GetComponent<Animator>();
            _animator.runtimeAnimatorController = weaponData.WeaponAnimatorController;
            if(weaponAnimationTriggers == null)
                weaponAnimationTriggers = GetComponentInChildren<WeaponAnimationTriggers>();
        }

        public void Dispose()
        {
            _parent = null;
            _animator = null;
        }

        public void SetIsAttacking(bool isAttacking)
        {
            _animator.SetBool(HelperAnimationStateName.IS_ATTACKING, isAttacking);
        }

        public virtual void SetAnimationDirection(Vector2 movement)
        {
            Debug.Log("Set Animation Direction: " + movement);
            if (_lastMovementVector == movement)
                return;
            _lastMovementVector = movement;
            _animator.SetFloat(HelperAnimationStateName.MOVEMENT_X, movement.x);
            _animator.SetFloat(HelperAnimationStateName.MOVEMENT_Y, movement.y);
        }

        private void OnDrawGizmosSelected()
        {
            
        }
    }
}