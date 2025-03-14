using UnityEngine;

public class EntityView : MonoBehaviour, ILifecycle<Entity>
{
    protected Entity _controller;
    public Entity Controller => _controller;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Animator _animator;
    public Rigidbody2D Rigidbody2D => _rigidbody2D;
    public Animator Animator => _animator;


    private const string MOVEMENT_X = "MovementX";
    private const string MOVEMENT_Y = "MovementY";
    public virtual void Initialize(Entity controller)
    {
        _controller = controller;
    }


    public virtual void Move(Vector2 movementVector)
    {

        _rigidbody2D.MovePosition(_rigidbody2D.position + movementVector);
    }


    public virtual void StartStateAnimation(string stateAnimation)
    {
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
}
