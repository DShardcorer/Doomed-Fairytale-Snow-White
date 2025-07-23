namespace EntityBase.NPC.BehaviourTrees
{
    public interface IStrategy
    {
        Node.Status Process();

        void Reset()
        {
            //No operation by default
        }
    }
}