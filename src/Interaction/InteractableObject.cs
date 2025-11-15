using UnityEngine;
using UnityEngine.Events;

namespace UnityMechanicsToolkit.Interaction
{
    public abstract class InteractableObject : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [SerializeField] protected string interactionPrompt = "Press E to interact";
        [SerializeField] protected float interactionDistance = 3f;
        [SerializeField] protected bool isInteractable = true;

        [Header("Events")]
        public UnityEvent onInteract;
        public UnityEvent onHoverEnter;
        public UnityEvent onHoverExit;

        protected bool isHovered;

        public string InteractionPrompt => interactionPrompt;
        public bool IsInteractable => isInteractable;

        public virtual void OnInteract(GameObject interactor)
        {
            if (!isInteractable) return;

            onInteract?.Invoke();
        }

        public virtual void OnHoverEnter(GameObject interactor)
        {
            isHovered = true;
            onHoverEnter?.Invoke();
        }

        public virtual void OnHoverExit(GameObject interactor)
        {
            isHovered = false;
            onHoverExit?.Invoke();
        }

        public void SetInteractable(bool state)
        {
            isInteractable = state;
        }

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = isInteractable ? Color.green : Color.red;
            Gizmos.DrawWireSphere(transform.position, interactionDistance);
        }
    }
}
