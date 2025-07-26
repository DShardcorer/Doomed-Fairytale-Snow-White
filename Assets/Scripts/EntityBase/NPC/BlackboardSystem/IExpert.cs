namespace EntityBase.NPC.BlackboardSystem
{
    public interface IExpert
    {
        int GetInsistence(Blackboard blackboard);

        void Execute(Blackboard blackboard);
    }
}