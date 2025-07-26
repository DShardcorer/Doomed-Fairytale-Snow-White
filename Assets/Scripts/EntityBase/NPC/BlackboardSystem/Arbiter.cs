using System;
using System.Collections.Generic;

namespace EntityBase.NPC.BlackboardSystem
{
    public class Arbiter
    {
        private readonly List<IExpert> experts = new();

        public void RegisterExpert(IExpert expert)
        {
            if (expert == null) return;
            experts.Add(expert);
        }

        public List<Action> BlackboardIteration(Blackboard blackboard)
        {
            IExpert bestExpert = null;
            int highestInsistence = 0;

            foreach (IExpert expert in experts)
            {
                int insistence = expert.GetInsistence(blackboard);
                if (insistence > highestInsistence)
                {
                    highestInsistence = insistence;
                    bestExpert = expert;
                }
            }

            bestExpert.Execute(blackboard);

            var actions = new List<Action>(blackboard.PassedActions);
            blackboard.ClearActions();
            return actions;
        }
    }
}