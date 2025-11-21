using UnityEngine;

namespace UnityMechanicsToolkit.Camera
{
    /// <summary>
    /// Resident Evil style look behind mechanic.
    /// Press key to rotate camera 180 degrees to look behind player.
    /// </summary>
    public class LookBehind : MonoBehaviour
    {
        [Header("Look Behind Settings")]
        [SerializeField] private KeyCode lookBehindKey = KeyCode.V;
        [SerializeField] private bool holdToLook = true; // Hold or toggle
        [SerializeField] private float rotationSpeed = 10f;

        [Header("Camera Offset")]
        [SerializeField] private float behindCameraOffset = 0.3f; // Slight upward angle

        private Transform cameraTransform;
        private bool isLookingBehind;
        private Quaternion targetRotation;
        private Quaternion normalRotation;

        private void Awake()
        {
            cameraTransform = transform;
            normalRotation = cameraTransform.localRotation;
        }

        private void Update()
        {
            HandleInput();
            UpdateRotation();
        }

        private void HandleInput()
        {
            if (holdToLook)
            {
                // Hold to look behind
                if (Input.GetKeyDown(lookBehindKey))
                {
                    StartLookBehind();
                }
                else if (Input.GetKeyUp(lookBehindKey))
                {
                    StopLookBehind();
                }
            }
            else
            {
                // Toggle look behind
                if (Input.GetKeyDown(lookBehindKey))
                {
                    if (isLookingBehind)
                    {
                        StopLookBehind();
                    }
                    else
                    {
                        StartLookBehind();
                    }
                }
            }
        }

        private void StartLookBehind()
        {
            isLookingBehind = true;
            targetRotation = Quaternion.Euler(-behindCameraOffset * 10f, 180f, 0f);
        }

        private void StopLookBehind()
        {
            isLookingBehind = false;
            targetRotation = normalRotation;
        }

        private void UpdateRotation()
        {
            Quaternion currentTarget = isLookingBehind ? targetRotation : normalRotation;
            cameraTransform.localRotation = Quaternion.Slerp(
                cameraTransform.localRotation,
                currentTarget,
                Time.deltaTime * rotationSpeed
            );
        }

        public bool IsLookingBehind()
        {
            return isLookingBehind;
        }
    }
}
