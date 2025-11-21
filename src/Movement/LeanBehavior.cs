using UnityEngine;

namespace UnityMechanicsToolkit.Movement
{
    /// <summary>
    /// Lean left/right mechanic for peeking around corners.
    /// Tilts camera and shifts position slightly.
    /// </summary>
    public class LeanBehavior : MonoBehaviour
    {
        [Header("Lean Settings")]
        [SerializeField] private KeyCode leanLeftKey = KeyCode.A; // AZERTY
        [SerializeField] private KeyCode leanRightKey = KeyCode.E; // AZERTY
        [SerializeField] private float leanAngle = 15f;
        [SerializeField] private float leanDistance = 0.5f; // Sideways shift
        [SerializeField] private float leanSpeed = 5f;

        [Header("Camera")]
        [SerializeField] private Transform cameraTransform;

        private float currentLeanAngle;
        private float targetLeanAngle;
        private Vector3 originalCameraPosition;
        private Vector3 targetCameraOffset;

        private enum LeanDirection { None, Left, Right }
        private LeanDirection currentLean = LeanDirection.None;

        private void Awake()
        {
            if (cameraTransform == null)
            {
                cameraTransform = Camera.main?.transform;
            }

            if (cameraTransform != null)
            {
                originalCameraPosition = cameraTransform.localPosition;
            }
        }

        private void Update()
        {
            HandleInput();
            UpdateLean();
        }

        private void HandleInput()
        {
            // Check if trying to lean
            bool leaningLeft = Input.GetKey(leanLeftKey);
            bool leaningRight = Input.GetKey(leanRightKey);

            // Can't lean both ways at once
            if (leaningLeft && leaningRight)
            {
                leaningLeft = false;
                leaningRight = false;
            }

            // Set target lean
            if (leaningLeft)
            {
                currentLean = LeanDirection.Left;
                targetLeanAngle = leanAngle;
                targetCameraOffset = Vector3.left * leanDistance;
            }
            else if (leaningRight)
            {
                currentLean = LeanDirection.Right;
                targetLeanAngle = -leanAngle;
                targetCameraOffset = Vector3.right * leanDistance;
            }
            else
            {
                currentLean = LeanDirection.None;
                targetLeanAngle = 0f;
                targetCameraOffset = Vector3.zero;
            }
        }

        private void UpdateLean()
        {
            if (cameraTransform == null) return;

            // Smooth lean angle transition
            currentLeanAngle = Mathf.Lerp(currentLeanAngle, targetLeanAngle, Time.deltaTime * leanSpeed);

            // Apply camera rotation (Z-axis tilt)
            Quaternion targetRotation = Quaternion.Euler(
                cameraTransform.localRotation.eulerAngles.x,
                cameraTransform.localRotation.eulerAngles.y,
                currentLeanAngle
            );

            cameraTransform.localRotation = Quaternion.Slerp(
                cameraTransform.localRotation,
                targetRotation,
                Time.deltaTime * leanSpeed
            );

            // Apply camera position shift
            Vector3 targetPosition = originalCameraPosition + cameraTransform.parent.TransformDirection(targetCameraOffset);
            cameraTransform.localPosition = Vector3.Lerp(
                cameraTransform.localPosition,
                targetPosition,
                Time.deltaTime * leanSpeed
            );
        }

        public bool IsLeaning()
        {
            return currentLean != LeanDirection.None;
        }

        public LeanDirection GetLeanDirection()
        {
            return currentLean;
        }
    }
}
