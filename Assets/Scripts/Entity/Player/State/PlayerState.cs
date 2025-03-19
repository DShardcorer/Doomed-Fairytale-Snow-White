

using UnityEngine;

public abstract class PlayerState : EntityState
{
    protected Player _player;

    protected InputManager _inputManager;

    protected PlayerState(string animationBoolName) : base(animationBoolName)
    {
    }

    public virtual void Initialize(Player controller)
    {
        _player = controller;
        _inputManager = _player.InputManager;
        if (_player == null)
        {
            Debug.LogError("Player is null");
        }
        if (_inputManager == null)
        {
            Debug.LogError("InputManager is null");
        }
        base.Initialize(controller);
    }


}
