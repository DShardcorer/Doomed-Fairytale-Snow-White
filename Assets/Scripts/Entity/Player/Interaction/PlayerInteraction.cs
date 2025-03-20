using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Player _player;
    private float interactRadius = 0.5f;
    [SerializeField] private LayerMask interactableLayer;
    public Transform interactPoint;

    public void Initialize(Player player)
    {
        _player = player;
        _player.InputManager.interactInputted += Interact;
    }

    private void Interact(object sender, EventArgs e)
    {
        IInteractable closestInteractable = GetClosestInteractable();
        if (closestInteractable != null)
        {
            closestInteractable.Interact(_player);
        }
        else
        {
            Debug.Log("No interactable object in range.");
        }
    }
    private IInteractable GetClosestInteractable()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(interactPoint.position, interactRadius, interactableLayer);
        if (colliders.Length == 0)
        {
            return null;
        }
        IInteractable closestInteractable = null;
        float closestDistance = Mathf.Infinity;
        foreach (Collider2D collider in colliders)
        {
            IInteractable interactable = collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = interactable;
                }
            }
        }
        return closestInteractable;
    }

    public void SetInteractRotation(Vector2 direction)
    {
        transform.localRotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.down, direction));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactPoint.position, interactRadius);
    }








}
