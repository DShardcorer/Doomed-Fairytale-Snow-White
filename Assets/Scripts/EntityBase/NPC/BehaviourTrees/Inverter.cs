namespace EntityBase.NPC.BehaviourTrees
{
    public class Inverter: Node
    {
        public Inverter(string name, int priority = 0) : base(name, priority)
        {
        }

        public override Status Process()
        {
            switch (children[0].Process())
            {
                case Status.Running:
                    return Status.Running;
                case Status.Success:
                    return Status.Failure;
                case Status.Failure:
                    return Status.Success;
                default:
                    return Status.Failure; // Should not happen, but just in case
            }
        }
    }
}