using GeneralManagers;
using UnityEngine;

namespace EntityBase.NPC.AI
{
    public class NPCSubAIStateMachine: ILifecycle<NPCSubAIController>
    {
        private NPCSubAIController _currentNPCSubAIController;
        public NPCSubAIController CurrentNpcSubAIController => _currentNPCSubAIController;
        

        public void Initialize(NPCSubAIController initialState)
        {
            _currentNPCSubAIController = initialState;
            _currentNPCSubAIController.OnEnter();
        }

        public void Dispose()
        {
            _currentNPCSubAIController = null;
        }

        public void ChangeNPCSubAIController(NPCSubAIController newState)
        {
            if (newState == null)
            {
                Debug.LogError("New state is null");
                return;
            }

            _currentNPCSubAIController.OnExit();
            _currentNPCSubAIController = newState;
            _currentNPCSubAIController.OnEnter();
        }



        public void UpdateLogic()
        {
            if (_currentNPCSubAIController == null)
            {
                Debug.LogError("Current state is null");
                return;
            }

            _currentNPCSubAIController.UpdateLogic();
        }

        public void FixedUpdateLogic()
        {
            if (_currentNPCSubAIController == null)
            {
                Debug.LogError("Current state is null");
                return;
            }

            _currentNPCSubAIController.FixedUpdateLogic();
        }

        
    }
}