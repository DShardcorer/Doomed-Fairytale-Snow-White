namespace EntityBase.NPC.BehaviourTrees
{
    //Logical OR operation for child nodes
    public class Selector: Node
    {
        public Selector(string name, int priority = 0) : base(name, priority)
        {
        }

        public override Status Process()
        {
            if(currentChildIndex< children.Count)
            {
                switch (children[currentChildIndex].Process())
                {
                    case Status.Running:
                        return Status.Running;
                    case Status.Success:
                        Reset();
                        return Status.Success;
                    default: // Status.Failure
                        currentChildIndex++;
                        return Status.Running;
                }
            }
            Reset();
            return Status.Failure;
        }
    }
}