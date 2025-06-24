

using Input;
using UnityEngine;

namespace EntityBase.Player.State
{
    public abstract class PlayerState : EntityState
    {
        protected Player _player;

        protected InputManager _inputManager;

        protected PlayerState(string animationBoolName, EntityStateProperties entityStateProperties) : base(animationBoolName, entityStateProperties)
        {
        }

        public virtual void Initialize(Player parent)
        {
            _player = parent;
            _inputManager = _player.InputManager;
            if (_player == null)
            {
                Debug.LogError("Player is null");
            }
            if (_inputManager == null)
            {
                Debug.LogError("InputManager is null");
            }
            base.Initialize(parent);
        }


    }
}
