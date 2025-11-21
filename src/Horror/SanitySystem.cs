using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;

namespace UnityMechanicsToolkit.Horror
{
    /// <summary>
    /// Amnesia-style sanity system with visual/audio effects and gameplay penalties.
    /// Decreases in darkness, near enemies, or when witnessing scary events.
    /// </summary>
    public class SanitySystem : MonoBehaviour
    {
        [Header("Sanity Settings")]
        [SerializeField] private float maxSanity = 100f;
        [SerializeField] private float currentSanity;

        [Header("Drain Settings")]
        [SerializeField] private bool drainInDarkness = true;
        [SerializeField] private float darknessDrainRate = 2f; // Per second
        [SerializeField] private float lightThreshold = 0.3f; // Below this = darkness

        [SerializeField] private bool drainNearEnemies = true;
        [SerializeField] private float enemyDrainRate = 5f;
        [SerializeField] private float enemyDetectionRadius = 10f;
        [SerializeField] private LayerMask enemyLayer;

        [Header("Recovery")]
        [SerializeField] private bool canRecover = true;
        [SerializeField] private float recoveryRate = 1f; // Per second in light
        [SerializeField] private float recoveryDelay = 3f;

        [Header("Visual Effects")]
        [SerializeField] private bool enableVisualEffects = true;
        [SerializeField] private PostProcessVolume postProcessVolume;
        [SerializeField] private float maxVignetteIntensity = 0.6f;
        [SerializeField] private float maxChromaticAberration = 1f;

        [Header("Audio Effects")]
        [SerializeField] private AudioSource heartbeatAudio;
        [SerializeField] private AudioSource whispersAudio;
        [SerializeField] private float lowSanityThreshold = 30f;

        [Header("Gameplay Penalties")]
        [SerializeField] private bool enablePenalties = true;
        [SerializeField] private float minSanityForPenalties = 20f;

        [Header("Events")]
        public UnityEvent<float> onSanityChanged;
        public UnityEvent onSanityLow;
        public UnityEvent onSanityDepleted;

        private float timeSinceLastDrain;
        private Vignette vignette;
        private ChromaticAberration chromaticAberration;

        public float CurrentSanity => currentSanity;
        public float SanityPercentage => currentSanity / maxSanity;
        public bool IsLowSanity => currentSanity < lowSanityThreshold;

        private void Awake()
        {
            currentSanity = maxSanity;

            if (enableVisualEffects && postProcessVolume != null)
            {
                postProcessVolume.profile.TryGetSettings(out vignette);
                postProcessVolume.profile.TryGetSettings(out chromaticAberration);
            }
        }

        private void Update()
        {
            HandleSanityDrain();
            HandleRecovery();
            ApplyEffects();
        }

        private void HandleSanityDrain()
        {
            bool isDraining = false;

            // Drain in darkness
            if (drainInDarkness && IsInDarkness())
            {
                DecreaseSanity(darknessDrainRate * Time.deltaTime);
                isDraining = true;
            }

            // Drain near enemies
            if (drainNearEnemies && IsNearEnemy())
            {
                DecreaseSanity(enemyDrainRate * Time.deltaTime);
                isDraining = true;
            }

            if (isDraining)
            {
                timeSinceLastDrain = 0f;
            }
        }

        private void HandleRecovery()
        {
            if (!canRecover || currentSanity >= maxSanity) return;

            timeSinceLastDrain += Time.deltaTime;

            if (timeSinceLastDrain >= recoveryDelay && !IsInDarkness())
            {
                IncreaseSanity(recoveryRate * Time.deltaTime);
            }
        }

        public void DecreaseSanity(float amount)
        {
            amount = Mathf.Abs(amount);
            currentSanity = Mathf.Max(0, currentSanity - amount);

            onSanityChanged?.Invoke(currentSanity);

            if (IsLowSanity)
            {
                onSanityLow?.Invoke();
            }

            if (currentSanity <= 0)
            {
                onSanityDepleted?.Invoke();
            }
        }

        public void IncreaseSanity(float amount)
        {
            amount = Mathf.Abs(amount);
            currentSanity = Mathf.Min(maxSanity, currentSanity + amount);
            onSanityChanged?.Invoke(currentSanity);
        }

        private bool IsInDarkness()
        {
            // Check if flashlight is on and pointed forward
            FlashlightSystem flashlight = GetComponent<FlashlightSystem>();
            if (flashlight != null && flashlight.IsOn && flashlight.CurrentBattery > 0)
            {
                return false; // Light source available
            }

            // Could add ambient light check here
            return true;
        }

        private bool IsNearEnemy()
        {
            Collider[] enemies = Physics.OverlapSphere(transform.position, enemyDetectionRadius, enemyLayer);
            return enemies.Length > 0;
        }

        private void ApplyEffects()
        {
            float sanityFactor = 1f - SanityPercentage; // 0 = full sanity, 1 = zero sanity

            // Visual effects
            if (enableVisualEffects)
            {
                if (vignette != null)
                {
                    vignette.intensity.value = Mathf.Lerp(0f, maxVignetteIntensity, sanityFactor);
                }

                if (chromaticAberration != null)
                {
                    chromaticAberration.intensity.value = Mathf.Lerp(0f, maxChromaticAberration, sanityFactor);
                }
            }

            // Audio effects
            if (heartbeatAudio != null && IsLowSanity)
            {
                heartbeatAudio.volume = Mathf.Lerp(0f, 1f, sanityFactor);
                if (!heartbeatAudio.isPlaying)
                {
                    heartbeatAudio.Play();
                }
            }

            if (whispersAudio != null && currentSanity < minSanityForPenalties)
            {
                whispersAudio.volume = Mathf.Lerp(0f, 0.5f, sanityFactor);
                if (!whispersAudio.isPlaying)
                {
                    whispersAudio.Play();
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (drainNearEnemies)
            {
                Gizmos.color = IsLowSanity ? Color.red : Color.yellow;
                Gizmos.DrawWireSphere(transform.position, enemyDetectionRadius);
            }
        }
#endif
    }
}
