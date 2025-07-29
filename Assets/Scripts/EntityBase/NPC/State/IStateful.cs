using EntityBase.NPC.BehaviourTrees;

namespace EntityBase.NPC.State
{
    /// <summary>
    /// Interface for states that can report their execution status
    /// </summary>
    public interface IStateful
    {
        /// <summary>
        /// Gets the current execution status of the state
        /// </summary>
        Node.Status GetStatus();
        
        /// <summary>
        /// Whether the state is currently executing
        /// </summary>
        bool IsRunning { get; }
        
        /// <summary>
        /// Whether the state has completed successfully
        /// </summary>
        bool HasSucceeded { get; }
        
        /// <summary>
        /// Whether the state has failed
        /// </summary>
        bool HasFailed { get; }
    }
}