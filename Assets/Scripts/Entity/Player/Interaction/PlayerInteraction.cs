using System;
using UnityEngine;
using System.Collections;

public class PlayerInteraction : MonoBehaviour, ILifecycle<Player>
{
    private Player _player;
    private float interactRadius = 0.5f;
    [SerializeField] private LayerMask interactableLayer;
    public Transform interactPoint;

    [SerializeField] private GameObject arrowPrefab;
    private GameObject currentArrow;
    private Vector3 arrowOffset = new Vector3(0, 1f, 0);

    private Coroutine arrowCoroutine;

    public void Initialize(Player player)
    {
        _player = player;
        _player.InputManager.interactInputted += Interact;
    }
    public void Dispose()
    {
        _player.InputManager.interactInputted -= Interact;
        if (currentArrow != null)
        {
            Destroy(currentArrow);
        }
    }

    private void Interact(InputEventContext context)
    {
        if (context != InputEventContext.DEFAULT)
        {
            return;
        }
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

    private void OnEnable()
    {
        // Start the coroutine when the script is enabled
        arrowCoroutine = StartCoroutine(UpdateFloatingArrowRoutine());
    }

    private void OnDisable()
    {
        // Stop the coroutine if the script is disabled
        if (arrowCoroutine != null)
        {
            StopCoroutine(arrowCoroutine);
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

    // Coroutine that updates the floating arrow every 0.2 seconds
    private IEnumerator UpdateFloatingArrowRoutine()
    {
        while (true)
        {
            IInteractable closestInteractable = GetClosestInteractable();

            if (closestInteractable != null)
            {
                MonoBehaviour interactableMB = closestInteractable as MonoBehaviour;
                if (interactableMB != null)
                {
                    Transform interactableTransform = interactableMB.transform;

                    if (currentArrow == null)
                    {
                        currentArrow = Instantiate(arrowPrefab, interactableTransform.position + arrowOffset, Quaternion.identity);
                    }
                    else
                    {
                        // Smoothly update the position (optional, for a nice effect)
                        currentArrow.transform.position = Vector3.Lerp(currentArrow.transform.position,
                                                                       interactableTransform.position + arrowOffset,
                                                                       0.2f * 10f);
                    }
                }
            }
            else
            {
                // If no interactable is found, remove the arrow
                if (currentArrow != null)
                {
                    Destroy(currentArrow);
                }
            }

            yield return new WaitForSeconds(0.2f);
        }
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
