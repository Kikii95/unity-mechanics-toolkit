using UnityEngine;

namespace UnityMechanicsToolkit.Camera
{
    /// <summary>
    /// Head bobbing effect for walk/run movement.
    /// Adds vertical/horizontal oscillation to camera based on movement speed.
    /// </summary>
    public class HeadBobbing : MonoBehaviour
    {
        [Header("Bobbing Settings")]
        [SerializeField] private bool enableBobbing = true;
        [SerializeField] private float walkBobbingSpeed = 14f;
        [SerializeField] private float walkBobbingAmount = 0.05f;
        [SerializeField] private float sprintBobbingSpeed = 18f;
        [SerializeField] private float sprintBobbingAmount = 0.1f;

        [Header("Horizontal Bobbing")]
        [SerializeField] private bool enableHorizontalBob = true;
        [SerializeField] private float horizontalBobbingAmount = 0.01f;

        private float timer;
        private Vector3 originalPosition;
        private bool isMoving;

        private void Awake()
        {
            originalPosition = transform.localPosition;
        }

        private void Update()
        {
            if (!enableBobbing) return;

            CheckMovement();
            ApplyBobbing();
        }

        private void CheckMovement()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            isMoving = Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;

            // Check if grounded (optional - requires CharacterController)
            CharacterController controller = GetComponentInParent<CharacterController>();
            if (controller != null && !controller.isGrounded)
            {
                isMoving = false;
            }
        }

        private void ApplyBobbing()
        {
            if (isMoving)
            {
                // Check if sprinting
                bool isSprinting = Input.GetKey(KeyCode.LeftShift);
                float bobbingSpeed = isSprinting ? sprintBobbingSpeed : walkBobbingSpeed;
                float bobbingAmount = isSprinting ? sprintBobbingAmount : walkBobbingAmount;

                timer += Time.deltaTime * bobbingSpeed;

                // Vertical bobbing (sine wave)
                float verticalOffset = Mathf.Sin(timer) * bobbingAmount;

                // Horizontal bobbing (slower, half frequency)
                float horizontalOffset = 0f;
                if (enableHorizontalBob)
                {
                    horizontalOffset = Mathf.Cos(timer * 0.5f) * horizontalBobbingAmount;
                }

                Vector3 newPosition = originalPosition;
                newPosition.y += verticalOffset;
                newPosition.x += horizontalOffset;

                transform.localPosition = newPosition;
            }
            else
            {
                // Return to original position when not moving
                timer = 0f;
                transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * 5f);
            }
        }

        public void SetEnabled(bool enabled)
        {
            enableBobbing = enabled;
            if (!enabled)
            {
                transform.localPosition = originalPosition;
            }
        }
    }
}
