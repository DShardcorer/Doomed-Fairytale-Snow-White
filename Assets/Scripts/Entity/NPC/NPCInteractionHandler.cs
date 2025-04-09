using System.Linq;
using UnityEngine;

public class NPCInteractionHandler : MonoBehaviour, IInteractable
{
    private IInteractable[] _interactables;

    private void Awake()
    {
        // Find all IInteractable components on this NPC
        _interactables = GetComponents<IInteractable>()
            .Where(i => i != (IInteractable)this) // Avoid recursion
            .OrderByDescending(i => i.Priority)   // Highest priority first
            .ToArray();
    }

    public int Priority => int.MinValue; // Lowest to ensure it's only a dispatcher

    public void Interact(Player player)
    {
        foreach (var interactable in _interactables)
        {
            interactable.Interact(player);
            break; // Only call the highest-priority one
        }
    }
}
