using UnityEngine;

namespace UnityMechanicsToolkit.Camera
{
    /// <summary>
    /// Camera shake effect for impacts, explosions, and monster footsteps.
    /// Supports both positional and rotational shake.
    /// </summary>
    public class CameraShake : MonoBehaviour
    {
        [Header("Shake Settings")]
        [SerializeField] private float defaultIntensity = 0.3f;
        [SerializeField] private float defaultDuration = 0.5f;
        [SerializeField] private float shakeFrequency = 25f;

        [Header("Shake Type")]
        [SerializeField] private bool shakePosition = true;
        [SerializeField] private bool shakeRotation = true;
        [SerializeField] private float rotationMultiplier = 2f;

        private Vector3 originalPosition;
        private Quaternion originalRotation;
        private float shakeTimer;
        private float currentIntensity;
        private bool isShaking;

        private void Awake()
        {
            originalPosition = transform.localPosition;
            originalRotation = transform.localRotation;
        }

        private void Update()
        {
            if (isShaking)
            {
                shakeTimer -= Time.deltaTime;

                if (shakeTimer <= 0)
                {
                    StopShake();
                }
                else
                {
                    ApplyShake();
                }
            }
        }

        /// <summary>
        /// Trigger camera shake with custom parameters.
        /// </summary>
        public void Shake(float intensity, float duration)
        {
            currentIntensity = intensity;
            shakeTimer = duration;
            isShaking = true;
        }

        /// <summary>
        /// Trigger camera shake with default parameters.
        /// </summary>
        public void Shake()
        {
            Shake(defaultIntensity, defaultDuration);
        }

        private void ApplyShake()
        {
            // Calculate shake amount (decreases over time)
            float shakeAmount = currentIntensity * (shakeTimer / defaultDuration);

            // Position shake
            if (shakePosition)
            {
                float offsetX = (Mathf.PerlinNoise(Time.time * shakeFrequency, 0f) - 0.5f) * shakeAmount;
                float offsetY = (Mathf.PerlinNoise(0f, Time.time * shakeFrequency) - 0.5f) * shakeAmount;
                float offsetZ = (Mathf.PerlinNoise(Time.time * shakeFrequency, Time.time * shakeFrequency) - 0.5f) * shakeAmount;

                transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, offsetZ);
            }

            // Rotation shake
            if (shakeRotation)
            {
                float rotX = (Mathf.PerlinNoise(Time.time * shakeFrequency + 1f, 0f) - 0.5f) * shakeAmount * rotationMultiplier;
                float rotY = (Mathf.PerlinNoise(0f, Time.time * shakeFrequency + 1f) - 0.5f) * shakeAmount * rotationMultiplier;
                float rotZ = (Mathf.PerlinNoise(Time.time * shakeFrequency + 1f, Time.time * shakeFrequency + 1f) - 0.5f) * shakeAmount * rotationMultiplier;

                Quaternion shakeRotation = Quaternion.Euler(rotX, rotY, rotZ);
                transform.localRotation = originalRotation * shakeRotation;
            }
        }

        private void StopShake()
        {
            isShaking = false;
            transform.localPosition = originalPosition;
            transform.localRotation = originalRotation;
        }

        /// <summary>
        /// Static method to shake camera from anywhere in code.
        /// </summary>
        public static void TriggerShake(float intensity, float duration)
        {
            CameraShake cameraShake = FindObjectOfType<CameraShake>();
            if (cameraShake != null)
            {
                cameraShake.Shake(intensity, duration);
            }
        }
    }
}
