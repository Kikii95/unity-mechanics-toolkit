using UnityEngine;
using UnityEngine.Events;

namespace UnityMechanicsToolkit.Interaction
{
    /// <summary>
    /// Hiding spot mechanic for closets, beds, lockers.
    /// Player enters to hide from enemies.
    /// </summary>
    public class HidingSpot : InteractableObject
    {
        [Header("Hiding Settings")]
        [SerializeField] private Transform hidingPosition; // Where player stands when hiding
        [SerializeField] private bool hidePlayerModel = true;
        [SerializeField] private LayerMask hiddenLayer; // Layer to switch player to when hidden

        [Header("Camera")]
        [SerializeField] private bool restrictCamera = true;
        [SerializeField] private float maxCameraAngle = 45f;

        [Header("Events")]
        public UnityEvent<GameObject> onPlayerHidden;
        public UnityEvent<GameObject> onPlayerExited;

        private GameObject hiddenPlayer;
        private Vector3 playerOriginalPosition;
        private int playerOriginalLayer;
        private bool isOccupied;

        public bool IsOccupied => isOccupied;

        public override void OnInteract(GameObject interactor)
        {
            if (!isInteractable) return;

            if (isOccupied)
            {
                // Exit hiding spot
                ExitHidingSpot(interactor);
            }
            else
            {
                // Enter hiding spot
                EnterHidingSpot(interactor);
            }

            base.OnInteract(interactor);
        }

        private void EnterHidingSpot(GameObject player)
        {
            hiddenPlayer = player;
            isOccupied = true;

            // Store original position/layer
            playerOriginalPosition = player.transform.position;
            playerOriginalLayer = player.layer;

            // Move player to hiding position
            if (hidingPosition != null)
            {
                player.transform.position = hidingPosition.position;
                player.transform.rotation = hidingPosition.rotation;
            }

            // Hide player model (optional)
            if (hidePlayerModel)
            {
                Renderer[] renderers = player.GetComponentsInChildren<Renderer>();
                foreach (Renderer r in renderers)
                {
                    r.enabled = false;
                }
            }

            // Change layer (AI won't detect)
            player.layer = LayerMask.NameToLayer("Hidden");

            // Disable movement (optional - could keep FirstPersonController reference)
            var controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false;
            }

            onPlayerHidden?.Invoke(player);
            Debug.Log("[HidingSpot] Player entered hiding spot");
        }

        private void ExitHidingSpot(GameObject player)
        {
            if (player != hiddenPlayer) return;

            isOccupied = false;

            // Restore player visibility
            if (hidePlayerModel)
            {
                Renderer[] renderers = player.GetComponentsInChildren<Renderer>();
                foreach (Renderer r in renderers)
                {
                    r.enabled = true;
                }
            }

            // Restore layer
            player.layer = playerOriginalLayer;

            // Re-enable movement
            var controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = true;
            }

            // Move player out (slightly forward)
            player.transform.position = transform.position + transform.forward * 1.5f;

            onPlayerExited?.Invoke(player);
            hiddenPlayer = null;

            Debug.Log("[HidingSpot] Player exited hiding spot");
        }

        public override string GetInteractionPrompt()
        {
            return isOccupied ? "Press E to exit" : "Press E to hide";
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();

            if (hidingPosition != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(hidingPosition.position, 0.5f);
                Gizmos.DrawRay(hidingPosition.position, hidingPosition.forward * 2f);
            }
        }
#endif
    }
}
