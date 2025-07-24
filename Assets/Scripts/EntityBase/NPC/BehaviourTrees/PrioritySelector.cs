using System.Collections.Generic;
using System.Linq;

namespace EntityBase.NPC.BehaviourTrees
{
    public class PrioritySelector : Selector
    {
        List<Node> sortedChildren;
        private List<Node> SortedChildren => sortedChildren ?? SortChildren();

        public PrioritySelector(string name, int priority = 0) : base(name, priority)
        {
        }


        protected virtual List<Node> SortChildren()
        {
            return children.OrderByDescending(child => child.priority).ToList();
        }

        public override void Reset()
        {
            base.Reset();
            sortedChildren = null; // Reset the sorted children to force re-sorting next time
        }


        public override Status Process()
        {
            foreach (var child in SortedChildren)
            {
                switch (child.Process())
                {
                    case Status.Running:
                        return Status.Running;
                    case Status.Success:
                        Reset();
                        return Status.Success;
                    case Status.Failure:
                        // Continue to the next child
                        continue;
                    default:
                        // Handle unexpected status if necessary
                        continue;
                }
            }

            return Status.Failure;
        }
    }
}