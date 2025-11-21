using UnityEngine;
using UnityEngine.Events;

namespace UnityMechanicsToolkit.Movement
{
    /// <summary>
    /// Crouch mechanic for stealth and hiding under objects.
    /// Modifies CharacterController height and movement speed.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class CrouchBehavior : MonoBehaviour
    {
        [Header("Crouch Settings")]
        [SerializeField] private KeyCode crouchKey = KeyCode.C;
        [SerializeField] private bool holdToCrouch = false; // Toggle or hold
        [SerializeField] private float crouchHeight = 1f;
        [SerializeField] private float standingHeight = 2f;
        [SerializeField] private float crouchSpeed = 2f; // Transition speed
        [SerializeField] private float crouchSpeedMultiplier = 0.5f; // Movement speed when crouched

        [Header("Camera")]
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float cameraStandingY = 0.8f;
        [SerializeField] private float cameraCrouchY = 0.3f;

        [Header("Events")]
        public UnityEvent onCrouchStart;
        public UnityEvent onCrouchEnd;

        private CharacterController controller;
        private bool isCrouching;
        private float targetHeight;
        private float targetCameraY;

        public bool IsCrouching => isCrouching;
        public float SpeedMultiplier => isCrouching ? crouchSpeedMultiplier : 1f;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            targetHeight = standingHeight;
            targetCameraY = cameraStandingY;

            if (cameraTransform == null)
            {
                cameraTransform = Camera.main?.transform;
            }
        }

        private void Update()
        {
            HandleInput();
            UpdateCrouchTransition();
        }

        private void HandleInput()
        {
            if (holdToCrouch)
            {
                // Hold to crouch
                if (Input.GetKeyDown(crouchKey))
                {
                    StartCrouch();
                }
                else if (Input.GetKeyUp(crouchKey))
                {
                    StopCrouch();
                }
            }
            else
            {
                // Toggle crouch
                if (Input.GetKeyDown(crouchKey))
                {
                    if (isCrouching)
                    {
                        StopCrouch();
                    }
                    else
                    {
                        StartCrouch();
                    }
                }
            }
        }

        public void StartCrouch()
        {
            if (isCrouching) return;

            isCrouching = true;
            targetHeight = crouchHeight;
            targetCameraY = cameraCrouchY;

            onCrouchStart?.Invoke();
        }

        public void StopCrouch()
        {
            if (!isCrouching) return;

            // Check if enough space to stand
            if (!CanStand())
            {
                return; // Blocked, stay crouched
            }

            isCrouching = false;
            targetHeight = standingHeight;
            targetCameraY = cameraStandingY;

            onCrouchEnd?.Invoke();
        }

        private bool CanStand()
        {
            // Raycast upward to check if space available
            float checkDistance = standingHeight - crouchHeight + 0.1f;
            Vector3 origin = transform.position + Vector3.up * crouchHeight;

            return !Physics.Raycast(origin, Vector3.up, checkDistance, ~0, QueryTriggerInteraction.Ignore);
        }

        private void UpdateCrouchTransition()
        {
            // Smooth height transition
            float currentHeight = controller.height;
            if (!Mathf.Approximately(currentHeight, targetHeight))
            {
                controller.height = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * crouchSpeed);
                controller.center = new Vector3(0, controller.height / 2f, 0);
            }

            // Smooth camera transition
            if (cameraTransform != null)
            {
                Vector3 localPos = cameraTransform.localPosition;
                if (!Mathf.Approximately(localPos.y, targetCameraY))
                {
                    localPos.y = Mathf.Lerp(localPos.y, targetCameraY, Time.deltaTime * crouchSpeed);
                    cameraTransform.localPosition = localPos;
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (controller == null) return;

            Gizmos.color = isCrouching ? Color.yellow : Color.green;
            Gizmos.DrawWireCube(transform.position + Vector3.up * controller.height / 2f,
                new Vector3(controller.radius * 2f, controller.height, controller.radius * 2f));
        }
#endif
    }
}
