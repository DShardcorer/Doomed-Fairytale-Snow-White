using System.Collections;
using GeneralManagers;
using UnityEngine;

namespace Entity
{
    public class EntityView : MonoBehaviour, ILifecycle<Entity>
    {
        protected Entity _controller;
        public Entity Controller => _controller;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private Animator _animator;
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public Animator Animator => _animator;

        private SpriteRenderer _spriteRenderer;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        [SerializeField] private Material _originalMaterial;
        public Material OriginalMaterial => _originalMaterial;
        [SerializeField] private Material _flashOnHitMaterial;
        public Material FlashOnHitMaterial => _flashOnHitMaterial;





        private const string MOVEMENT_X = "MovementX";
        private const string MOVEMENT_Y = "MovementY";
        public virtual void Initialize(Entity controller)
        {

            _controller = controller;
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }


        public virtual void Move(Vector2 movementVector)
        {

            _rigidbody2D.MovePosition(_rigidbody2D.position + movementVector);
        }


        public virtual void StartStateAnimation(string stateAnimation)
        {
            StopStateAnimation(stateAnimation);
            _animator.SetBool(stateAnimation, true);
        }
        public virtual void StopStateAnimation(string stateAnimation)
        {
            _animator.SetBool(stateAnimation, false);
        }

        public virtual void SetAnimationDirection(Vector2 movement)
        {
            _animator.SetFloat(MOVEMENT_X, movement.x);
            _animator.SetFloat(MOVEMENT_Y, movement.y);
        }

        public void Dispose()
        {
            _controller = null;
        }
        public void Damaged()
        {
            _spriteRenderer.material = _flashOnHitMaterial;
            Invoke("ResetMaterial", 0.1f);
        }

        public void PlayDamagedAnimation()
        {
            //switch material to flash on hit material
            _spriteRenderer.material = _flashOnHitMaterial;
            //reset material after 0.1 seconds
            StartCoroutine(ResetMaterial());

        }
        public void PlayDeathAnimation()
        {
            // _animator.SetTrigger("Death");
        }
        private IEnumerator ResetMaterial()
        {
            yield return new WaitForSeconds(0.1f);
            _spriteRenderer.material = _originalMaterial;
        }
    }
}
