using UnityEngine;
using UnityEngine.Events;
using UnityMechanicsToolkit.Core;

namespace UnityMechanicsToolkit.Combat
{
    /// <summary>
    /// Block and dodge mechanics for defensive gameplay.
    /// Integrates with stamina system.
    /// </summary>
    public class BlockDodge : MonoBehaviour
    {
        [Header("Block Settings")]
        [SerializeField] private KeyCode blockKey = KeyCode.Mouse1; // Right click
        [SerializeField] private float blockDamageReduction = 0.5f; // 50% reduction
        [SerializeField] private float blockStaminaDrain = 10f; // Per second while blocking

        [Header("Dodge Settings")]
        [SerializeField] private KeyCode dodgeKey = KeyCode.LeftControl;
        [SerializeField] private float dodgeDistance = 3f;
        [SerializeField] private float dodgeDuration = 0.5f;
        [SerializeField] private float dodgeCooldown = 1f;
        [SerializeField] private float dodgeStaminaCost = 20f;

        [Header("Events")]
        public UnityEvent onBlockStart;
        public UnityEvent onBlockEnd;
        public UnityEvent onDodge;

        private StaminaSystem stamina;
        private CharacterController controller;
        private bool isBlocking;
        private bool isDodging;
        private float dodgeTimer;
        private float lastDodgeTime;
        private Vector3 dodgeDirection;

        public bool IsBlocking => isBlocking;
        public bool IsDodging => isDodging;

        private void Awake()
        {
            stamina = GetComponent<StaminaSystem>();
            controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            HandleBlock();
            HandleDodge();
        }

        private void HandleBlock()
        {
            if (Input.GetKey(blockKey))
            {
                if (!isBlocking)
                {
                    StartBlock();
                }

                // Drain stamina while blocking
                if (stamina != null)
                {
                    stamina.Consume(blockStaminaDrain * Time.deltaTime);

                    // Stop blocking if out of stamina
                    if (stamina.IsDepleted)
                    {
                        StopBlock();
                    }
                }
            }
            else
            {
                if (isBlocking)
                {
                    StopBlock();
                }
            }
        }

        private void HandleDodge()
        {
            if (isDodging)
            {
                dodgeTimer -= Time.deltaTime;

                // Move during dodge
                if (controller != null)
                {
                    controller.Move(dodgeDirection * (dodgeDistance / dodgeDuration) * Time.deltaTime);
                }

                if (dodgeTimer <= 0)
                {
                    isDodging = false;
                }
            }
            else
            {
                if (Input.GetKeyDown(dodgeKey))
                {
                    TryDodge();
                }
            }
        }

        private void StartBlock()
        {
            isBlocking = true;
            onBlockStart?.Invoke();
            Debug.Log("[BlockDodge] Blocking...");
        }

        private void StopBlock()
        {
            isBlocking = false;
            onBlockEnd?.Invoke();
            Debug.Log("[BlockDodge] Stopped blocking.");
        }

        private void TryDodge()
        {
            // Check cooldown
            if (Time.time < lastDodgeTime + dodgeCooldown)
            {
                return;
            }

            // Check stamina
            if (stamina != null && !stamina.TryConsume(dodgeStaminaCost))
            {
                Debug.Log("[BlockDodge] Not enough stamina to dodge!");
                return;
            }

            Dodge();
        }

        private void Dodge()
        {
            isDodging = true;
            dodgeTimer = dodgeDuration;
            lastDodgeTime = Time.time;

            // Determine dodge direction based on input
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 inputDirection = (transform.right * horizontal + transform.forward * vertical).normalized;

            // Default to backward dodge if no input
            dodgeDirection = inputDirection != Vector3.zero ? inputDirection : -transform.forward;

            onDodge?.Invoke();
            Debug.Log("[BlockDodge] Dodge!");
        }

        /// <summary>
        /// Apply damage reduction if blocking. Call this from Health system.
        /// </summary>
        public float ApplyBlockReduction(float incomingDamage)
        {
            if (isBlocking)
            {
                return incomingDamage * blockDamageReduction;
            }
            return incomingDamage;
        }
    }
}
