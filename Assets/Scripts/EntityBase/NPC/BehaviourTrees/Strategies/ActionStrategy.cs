using System;

namespace EntityBase.NPC.BehaviourTrees.Strategies
{
    public class ActionStrategy:IStrategy
    {
        readonly Action doSomething;

        public ActionStrategy(Action doSomething)
        {
            this.doSomething = doSomething;
        }
        
        public Node.Status Process()
        {
            doSomething.Invoke();
            return Node.Status.Success; // Assuming the action always succeeds
        }
    }
}