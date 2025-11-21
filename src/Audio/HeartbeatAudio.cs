using UnityEngine;
using UnityMechanicsToolkit.Horror;

namespace UnityMechanicsToolkit.Audio
{
    /// <summary>
    /// Dynamic heartbeat audio based on danger/sanity levels.
    /// Integrates with SanitySystem and AI detection.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class HeartbeatAudio : MonoBehaviour
    {
        [Header("Heartbeat Settings")]
        [SerializeField] private AudioClip heartbeatClip;
        [SerializeField] private float normalPitch = 1f;
        [SerializeField] private float panicPitch = 1.5f;
        [SerializeField] private float normalVolume = 0.3f;
        [SerializeField] private float panicVolume = 0.8f;

        [Header("Triggers")]
        [SerializeField] private bool reactToSanity = true;
        [SerializeField] private float lowSanityThreshold = 30f;
        [SerializeField] private bool reactToEnemies = true;
        [SerializeField] private float enemyDetectionRadius = 10f;
        [SerializeField] private LayerMask enemyLayer;

        private AudioSource audioSource;
        private SanitySystem sanitySystem;
        private float currentIntensity;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            sanitySystem = GetComponent<SanitySystem>();

            if (audioSource != null && heartbeatClip != null)
            {
                audioSource.clip = heartbeatClip;
                audioSource.loop = true;
                audioSource.playOnAwake = false;
            }
        }

        private void Update()
        {
            CalculateIntensity();
            ApplyIntensity();
        }

        private void CalculateIntensity()
        {
            float targetIntensity = 0f;

            // Sanity-based intensity
            if (reactToSanity && sanitySystem != null)
            {
                if (sanitySystem.CurrentSanity < lowSanityThreshold)
                {
                    targetIntensity = Mathf.Max(targetIntensity, 1f - sanitySystem.SanityPercentage);
                }
            }

            // Enemy proximity intensity
            if (reactToEnemies)
            {
                Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, enemyDetectionRadius, enemyLayer);
                if (nearbyEnemies.Length > 0)
                {
                    float closestDistance = Mathf.Infinity;
                    foreach (Collider enemy in nearbyEnemies)
                    {
                        float distance = Vector3.Distance(transform.position, enemy.transform.position);
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                        }
                    }

                    // Closer = higher intensity
                    float proximityIntensity = 1f - (closestDistance / enemyDetectionRadius);
                    targetIntensity = Mathf.Max(targetIntensity, proximityIntensity);
                }
            }

            // Smooth intensity transition
            currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime * 2f);
        }

        private void ApplyIntensity()
        {
            if (audioSource == null) return;

            if (currentIntensity > 0.1f)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }

                audioSource.volume = Mathf.Lerp(normalVolume, panicVolume, currentIntensity);
                audioSource.pitch = Mathf.Lerp(normalPitch, panicPitch, currentIntensity);
            }
            else
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
        }
    }
}
