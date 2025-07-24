using System.Collections.Generic;
using System.Linq;

namespace EntityBase.NPC.BehaviourTrees
{
    public class RandomSelector: PrioritySelector
    {
        protected override List<Node> SortChildren() => children.Shuffle().ToList();
        public RandomSelector(string name, int priority = 0) : base(name, priority)
        {
        }
    }
}