using System.Collections;
using EntitySystems.WeaponSystem;
using GeneralManagers;
using Helpers;
using UI.General;
using UnityEngine;

namespace EntityBase
{
    public class EntityView : MonoBehaviour, ILifecycle<Entity>
    {
        protected Entity _parent;
        public Entity Parent => _parent;
        
        [Header("Components")]
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        [Header("Weapons")]
        [SerializeField] private WeaponView _primaryWeaponView;
        [SerializeField] private WeaponView _secondaryWeaponView;
        
        [Header("Visual Effects")]
        [SerializeField] private Material _originalMaterial;
        [SerializeField] private Material _flashOnHitMaterial;
        [SerializeField] private float _hitFlashDuration = 0.1f;
        
        [Header("UI")]
        [SerializeField] private EntityHealthbarUI _healthbarUI;

        // Properties
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public Animator Animator => _animator;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public Material OriginalMaterial => _originalMaterial;
        public Material FlashOnHitMaterial => _flashOnHitMaterial;
        public WeaponView PrimaryWeaponView => _primaryWeaponView;
        public WeaponView SecondaryWeaponView => _secondaryWeaponView;
        
        private Vector2 _lastMovementVector;

        public virtual void Initialize(Entity controller)
        {
            _parent = controller;
            
            if (_spriteRenderer == null)
                _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
                
            if (_rigidbody2D == null)
                _rigidbody2D = GetComponent<Rigidbody2D>();
                
            if (_healthbarUI == null)
                _healthbarUI = GetComponentInChildren<EntityHealthbarUI>();
                
            _healthbarUI.Initialize(this);
        }

        public void Dispose()
        {
            if (_healthbarUI != null)
            {
                _healthbarUI.Dispose();
            }
            
            Destroy(gameObject);
        }

        public virtual void Move(Vector2 movementVector)
        {
            _rigidbody2D.MovePosition(_rigidbody2D.position + movementVector);
        }

        public virtual void StartStateAnimation(string stateAnimation)
        {
            // StopStateAnimation(stateAnimation);
            _animator.SetBool(stateAnimation, true);
        }

        public virtual void StopStateAnimation(string stateAnimation)
        {
            _animator.SetBool(stateAnimation, false);
        }

        public virtual void SetAnimationDirection(Vector2 movement)
        {
            if(_lastMovementVector == movement)
                return;
            _lastMovementVector = movement;
            _animator.SetFloat(HelperAnimationStateName.MOVEMENT_X ,movement.x);
            _animator.SetFloat(HelperAnimationStateName.MOVEMENT_Y, movement.y);
        }
        public void SetAttackCounter(int attackCounter)
        {
            _animator.SetInteger(HelperAnimationStateName.ATTACK_COUNTER, attackCounter);
        }

        public void PlayDamagedAnimation()
        {
            if (_spriteRenderer != null && _flashOnHitMaterial != null)
            {
                StopCoroutine(nameof(ResetMaterialCoroutine));
                _spriteRenderer.material = _flashOnHitMaterial;
                StartCoroutine(ResetMaterialCoroutine());
            }
        }

        public void PlayDeathAnimation()
        {
            // _animator.SetTrigger(DEATH_TRIGGER);
            
        }

        private IEnumerator ResetMaterialCoroutine()
        {
            yield return new WaitForSeconds(_hitFlashDuration);
            
            if (_spriteRenderer != null && _originalMaterial != null)
                _spriteRenderer.material = _originalMaterial;
        }
    }
}