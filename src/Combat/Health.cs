using UnityEngine;
using UnityEngine.Events;

namespace UnityMechanicsToolkit.Combat
{
    public class Health : MonoBehaviour
    {
        [Header("Health Settings")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float currentHealth;
        [SerializeField] private bool invulnerable = false;

        [Header("Regeneration")]
        [SerializeField] private bool canRegenerate = false;
        [SerializeField] private float regenRate = 5f;
        [SerializeField] private float regenDelay = 3f;

        private float timeSinceLastDamage;

        [Header("Events")]
        public UnityEvent<float> onHealthChanged;
        public UnityEvent<float> onDamaged;
        public UnityEvent<float> onHealed;
        public UnityEvent onDeath;

        public float MaxHealth => maxHealth;
        public float CurrentHealth => currentHealth;
        public float HealthPercentage => currentHealth / maxHealth;
        public bool IsAlive => currentHealth > 0;
        public bool IsDead => currentHealth <= 0;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        private void Update()
        {
            if (canRegenerate && currentHealth < maxHealth && IsAlive)
            {
                timeSinceLastDamage += Time.deltaTime;

                if (timeSinceLastDamage >= regenDelay)
                {
                    Heal(regenRate * Time.deltaTime);
                }
            }
        }

        public void TakeDamage(float amount)
        {
            if (invulnerable || IsDead) return;

            amount = Mathf.Abs(amount);
            currentHealth = Mathf.Max(0, currentHealth - amount);

            timeSinceLastDamage = 0f;

            onDamaged?.Invoke(amount);
            onHealthChanged?.Invoke(currentHealth);

            if (IsDead)
            {
                Die();
            }
        }

        public void Heal(float amount)
        {
            if (IsDead) return;

            amount = Mathf.Abs(amount);
            float previousHealth = currentHealth;
            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);

            float actualHealAmount = currentHealth - previousHealth;

            if (actualHealAmount > 0)
            {
                onHealed?.Invoke(actualHealAmount);
                onHealthChanged?.Invoke(currentHealth);
            }
        }

        public void SetHealth(float amount)
        {
            currentHealth = Mathf.Clamp(amount, 0, maxHealth);
            onHealthChanged?.Invoke(currentHealth);

            if (IsDead)
            {
                Die();
            }
        }

        public void ResetHealth()
        {
            currentHealth = maxHealth;
            onHealthChanged?.Invoke(currentHealth);
        }

        public void SetMaxHealth(float newMax, bool healToMax = false)
        {
            maxHealth = Mathf.Max(1f, newMax);

            if (healToMax)
            {
                currentHealth = maxHealth;
            }
            else
            {
                currentHealth = Mathf.Min(currentHealth, maxHealth);
            }

            onHealthChanged?.Invoke(currentHealth);
        }

        private void Die()
        {
            onDeath?.Invoke();
        }

        public void SetInvulnerable(bool state)
        {
            invulnerable = state;
        }
    }
}
