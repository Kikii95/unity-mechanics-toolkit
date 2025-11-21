using UnityEngine;
using UnityEngine.Events;
using UnityMechanicsToolkit.Core;

namespace UnityMechanicsToolkit.Movement
{
    /// <summary>
    /// Sprint mechanic with optional stamina integration.
    /// Increases movement speed while draining stamina.
    /// </summary>
    public class SprintBehavior : MonoBehaviour
    {
        [Header("Sprint Settings")]
        [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
        [SerializeField] private float sprintSpeedMultiplier = 1.5f;
        [SerializeField] private bool canSprintBackwards = false;

        [Header("Stamina Integration (Optional)")]
        [SerializeField] private bool useStamina = true;
        [SerializeField] private float staminaDrainRate = 15f; // Per second
        [SerializeField] private float minStaminaToSprint = 10f;

        [Header("FOV Effect (Optional)")]
        [SerializeField] private bool changeFOV = true;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float sprintFOV = 75f;
        [SerializeField] private float normalFOV = 60f;
        [SerializeField] private float fovTransitionSpeed = 5f;

        [Header("Events")]
        public UnityEvent onSprintStart;
        public UnityEvent onSprintEnd;

        private StaminaSystem stamina;
        private bool isSprinting;
        private float currentFOV;

        public bool IsSprinting => isSprinting;
        public float SpeedMultiplier => isSprinting ? sprintSpeedMultiplier : 1f;

        private void Awake()
        {
            if (useStamina)
            {
                stamina = GetComponent<StaminaSystem>();
            }

            if (playerCamera == null)
            {
                playerCamera = Camera.main;
            }

            if (playerCamera != null)
            {
                currentFOV = playerCamera.fieldOfView;
                normalFOV = currentFOV;
            }
        }

        private void Update()
        {
            HandleSprint();
            UpdateFOV();
        }

        private void HandleSprint()
        {
            bool wantsToSprint = Input.GetKey(sprintKey);
            bool isMovingForward = CheckMovementDirection();

            if (wantsToSprint && isMovingForward && CanSprint())
            {
                if (!isSprinting)
                {
                    StartSprint();
                }

                // Drain stamina
                if (useStamina && stamina != null)
                {
                    stamina.Consume(staminaDrainRate * Time.deltaTime);

                    // Stop if out of stamina
                    if (stamina.CurrentStamina < minStaminaToSprint)
                    {
                        StopSprint();
                    }
                }

                // Emit event for footsteps (louder)
                GameEvents.PlayerSprint(true);
            }
            else
            {
                if (isSprinting)
                {
                    StopSprint();
                }
            }
        }

        private bool CanSprint()
        {
            // Check stamina
            if (useStamina && stamina != null)
            {
                if (stamina.CurrentStamina < minStaminaToSprint)
                {
                    return false;
                }
            }

            // Check crouch (can't sprint while crouched)
            CrouchBehavior crouch = GetComponent<CrouchBehavior>();
            if (crouch != null && crouch.IsCrouching)
            {
                return false;
            }

            return true;
        }

        private bool CheckMovementDirection()
        {
            float vertical = Input.GetAxis("Vertical");

            if (canSprintBackwards)
            {
                return Mathf.Abs(vertical) > 0.1f; // Any direction
            }
            else
            {
                return vertical > 0.1f; // Forward only
            }
        }

        private void StartSprint()
        {
            isSprinting = true;
            onSprintStart?.Invoke();
        }

        private void StopSprint()
        {
            isSprinting = false;
            onSprintEnd?.Invoke();
            GameEvents.PlayerSprint(false);
        }

        private void UpdateFOV()
        {
            if (!changeFOV || playerCamera == null) return;

            float targetFOV = isSprinting ? sprintFOV : normalFOV;
            currentFOV = Mathf.Lerp(currentFOV, targetFOV, Time.deltaTime * fovTransitionSpeed);
            playerCamera.fieldOfView = currentFOV;
        }
    }
}
