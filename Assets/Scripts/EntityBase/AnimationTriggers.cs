using System;
using EntityBase.AttackCheck;
using GeneralManagers;
using UnityEngine;

namespace EntityBase
{
    public class AnimationTriggers : MonoBehaviour, ILifecycle<Entity>
    {
        private Entity _entity;
        private AttackHitbox _attackHitbox;
        private EntityStateMachine _stateMachine;

        public event Action OnTakingEffect;

        public void Dispose()
        {
            _entity = null;
        }

        public void Initialize(Entity parent)
        {
            _entity = parent;
            _attackHitbox = _entity.AttackHitbox;
            _stateMachine = _entity.StateMachine;
            //if null, throw error
            if (_attackHitbox == null)
            {
                Debug.LogError("AttackHitbox is not assigned!");
            }

            if (_stateMachine == null)
            {
                Debug.LogError("StateMachine is not assigned!");
            }
        }

        public void OnAnimationEnd()
        {
            _stateMachine.OnAnimationEnd();
        }

        public void PerformAttack()
        {
            OnTakingEffect?.Invoke();
        }

        public event Action OnAttackMovementStart;
        public event Action OnAttackMovementEnd;

        public void StartAttackMovement()
        {
            OnAttackMovementStart?.Invoke();
        }

        public void EndAttackMovement()
        {
            OnAttackMovementEnd?.Invoke();
        }
    }
}