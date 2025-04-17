using UnityEngine;

namespace Entity
{
    public class EntityStateMachine
    {
        private EntityState _currentState;
        public EntityState CurrentState => _currentState;

        public EntityStateMachine()
        {
        }

        public void Initialize(EntityState initialState)
        {
            _currentState = initialState;
            _currentState.EnterState();
        }

        public void ChangeState(EntityState newState)
        {
            if(newState == null)
            {
                Debug.LogError("New state is null");
                return;
            }
            _currentState.ExitState();
            _currentState = newState;
            _currentState.EnterState();
        }



        public void UpdateLogic()
        {
            if (_currentState == null)
            {
                Debug.LogError("Current state is null");
                return;
            }
            _currentState.UpdateState();
        }

        public void FixedUpdateLogic()
        {
            if (_currentState == null)
            {
                Debug.LogError("Current state is null");
                return;
            }
            _currentState.FixedUpdateState();
        }


        public void OnAnimationEnd()
        {
            _currentState.OnAnimationEnd();
        }

        public bool IsAnimationEnded()
        {
            return _currentState.IsAnimationEnded();
        }
    }
}
