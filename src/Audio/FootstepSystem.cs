using UnityEngine;
using UnityMechanicsToolkit.Core;

namespace UnityMechanicsToolkit.Audio
{
    /// <summary>
    /// Footstep system with material detection (wood, metal, concrete, etc.).
    /// Plays footstep sounds based on surface and movement speed.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class FootstepSystem : MonoBehaviour
    {
        [System.Serializable]
        public class SurfaceSounds
        {
            public string surfaceTag = "Default";
            public AudioClip[] footstepClips;
        }

        [Header("Footstep Settings")]
        [SerializeField] private SurfaceSounds[] surfaceSounds;
        [SerializeField] private float walkStepInterval = 0.5f;
        [SerializeField] private float runStepInterval = 0.3f;

        [Header("Volume")]
        [SerializeField] private float walkVolume = 0.3f;
        [SerializeField] private float runVolume = 0.6f;

        private AudioSource audioSource;
        private float stepTimer;
        private bool isMoving;
        private bool isSprinting;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            CheckMovement();
            HandleFootsteps();
        }

        private void CheckMovement()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            isMoving = Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;
            isSprinting = Input.GetKey(KeyCode.LeftShift);

            // Check if grounded
            CharacterController controller = GetComponent<CharacterController>();
            if (controller != null && !controller.isGrounded)
            {
                isMoving = false;
            }
        }

        private void HandleFootsteps()
        {
            if (!isMoving) return;

            float stepInterval = isSprinting ? runStepInterval : walkStepInterval;
            stepTimer += Time.deltaTime;

            if (stepTimer >= stepInterval)
            {
                stepTimer = 0f;
                PlayFootstep();
            }
        }

        private void PlayFootstep()
        {
            // Detect surface below
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
            {
                string surfaceTag = hit.collider.tag;
                AudioClip footstepClip = GetFootstepClip(surfaceTag);

                if (footstepClip != null)
                {
                    float volume = isSprinting ? runVolume : walkVolume;
                    audioSource.PlayOneShot(footstepClip, volume);

                    // Emit event for AI hearing
                    GameEvents.PlayerFootstep(transform.position, volume);
                }
            }
        }

        private AudioClip GetFootstepClip(string surfaceTag)
        {
            foreach (SurfaceSounds surface in surfaceSounds)
            {
                if (surface.surfaceTag == surfaceTag && surface.footstepClips.Length > 0)
                {
                    return surface.footstepClips[Random.Range(0, surface.footstepClips.Length)];
                }
            }

            // Default fallback
            if (surfaceSounds.Length > 0 && surfaceSounds[0].footstepClips.Length > 0)
            {
                return surfaceSounds[0].footstepClips[Random.Range(0, surfaceSounds[0].footstepClips.Length)];
            }

            return null;
        }
    }
}
