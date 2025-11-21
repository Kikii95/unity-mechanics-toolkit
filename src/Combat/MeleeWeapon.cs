using UnityEngine;
using UnityEngine.Events;
using UnityMechanicsToolkit.Core;

namespace UnityMechanicsToolkit.Combat
{
    /// <summary>
    /// Melee weapon system with swing animation, durability, and damage zones.
    /// Integrates with stamina system.
    /// </summary>
    public class MeleeWeapon : MonoBehaviour
    {
        [Header("Weapon Settings")]
        [SerializeField] private float damage = 25f;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attackCooldown = 1f;
        [SerializeField] private LayerMask damageableLayer;

        [Header("Stamina")]
        [SerializeField] private bool useStamina = true;
        [SerializeField] private float staminaCost = 15f;

        [Header("Durability")]
        [SerializeField] private bool hasDurability = true;
        [SerializeField] private float maxDurability = 100f;
        [SerializeField] private float currentDurability;
        [SerializeField] private float durabilityLossPerHit = 5f;

        [Header("Animation")]
        [SerializeField] private Animator animator;
        [SerializeField] private string attackTrigger = "Attack";

        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip swingSound;
        [SerializeField] private AudioClip hitSound;

        [Header("Events")]
        public UnityEvent onAttack;
        public UnityEvent<GameObject> onHit;
        public UnityEvent onWeaponBroken;

        private float lastAttackTime;
        private StaminaSystem stamina;
        private bool isBroken;

        public float Damage => damage;
        public float CurrentDurability => currentDurability;
        public float DurabilityPercentage => currentDurability / maxDurability;
        public bool IsBroken => isBroken;

        private void Awake()
        {
            currentDurability = maxDurability;
            stamina = GetComponentInParent<StaminaSystem>();

            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) // Left click
            {
                TryAttack();
            }
        }

        public void TryAttack()
        {
            if (isBroken)
            {
                Debug.Log("[MeleeWeapon] Weapon is broken!");
                return;
            }

            // Check cooldown
            if (Time.time < lastAttackTime + attackCooldown)
            {
                return;
            }

            // Check stamina
            if (useStamina && stamina != null)
            {
                if (!stamina.TryConsume(staminaCost))
                {
                    Debug.Log("[MeleeWeapon] Not enough stamina!");
                    return;
                }
            }

            Attack();
        }

        private void Attack()
        {
            lastAttackTime = Time.time;

            // Play animation
            if (animator != null)
            {
                animator.SetTrigger(attackTrigger);
            }

            // Play swing sound
            if (audioSource != null && swingSound != null)
            {
                audioSource.PlayOneShot(swingSound);
            }

            // Raycast for hit detection
            RaycastHit hit;
            Vector3 attackOrigin = transform.position + transform.forward * 0.5f;

            if (Physics.SphereCast(attackOrigin, 0.3f, transform.forward, out hit, attackRange, damageableLayer))
            {
                // Hit something
                IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(damage);
                    onHit?.Invoke(hit.collider.gameObject);

                    // Play hit sound
                    if (audioSource != null && hitSound != null)
                    {
                        audioSource.PlayOneShot(hitSound);
                    }

                    // Reduce durability
                    if (hasDurability)
                    {
                        currentDurability -= durabilityLossPerHit;

                        if (currentDurability <= 0)
                        {
                            currentDurability = 0;
                            isBroken = true;
                            onWeaponBroken?.Invoke();
                            Debug.Log("[MeleeWeapon] Weapon broken!");
                        }
                    }
                }
            }

            onAttack?.Invoke();
        }

        public void RepairWeapon(float amount)
        {
            if (!hasDurability) return;

            currentDurability = Mathf.Min(maxDurability, currentDurability + amount);

            if (currentDurability > 0)
            {
                isBroken = false;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = isBroken ? Color.red : Color.green;
            Vector3 attackOrigin = transform.position + transform.forward * 0.5f;
            Gizmos.DrawWireSphere(attackOrigin, 0.3f);
            Gizmos.DrawRay(attackOrigin, transform.forward * attackRange);
        }
#endif
    }
}
