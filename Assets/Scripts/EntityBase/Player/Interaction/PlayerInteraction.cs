using System.Collections;
using EntityBase.NPC;
using EventBus.Dialogue;
using EventBus.Player;
using GeneralManagers;
using Input;
using InteractInterface;
using UnityEngine;

namespace EntityBase.Player.Interaction
{
    public class PlayerInteraction : MonoBehaviour, ILifecycle<Player>
    {
        private Player player;
        private float interactRadius = 1f;
        [SerializeField] private LayerMask interactableLayer;
        public Transform interactPoint;

        [SerializeField] private GameObject arrowPrefab;
        private GameObject currentArrow;
        private Vector3 arrowOffset = new Vector3(0, 2f, 0);

        private Coroutine arrowCoroutine;
        private IInteractable currentInteractable;
        private InteractionHandler currentInteractionHandler;

        public IInteractable CurrentInteractable => currentInteractable;

        public void Initialize(Player player)
        {
            this.player = player;
            this.player.InputManager.interactInputted += Interact;
            DialogueEventSystem.OnExitDialogue += OnExitDialogue;
        }

        private void OnExitDialogue()
        {
            PlayerInteractEventSystem.InvokeExitInteraction(
                new PlayerInteractEventSystem.ExitInteractionEventArgs(currentInteractable, player));
        }

        public void Dispose()
        {
            player.InputManager.interactInputted -= Interact;
            DialogueEventSystem.OnExitDialogue -= OnExitDialogue;

            if (currentArrow != null)
            {
                Destroy(currentArrow);
            }
        }

        private void Interact(InputEventContext context)
        {
            if (context != InputEventContext.DEFAULT)
                return;

            var (closestInteractable, _) = GetClosestInteractableWithHandler();
            if (closestInteractable != null)
            {
                currentInteractable = closestInteractable;
                closestInteractable.Interact(player);
                PlayerInteractEventSystem.InvokeEnterInteraction(
                    new PlayerInteractEventSystem.EnterInteractionEventArgs(currentInteractable, player));
            }
            else
            {
                Debug.Log("No interactable object in range.");
            }
        }

        private void OnEnable()
        {
            arrowCoroutine = StartCoroutine(UpdateFloatingArrowRoutine());
        }

        private void OnDisable()
        {
            if (arrowCoroutine != null)
            {
                StopCoroutine(arrowCoroutine);
            }
        }

        private (IInteractable, InteractionHandler) GetClosestInteractableWithHandler()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(interactPoint.position, interactRadius, interactableLayer);
            if (colliders.Length == 0)
            {
                return (null, null);
            }

            IInteractable closestInteractable = null;
            InteractionHandler closestHandler = null;
            float closestDistance = Mathf.Infinity;

            foreach (Collider2D collider in colliders)
            {
                var handler = collider.GetComponent<InteractionHandler>();
                if (handler == null) continue;

                IInteractable interactable = handler.GetHighestPriorityInteractable();
                if (interactable != null)
                {
                    float distance = Vector2.Distance(transform.position, collider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestInteractable = interactable;
                        closestHandler = handler;
                    }
                }
            }

            return (closestInteractable, closestHandler);
        }

        private IEnumerator UpdateFloatingArrowRoutine()
        {
            while (true)
            {
                var (closestInteractable, handler) = GetClosestInteractableWithHandler();

                if (handler != null)
                {
                    Transform handlerTransform = handler.transform;

                    if (currentArrow == null)
                    {
                        currentArrow = Instantiate(arrowPrefab, handlerTransform.position + arrowOffset, Quaternion.identity);
                    }
                    else
                    {
                        currentArrow.transform.position = Vector3.Lerp(currentArrow.transform.position,
                            handlerTransform.position + arrowOffset,
                            0.2f * 10f);
                    }

                    currentInteractionHandler = handler;
                }
                else
                {
                    if (currentArrow != null)
                    {
                        Destroy(currentArrow);
                        currentInteractionHandler = null;
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
}
