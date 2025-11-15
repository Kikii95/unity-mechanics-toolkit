using UnityEngine;

namespace UnityMechanicsToolkit.Interaction
{
    public class RaycastInteractor : MonoBehaviour
    {
        [Header("Raycast Settings")]
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float interactionRange = 3f;
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private KeyCode interactKey = KeyCode.E;

        [Header("UI Feedback")]
        [SerializeField] private bool showDebugRay = true;

        private InteractableObject currentInteractable;

        private void Awake()
        {
            if (playerCamera == null)
            {
                playerCamera = Camera.main;
            }
        }

        private void Update()
        {
            CheckForInteractable();
            HandleInteraction();
        }

        private void CheckForInteractable()
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            if (showDebugRay)
            {
                Debug.DrawRay(ray.origin, ray.direction * interactionRange, Color.yellow);
            }

            if (Physics.Raycast(ray, out RaycastHit hit, interactionRange, interactableLayer))
            {
                InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();

                if (interactable != null && interactable.IsInteractable)
                {
                    if (currentInteractable != interactable)
                    {
                        if (currentInteractable != null)
                        {
                            currentInteractable.OnHoverExit(gameObject);
                        }

                        currentInteractable = interactable;
                        currentInteractable.OnHoverEnter(gameObject);
                    }
                    return;
                }
            }

            if (currentInteractable != null)
            {
                currentInteractable.OnHoverExit(gameObject);
                currentInteractable = null;
            }
        }

        private void HandleInteraction()
        {
            if (Input.GetKeyDown(interactKey) && currentInteractable != null)
            {
                currentInteractable.OnInteract(gameObject);
            }
        }

        public InteractableObject GetCurrentInteractable()
        {
            return currentInteractable;
        }
    }
}
