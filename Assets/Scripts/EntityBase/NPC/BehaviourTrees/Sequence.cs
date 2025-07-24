namespace EntityBase.NPC.BehaviourTrees
{
    //Basically a logical AND operation for child nodes.
    public class Sequence : Node
    {
        public Sequence(string name, int priority = 0) : base(name, priority)
        {
        }

        public override Status Process()
        {
            if (currentChildIndex < children.Count)
            {
                switch (children[currentChildIndex].Process())
                {
                    case Status.Running:
                        return Status.Running;
                    case Status.Failure:
                        children[currentChildIndex].Reset();
                        return Status.Failure;
                    default: // Status.Success
                        currentChildIndex++;
                        return currentChildIndex == children.Count ? Status.Success : Status.Running;
                }
            }

            Reset();
            return Status.Success;
        }
    }
}