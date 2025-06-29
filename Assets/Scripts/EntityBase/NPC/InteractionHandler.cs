using System.Linq;
using GeneralInterfaces;
using UnityEngine;

namespace EntityBase.NPC
{
    public class InteractionHandler : MonoBehaviour
    {
        private IInteractable[] _interactables;

        private void Start()
        {
            // Find all IInteractable components on this NPC
            _interactables = GetComponents<IInteractable>()
                .OrderByDescending(i => i.Priority) // Highest priority first
                .ToArray();
        }

        public int Priority => int.MinValue; // Lowest to ensure it's only a dispatcher


        public IInteractable GetHighestPriorityInteractable()
        {
            return _interactables.FirstOrDefault();
        }
        // public void Interact(Player player)
        // {
        //     foreach (var interactable in _interactables)
        //     {
        //         interactable.Interact(player);
        //         break; // Only call the highest-priority one
        //     }
        // }
    }
}