using EntityBase.Player;

namespace GeneralInterfaces
{
    public interface IInteractable
    {
        int Priority { get; }
        /// <summary>
        /// Called when an interactor interacts with this object.
        /// </summary>
        /// <param name="player">The interacting player's controller.</param>
        void Interact(Player player);
    }
}
