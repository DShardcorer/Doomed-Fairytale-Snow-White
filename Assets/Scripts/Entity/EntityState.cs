using UnityEngine;

public class EntityState
{
    protected Entity _controller;
    protected EntityStateMachine _stateMachine;
    protected EntityView _view;
    protected Rigidbody2D _rigidbody;
    protected Animator _animator;
    protected float _stateTimer = 1f;
    protected bool _isAnimationEnded;
    protected string _animationBoolName;

    public EntityState(string animationBoolName)
    {
        _animationBoolName = animationBoolName;
    }

    public virtual void Initialize(Entity controller)
    {
        _controller = controller;
        _stateMachine = _controller.StateMachine;
        _view = _controller.View;
        _rigidbody = _controller.View.Rigidbody2D;
        _animator = _controller.View.Animator;
    }
    public virtual void EnterState()
    {
        _isAnimationEnded = false;
        _view.StartStateAnimation(_animationBoolName);
    }
    public virtual void UpdateState()
    {
        _stateTimer -= Time.deltaTime;
    }
    public virtual void FixedUpdateState()
    {

    }

    public virtual void OnAnimationEnd()
    {
        _isAnimationEnded = true;
    }

    public virtual void ExitState()
    {
        _view.StopStateAnimation(_animationBoolName);
    }
    public bool IsAnimationEnded()
    {
        return _isAnimationEnded;
    }

}
