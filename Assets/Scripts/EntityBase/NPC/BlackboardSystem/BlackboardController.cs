using System;
using GeneralManagers;
using UnityEngine;

namespace EntityBase.NPC.BlackboardSystem
{
    public class BlackboardController: ILifecycle<NPC>
    {
        private Blackboard _blackboard = new Blackboard();
        public Blackboard BlackBoard => _blackboard;
        private Arbiter _arbiter = new Arbiter();
        public Arbiter Arbiter => _arbiter;
        
        public void RegisterExpert(IExpert expert) => _arbiter.RegisterExpert(expert);
        
        
        public void Initialize(NPC parent)
        {
            
        }

        public void Dispose()
        {
        }

        private void UpdateLogic()
        {
            foreach (Action action in _arbiter.BlackboardIteration(_blackboard))
            {
                action();
            }
        }
    }
}